using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpretFromCache : Interpreter
{
    public IPC_Data ipcData;

    [ContextMenu("Interp")]
    public override void Interpret()
    {
        if (loadNewIpcData)
        {
            ipcData = DataInitializer.Instance.GetIpcData(GetComponent<IpcGetService>().inputIPCID);
        }

        calculateAttributes();
        calculateDna();
        birth = UnixTimeStampToDateTime(ipcData.ipc_timeOfBirth);
        timeOfBirth = birth.ToShortDateString() + "   " + birth.ToShortTimeString();

        if (autoScribe)
        {
            GetComponent<Scribe>().Transcribe();
        }
    }

    protected override void calculateAttributes()
    {
        string attributeSeed = ipcData.ipc_attributes;
        attributeBytes = new int[13];
        for (int i = 0; i < 12; ++i)
        {
            string stringToConvert = attributeSeed[i * 2] + "" + attributeSeed[(i * 2) + 1];
            attributeBytes[i] = int.Parse(stringToConvert, System.Globalization.NumberStyles.HexNumber);
        }

        for (int i = 0; i < 12; ++i)
        {
            attributeBytes[i] = (attributeBytes[i] * 6) / 256 + 1;

        }

        int luck = 0;
        for (int i = 0; i < 12; ++i)
        {
            luck += attributeBytes[i];
        }
        luck /= 4;
        attributeBytes[12] = 21 - luck;
    }

    protected override void calculateDna()
    {
        string dnaSeed = ipcData.ipc_dna;
        dnaBytes = new int[8];
        for (int i = 0; i < 8; ++i)
        {
            string stringToConvert = dnaSeed[i * 2] + "" + dnaSeed[(i * 2) + 1];
            dnaBytes[i] = int.Parse(stringToConvert, System.Globalization.NumberStyles.HexNumber);
        }
        dnaBytes[0] = _calculateRace(dnaBytes[0]);
        dnaBytes[1] = _calculateSubrace(dnaBytes[0], dnaBytes[1]);
        dnaBytes[2] = _calculateGender(dnaBytes[0], dnaBytes[2]);
        dnaBytes[3] = _calculateHeight(dnaBytes[0], dnaBytes[3], dnaBytes[2]);
        dnaBytes[4] = _calculateHandedness(dnaBytes[0], dnaBytes[4]);
        _calculateColors();
    }

}
