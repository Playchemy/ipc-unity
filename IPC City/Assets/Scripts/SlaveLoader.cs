using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveLoader : MonoBehaviour
{
    public void LoadToMainList (IpcGetService gService)
    {
        IPC_Data data = new IPC_Data();
        data.name = gService.inputIPCID.ToString();

        data.ipc_id = gService.inputIPCID;
        data.ipc_name = gService.ipcStorage.m_name;
        data.ipc_owner = gService.ipcStorage.m_owner;
        data.ipc_timeOfBirth = gService.ipcStorage.m_timeOfBirth;
        data.ipc_dna = gService.ipcStorage.m_dna;
        data.ipc_attributes = gService.ipcStorage.m_attributes;
        data.ipc_price = gService.ipcStorage.m_price;

        DataInitializer.Instance.ipcList.Add(data);
        DataInitializer.Instance.CompareListCount();
        Destroy(gameObject);
	}
}
