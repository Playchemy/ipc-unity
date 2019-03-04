using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnaInterpreter : MonoBehaviour
{

    public enum Race
    {
        uninitialized,
        Elf,
        Human,
        Dwarf,
        Orc
    }
    public int[] PercentOccurenceByRace;

    public enum Gender
    {
        uninitialized,
        Female,
        Male,
        nonBinary   // not a default option; set by owner
    }
    public int minimumMaleValue = 128;

    public void InterpretDna(string dnaSeed)
    {
        var dnaBytes = new int[8];
        for (int i = 0; i < 8; ++i)
        {
            string stringToConvert = dnaSeed[i * 2] + "" + dnaSeed[(i * 2) + 1];
            dnaBytes[i] = int.Parse(stringToConvert, System.Globalization.NumberStyles.HexNumber);
        }
        GetComponent<IPC_Stats>().race = _calculateRace(dnaBytes[0]).ToString();
        GetComponent<IPC_Stats>().gender = _calculateGender(dnaBytes[2]).ToString();
        //GetComponent<IPC_Stats>().hairColor = _CalculateHairColor(dnaBytes[7]).ToString();

        if(GetComponent<IPC_Stats>().race == "uninitialized")
        {
            //GetComponent<IpcGetService>().StartAgain();
        }
    }

    Race _calculateRace(int _raceValue)
    {
        int racePercent = _getPercent(_raceValue);
        int counter = 0;
        for (int i = 1; i < PercentOccurenceByRace.Length; ++i)
        {
            counter += PercentOccurenceByRace[i];
            if (racePercent < counter)
            {
                return (Race)i;
            }
        }
        return 0;
    }


    Gender _calculateGender(int _genderValue)
    {
        int genderPercent = _getPercent(_genderValue);
        bool isMale = (_genderValue >= minimumMaleValue);
        return isMale ? Gender.Male : Gender.Female;
    }


    int _getPercent(int byteValue)
    {
        return byteValue * 100 / 256;
    }
}
