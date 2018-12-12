using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Runtime.InteropServices;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IpcGetService : MonoBehaviour
{
    public bool TokensLoaded = false;
    public bool OneIPCLoaded = false;
    public bool IPCCountLoaded = false;

    [System.Serializable]
    public class IpcInfo
    {
        public string m_name;
        public string m_owner;
        public uint m_timeOfBirth;
        public uint m_xp;
        public string m_dna;
        public string m_attributes;
        public uint m_price;
        public string m_beneficiaryAddress;
        public uint m_beneficiaryPrice;
    }

    public int inputIPCID;
    public string inputWalletID;

    public IpcInfo ipcStorage;

    public List<int> ownedIpcsIds;

    public int ipcCount;

    protected IpcContractService _ipcContractService;
    protected string _url = @"https://mainnet.infura.io/BO2vivj28SMFpdc2rufp";

    /*
    private string _addressOwner = "0x12890d2cce102216644c59daE5baed380d84830c";
    private string _userAddress; // = 
    private byte[] _key;
    //Partial private key to sign the transactions
    private string _privateKey = "fa002a6a5bc0f42cc9a8806ab109bf5cd2f8bb6c54d4";

    private bool submitting = false;

#if !UNITY_EDITOR
    public bool ExternalProvider = true;
    [DllImport ("__Internal")]
    private static extern string GetAccount ();
    [DllImport ("__Internal")]
    private static extern string SendTransaction (string to, string data);

#else
    public bool ExternalProvider = false;
    private static string GetAccount() { return null; }
    private static string SendTransaction(string to, string data) { return null; }
#endif
    */

    private void Start()
    {
        _ipcContractService = new IpcContractService();
        //StartCoroutine(GetIpcCount());

        if (GetComponent<SpriteHandler>())
        {
            GetSingleIPC();
        }
    }

    public void StartAgain()
    {
        //TEMP Randomizer
        inputIPCID = UnityEngine.Random.Range(1, 504);

        GetSingleIPC();
    }

    [ContextMenu("GetOneIPC")]
    public void GetSingleIPC()
    {
        StartCoroutine("GetOneIpc", inputIPCID);
    }

    [ContextMenu("GetOwnerTokens")]
    public void GetOwnerTokens()
    {
        StartCoroutine("GetTokensOfOwner", inputWalletID);
    }

    public virtual IEnumerator GetIpcCount()
    {
        IPCCountLoaded = false;

        var ipcContractRequest = new EthCallUnityRequest(_url);
        var countIpcsCallInput = _ipcContractService.CreateCountIpcsCallInput();
        yield return ipcContractRequest.SendRequest(countIpcsCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
        ipcCount = _ipcContractService.DecodeIpcCount(ipcContractRequest.Result);

        IPCCountLoaded = true;
    }

    // result stored in ipcStorage
    public virtual IEnumerator GetOneIpc(int ipcId)
    {
        OneIPCLoaded = false;

        inputIPCID = ipcId;
        // create a unity call request
        var ipcContractRequest = new EthCallUnityRequest(_url);
        var getIpcCallInput = _ipcContractService.CreateGetIpcCallInput(ipcId);
        yield return ipcContractRequest.SendRequest(getIpcCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
        GetIpcDto ipc = _ipcContractService.DecodeGetIpcDto(ipcContractRequest.Result);

        // yield return new WaitUntil(() => ipc != null);

        if(ipc == null)
        {
            print("RESTARTED BCZ IPC WAS NULL" + gameObject.name);
            //GetSingleIPC();

            yield break; 
        }

        ipcStorage = new IpcInfo
        {
            m_name = ipc.Name,
            m_timeOfBirth = ipc.TimeOfBirth,
            m_xp = ipc.Experience,
            m_dna = Nethereum.Hex.HexConvertors.Extensions.HexByteConvertorExtensions.ToHex(ipc.Dna),
            m_attributes = Nethereum.Hex.HexConvertors.Extensions.HexByteConvertorExtensions.ToHex(ipc.AttributeSeed)
        };

        ipcContractRequest = new EthCallUnityRequest(_url);
        var ipcToMarketInfoCallInput = _ipcContractService.CreateIpcToMarketInfoCallInput(ipcId);
        yield return ipcContractRequest.SendRequest(ipcToMarketInfoCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        IpcToMarketInfoDto marketInfo = _ipcContractService.DecodeIpcToMarketInfoDto(ipcContractRequest.Result);

        if (marketInfo == null)
        {
            print("RESTARTED BCZ MARKETINFO WAS NULL" + gameObject.name);
            GetSingleIPC();

            yield break;
        }

        ipcStorage.m_price = marketInfo.Price;

        ipcContractRequest = new EthCallUnityRequest(_url);
        var ownerOfCallInput = _ipcContractService.CreateOwnerOfCallInput(ipcId);
        yield return ipcContractRequest.SendRequest(ownerOfCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        string owner = _ipcContractService.DecodeGetOwner(ipcContractRequest.Result);

        ipcStorage.m_owner = owner;
        inputWalletID = owner;

        OneIPCLoaded = true;

        if (GetComponent<Interpreter>())
        {
            GetComponent<Interpreter>().Interpret();
            if(GetComponent<CharController>())
            {
                GetComponent<CharController>().enabled = true;
                GetComponent<Animator>().enabled = true;
            }

            if (GetComponent<CharMovement>())
            {
                GetComponent<CharMovement>().enabled = true;
                GetComponent<Animator>().enabled = true;
            }
        }

        if(GetComponent<SlaveLoader>())
        {
            GetComponent<SlaveLoader>().LoadToMainList(this);
        }
    }


    // result stored in ownedIpcsIds
    public virtual IEnumerator GetTokensOfOwner(string address)
    {
        TokensLoaded = false;

        var ipcContractRequest = new EthCallUnityRequest(_url);
        var tokensOfOwnerCallInput = _ipcContractService.CreateTokensOfOwnerCallInput(address);
        yield return ipcContractRequest.SendRequest(tokensOfOwnerCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
        //TokensOfOwnerDto dto = _ipcContractService.DecodeTokensOfOwner(ipcContractRequest.Result);
        ownedIpcsIds = _ipcContractService.DecodeTokensOfOwner(ipcContractRequest.Result);

        TokensLoaded = true;

        /*
        for(int i = 0; i < returnArray.Length; ++i)
        {
            if(returnArray[i] != 0)
            {
                ownedIpcsIds.Add(returnArray[i]);
            }
        }
        

        ipcContractRequest = new EthCallUnityRequest(_url);
        var ownerOfCallInput = _ipcContractService.CreateOwnerOfCallInput(ipcCount);
        yield return ipcContractRequest.SendRequest(ownerOfCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
        string returnAddress = _ipcContractService.DecodeGetOwner(ipcContractRequest.Result);
        if(returnAddress.ToLower() == address.ToLower())
        {
            ownedIpcsIds.Add(ipcCount);
        }

    */
    }
}
