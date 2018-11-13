using UnityEngine;
using UnityEngine.UI;

public class SelectIPCButton : MonoBehaviour 
{
    public Text ipcNameText;
    public Text ipcIDText;
    private int _id;
    GameObject ipcGameobject;

    public void SetText(string name, int ID, GameObject ipcObject)
    {
        ipcGameobject = ipcObject;
        ipcNameText.text = name;
        _id = ID;
        ipcIDText.text = "#" + _id;
    }

    public void SelectIpcByButton()
    {
        IPC_Controller.Instance.SelectIPC(ipcGameobject);
        
        //if(FindObjectOfType<IpcSpawnMenu>())
        //{
        //    FindObjectOfType<IpcSpawnMenu>().SelectIPCbyID(_id);
        //}
    }

    public void DeleteIpc()
    {
        Destroy(ipcGameobject);
        Destroy(gameObject);
    }
}
