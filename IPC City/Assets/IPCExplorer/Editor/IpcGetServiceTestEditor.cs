using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IpcGetServiceTest))]
public class IpcGetServiceTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IpcGetServiceTest script = (IpcGetServiceTest)target;
        if (GUILayout.Button("Run Ed's Wallet"))
        {
            script.RunWalletEd();
        }

        if (GUILayout.Button("Run Sherman's Wallet"))
        {
            script.RunWalletSherman();
        }

        if (GUILayout.Button("Run Omer's Wallet"))
        {
            script.RunWalletOmer();
        }

        if (GUILayout.Button("Run Armaan's Wallet"))
        {
            script.RunWalletArmaan();
        }
    }
}
