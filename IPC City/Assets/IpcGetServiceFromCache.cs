using System.Collections;
using UnityEngine;

public class IpcGetServiceFromCache : IpcGetService
{
    private void Start()
    {
        if (GetComponent<SpriteHandler>())
        {
            GetSingleIPC();
        }
    }

    [ContextMenu("GetOwnerTokens")]
    public void GetOwnerTokens()
    {
        StartCoroutine("GetTokensOfOwner", inputWalletID);
    }

    public override IEnumerator GetIpcCount()
    {
        IPCCountLoaded = false;

        yield return new WaitForEndOfFrame();
        ipcCount = 0;

        IPCCountLoaded = true;
    }

    public override IEnumerator GetOneIpc(int ipcId)
    {
        OneIPCLoaded = false;
        inputIPCID = ipcId;
        IPC_Data ipc = DataInitializer.Instance.GetIpcData(inputIPCID);
        ipcStorage = new IpcInfo
        {
            m_name = ipc.ipc_name,
            m_timeOfBirth = ipc.ipc_timeOfBirth,
            m_xp = ipc.ipc_xp,
            m_dna = ipc.ipc_dna,
            m_attributes = ipc.ipc_attributes
        };


        ipcStorage.m_price = ipc.ipc_price;
        string owner = ipc.ipc_owner;
        ipcStorage.m_owner = owner;

        yield return new WaitForEndOfFrame();

        OneIPCLoaded = true;

        if (GetComponent<Interpreter>())
        {
            GetComponent<Interpreter>().Interpret();
            if (GetComponent<CharController>())
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

        if (GetComponent<SlaveLoader>())
        {
            GetComponent<SlaveLoader>().LoadToMainList(this);
        }
    }

    public override IEnumerator GetTokensOfOwner(string address)
    {
        Debug.Log(address);
        TokensLoaded = false;
        yield return new WaitForEndOfFrame();
        ownedIpcsIds = DataInitializer.Instance.GetOwnedIpcsByAddress(address);
        Debug.Log(ownedIpcsIds.Count);
        TokensLoaded = true;
    }
}
