using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    [SerializeField]
    protected bool loadNewIpcData;

    public DateTime birth;
    public string timeOfBirth;

    public int[] attributeBytes;
    public int[] dnaBytes;

    public InterpreterDataHolder dataHolder;

    public bool autoScribe = false;

    public enum Race
    {
        uninitialized,
        Elf,
        Human,
        Dwarf,
        Orc
    }

    public enum elfSubrace
    {
        uninitialized,
        Night,   // nocturnal
        Wood,    // forest-dwelling
        High,    // city-dwelling
        Sun,     // island/desert-dwelling
        Dark     // cave-dwelling
    }

    public enum humanSubrace
    {
        uninitialized,
        Mythical,  // ???
        Nordic,    // caucasian
        Eastern,   // east asian
        Coastal,   // mediterranian
        Southern  // african
    }

    public enum dwarfSubrace
    {
        uninitialized,
        Quarry,    // skin like marble
        Mountain,  // skin like metals
        Lumber,    // skin like woods
        Hill,      // skin like earth
        Volcano    // skin like obsidian
    }

    public enum orcSubrace
    {
        uninitialized,
        Ash,     // skin covered in ash
        Sand,    // skin like sand
        Plains,  // green skin
        Swamp,   // dark blue-green-brown skin
        Blood    // red skin
    }

    public enum Gender
    {
        uninitialized,
        Female,
        Male,
        nonBinary   // not a default option; set by owner
    }

    public enum Handedness
    {
        uninitialized,
        Left,
        Right,
        Ambidextrous    // gained after training ambidexterity skill
    }

    [ContextMenu("Interp")]
    public virtual void Interpret()
    {
        calculateAttributes();
        calculateDna();
        birth = UnixTimeStampToDateTime(GetComponent<IpcGetService>().ipcStorage.m_timeOfBirth);
        timeOfBirth = birth.ToShortDateString() + "   " + birth.ToShortTimeString();

        if(autoScribe)
        {
            GetComponent<Scribe>().Transcribe();
        }
    }

    protected virtual void calculateAttributes()
    {
        // Convert the hex string back to the number
        string attributeSeed = (GetComponent<IpcGetService>().ipcStorage.m_attributes);
        attributeBytes = new int[13];
        for (int i = 0; i < 12; ++i)
        {
            string stringToConvert = attributeSeed[i * 2] + "" + attributeSeed[(i * 2) + 1];
            attributeBytes[i] = int.Parse(stringToConvert, System.Globalization.NumberStyles.HexNumber);
        }

        // Convert the byte values to dice rolls
        for (int i = 0; i < 12; ++i)
        {
            attributeBytes[i] = (attributeBytes[i] * 6) / 256 + 1;
            // attributeBytes[i] = (attributeBytes[i] + 2) % 6 + 1;

        }

        // Calculate luck
        int luck = 0;
        for (int i = 0; i < 12; ++i)
        {
            luck += attributeBytes[i];
        }
        luck /= 4;
        attributeBytes[12] = 21 - luck;
    }

    protected virtual void calculateDna()
    {
        // Convert the hex string back to the number
        string dnaSeed = GetComponent<IpcGetService>().ipcStorage.m_dna;
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

    protected int _calculateRace(int _raceValue)
    {
        int racePercent = _getPercent(_raceValue);
        int counter = 0;
        for (int i = 1; i < dataHolder.PercentOccurenceByRace.Length; ++i)
        {
            counter += dataHolder.PercentOccurenceByRace[i];
            if (racePercent < counter)
            {
                return i;
            }
        }
        return 0;
    }

    protected int _calculateSubrace(int _race, int _subraceValue)
    {
        int subracePercent = _getPercent(_subraceValue);
        int counter = 0;
        for (int i = 1; i < 7; ++i)
        {
            if (_race == (int)Race.Elf)
            {
                counter += dataHolder.PercentOccurenceByElfSubrace[i];
            }
            else if (_race == (int)Race.Human)
            {
                counter += dataHolder.PercentOccurenceByHumanSubrace[i];
            }
            else if (_race == (int)Race.Dwarf)
            {
                counter += dataHolder.PercentOccurenceByDwarfSubrace[i];
            }
            else
            {
                counter += dataHolder.PercentOccurenceByOrcSubrace[i];
            }
            if (subracePercent < counter + 1)
            {
                return i;
            }
        }
        Debug.Log("SUBRACE CALCULATE ZEROOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO!?!?!");
        return 0;
    }

    protected int _calculateGender(int _race, int _genderValue)
    {
        int genderPercent = _getPercent(_genderValue);
        bool isMale;
        isMale = (genderPercent > (99 - dataHolder.MalePercentageByRace[_race]));
        return isMale ? (int)Gender.Male : (int)Gender.Female;
    }

    protected int _calculateHeight(int _race, int _heightValue, int _gender)
    {
        bool isMale = (_gender == (int)Gender.Male);
        int height;
        int heightPercent = _getPercent(_heightValue);
        if (_race == (int)Race.Elf)
        {
            height = dataHolder.baseHeightByRace[(int)Race.Elf]; // default
            if (isMale) { height += 2; } // elf males are 2 inches taller
            if (heightPercent < 5) { return height; }
            else if (heightPercent < 35) { return height + 1; }
            else if (heightPercent < 60) { return height + 2; }
            else if (heightPercent < 95) { return height + 3; }
            else { return height + 4; }
        }
        else if (_race == (int)Race.Human)
        {
            height = dataHolder.baseHeightByRace[(int)Race.Human]; // default
            if (isMale) { height += 4; } // human males are 4 inches taller
            if (heightPercent < 5) { return height; }
            else if (heightPercent < 10) { return height + 1; }
            else if (heightPercent < 15) { return height + 2; }
            else if (heightPercent < 25) { return height + 3; }
            else if (heightPercent < 40) { return height + 4; }
            else if (heightPercent < 55) { return height + 5; }
            else if (heightPercent < 65) { return height + 6; }
            else if (heightPercent < 75) { return height + 7; }
            else if (heightPercent < 85) { return height + 8; }
            else if (heightPercent < 90) { return height + 9; }
            else if (heightPercent < 95) { return height + 10; }
            else { return height + 11; }
        }
        else if (_race == (int)Race.Dwarf)
        {
            height = dataHolder.baseHeightByRace[(int)Race.Dwarf]; // default
            if (isMale) { height += 2; } // dwarf males are 2 inches taller
            if (heightPercent < 5) { return height; }
            else if (heightPercent < 15) { return height + 1; }
            else if (heightPercent < 40) { return height + 2; }
            else if (heightPercent < 65) { return height + 3; }
            else if (heightPercent < 85) { return height + 4; }
            else if (heightPercent < 95) { return height + 5; }
            else { return height + 6; }
        }
        else
        {
            height = dataHolder.baseHeightByRace[(int)Race.Orc]; // default
            if (isMale) { height += 4; } // orc males are 4 inches taller
            if (heightPercent < 5) { return height; }
            else if (heightPercent < 10) { return height + 1; }
            else if (heightPercent < 15) { return height + 2; }
            else if (heightPercent < 25) { return height + 3; }
            else if (heightPercent < 40) { return height + 4; }
            else if (heightPercent < 55) { return height + 5; }
            else if (heightPercent < 65) { return height + 6; }
            else if (heightPercent < 75) { return height + 7; }
            else if (heightPercent < 85) { return height + 8; }
            else if (heightPercent < 90) { return height + 9; }
            else if (heightPercent < 95) { return height + 10; }
            else { return height + 11; }
        }
    }

    protected int _calculateHandedness(int _race, int _handednessValue)
    {
        bool isLeftHanded;
        int handednessPercent = _getPercent(_handednessValue);
        isLeftHanded = (handednessPercent < dataHolder.leftHandednessPercentageByRace[(int)_race]);
        return (isLeftHanded ? (int)Handedness.Left : (int)Handedness.Right);
    }

    protected void _calculateColors()
    {
        switch (dnaBytes[0])
        {
            case 1:
                switch (dnaBytes[1])
                {
                    case 1:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.nightElfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.nightElfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.nightElfEyeColor.Length / 256;
                        break;
                    case 2:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.woodElfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.woodElfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.woodElfEyeColor.Length / 256;
                        break;
                    case 3:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.highElfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.highElfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.highElfEyeColor.Length / 256;
                        break;
                    case 4:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.sunElfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.sunElfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.sunElfEyeColor.Length / 256;
                        break;
                    case 5:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.darkElfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.darkElfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.darkElfEyeColor.Length / 256;
                        break;
                }
                break;
            case 2:
                switch (dnaBytes[1])
                {
                    case 1:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.mythicalHumanSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.mythicalHumanHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.mythicalHumanEyeColor.Length / 256;
                        break;
                    case 2:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.nordicHumanSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.nordicHumanHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.nordicHumanSkinColor.Length / 256;
                        break;
                    case 3:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.easternHumanSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.easternHumanHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.easternHumanEyeColor.Length / 256;
                        break;
                    case 4:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.coastalHumanSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.coastalHumanHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.coastalHumanEyeColor.Length / 256;
                        break;
                    case 5:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.southernHumanSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.southernHumanHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.southernHumanEyeColor.Length / 256;
                        break;
                }
                break;
            case 3:
                switch (dnaBytes[1])
                {
                    case 1:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.quarryDwarfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.quarryDwarfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.quarryDwarfEyeColor.Length / 256;
                        break;
                    case 2:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.mountainDwarfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.mountainDwarfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.mountainDwarfEyeColor.Length / 256;
                        break;
                    case 3:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.lumberDwarfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.lumberDwarfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.lumberDwarfEyeColor.Length / 256;
                        break;
                    case 4:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.hillDwarfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.hillDwarfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.hillDwarfEyeColor.Length / 256;
                        break;
                    case 5:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.volcanoDwarfSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.volcanoDwarfHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.volcanoDwarfEyeColor.Length / 256;
                        break;
                }
                break;
            case 4:
                switch (dnaBytes[1])
                {
                    case 1:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.ashOrcSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.ashOrcHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.ashOrcEyeColor.Length / 256;
                        break;
                    case 2:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.sandOrcSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.sandOrcHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.sandOrcEyeColor.Length / 256;
                        break;
                    case 3:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.plainsOrcSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.plainsOrcHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.plainsOrcEyeColor.Length / 256;
                        break;
                    case 4:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.swampOrcSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.swampOrcHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.swampOrcEyeColor.Length / 256;
                        break;
                    case 5:
                        dnaBytes[5] = dnaBytes[5] * dataHolder.bloodOrcSkinColor.Length / 256;
                        dnaBytes[6] = dnaBytes[6] * dataHolder.bloodOrcHairColor.Length / 256;
                        dnaBytes[7] = dnaBytes[7] * dataHolder.bloodOrcEyeColor.Length / 256;
                        break;
                }
                break;
        }
    }

    protected int _getPercent(int byteValue)
    {
        return byteValue * 100 / 255;
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
}
