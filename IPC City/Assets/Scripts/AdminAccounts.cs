using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdminAccounts", menuName = "Admin Account Data", order = 1)]
public class AdminAccounts : ScriptableObject
{
    public List<AdminAccountAccessory> adminAccounts;
    public List<Ipc_SpriteData> accessoryList;

    public void AddEntry(string id, int accessoryNum)
    {
        AdminAccountAccessory newEntry = new AdminAccountAccessory
        {
            adminAccountID = "#" + id,
            assignedAccessory = GetAccessory(accessoryNum)
        };
        adminAccounts.Add(newEntry);
    }

    Ipc_SpriteData GetAccessory(int accessoryID)
    {
        return accessoryList[accessoryID];
    }
}

[System.Serializable]
public class AdminAccountAccessory
{
    public string adminAccountID;

    public Ipc_SpriteData assignedAccessory;
}
