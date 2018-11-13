using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterSheetGenerator : MonoBehaviour
{
    public InputField inputID;
    public IpcGetService getService;
    public Interpreter interp;
    public InterpretFromCache interp2;
    public bool usedCachedData;

    public IPC_Data dataToUse;

    public void CreateSheet()
    {
        if (!usedCachedData)
        {
            StartCoroutine("LoadOneIPC");
        }
        else
        {
            print("called !!");
            dataToUse = DataInitializer.Instance.GetIpcData(int.Parse(inputID.text));
            interp2.ipcData = dataToUse;
            interp2.Interpret();

        }
    }

    public void ChangeSheet(int add)
    {
        int newNum = int.Parse(inputID.text);
        newNum += add;
        inputID.text = newNum.ToString();

        dataToUse = DataInitializer.Instance.GetIpcData(newNum);
        interp2.ipcData = dataToUse;
        interp2.Interpret();
    }

    // Waits until the IPC is loaded before character creation
    private IEnumerator LoadOneIPC()
    {
            StartCoroutine(getService.GetOneIpc(int.Parse(inputID.text)));
            yield return new WaitUntil(() => getService.OneIPCLoaded == true);
            interp.Interpret();

        
    }
}
