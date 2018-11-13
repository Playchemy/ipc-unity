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

public class BuyIPC : MonoBehaviour {

    private IpcContractService _ipcContractService;
    private string _url = @"https://mainnet.infura.io/BO2vivj28SMFpdc2rufp";

    public string addressfrom;
    public string addressowner;
    public string privkey;
    public BigInteger idToBuy = 565;
    public HexBigInteger gass = new HexBigInteger(50) ;
    public HexBigInteger valuetoSet = new HexBigInteger(3232);


    // Use this for initialization
    void Start ()
    {
        _ipcContractService = new IpcContractService();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("BUY IPC")]
    public void BuyIt()
    {
        _ipcContractService.CreateBuyIpcTransactionInput(addressfrom, addressowner, privkey, idToBuy, gass, valuetoSet);
        print(idToBuy);
        print(gass);
        print(valuetoSet);
    }
}
