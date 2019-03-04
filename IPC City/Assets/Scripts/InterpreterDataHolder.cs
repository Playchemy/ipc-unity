using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interpreter Data", menuName = "SpriteDataAssigner", order = 1)]
public class InterpreterDataHolder : ScriptableObject
{

    public enum Race
    {
        uninitialized,
        Elf,
        Human,
        Dwarf,
        Orc
    }
    public int[] PercentOccurenceByRace = new int[] { 0, 23, 31, 23, 23 };

    public enum elfSubrace
    {
        uninitialized,
        Night,   // nocturnal
        Wood,    // forest-dwelling
        High,    // city-dwelling
        Sun,     // island/desert-dwelling
        Dark     // cave-dwelling
    }
    public int[] PercentOccurenceByElfSubrace;

    public string[] nightElfSkinColor;
    public string[] woodElfSkinColor;
    public string[] highElfSkinColor;
    public string[] sunElfSkinColor;
    public string[] darkElfSkinColor;

    public string[] nightElfHairColor;
    public string[] woodElfHairColor;
    public string[] highElfHairColor;
    public string[] sunElfHairColor;
    public string[] darkElfHairColor;

    public string[] nightElfEyeColor;
    public string[] woodElfEyeColor;
    public string[] highElfEyeColor;
    public string[] sunElfEyeColor;
    public string[] darkElfEyeColor;

    public enum humanSubrace
    {
        uninitialized,
        atlantean,  // ???
        Nordic,    // caucasian
        Eastern,   // east asian
        Coastal,   // mediterranian
        Southern  // african
    }
    public int[] PercentOccurenceByHumanSubrace;

    public string[] nordicHumanSkinColor;
    public string[] easternHumanSkinColor;
    public string[] coastalHumanSkinColor;
    public string[] southernHumanSkinColor;
    public string[] mythicalHumanSkinColor;


    public string[] nordicHumanHairColor;
    public string[] easternHumanHairColor;
    public string[] coastalHumanHairColor;
    public string[] southernHumanHairColor;
    public string[] mythicalHumanHairColor;

    public string[] nordicHumanEyeColor;
    public string[] easternHumanEyeColor;
    public string[] coastalHumanEyeColor;
    public string[] southernHumanEyeColor;
    public string[] mythicalHumanEyeColor;

    public enum dwarfSubrace
    {
        uninitialized,
        Quarry,    // skin like marble
        Mountain,  // skin like metals
        Lumber,    // skin like woods
        Hill,      // skin like earth
        Volcano    // skin like obsidian
    }
    public int[] PercentOccurenceByDwarfSubrace;

    public string[] quarryDwarfSkinColor;
    public string[] mountainDwarfSkinColor;
    public string[] lumberDwarfSkinColor;
    public string[] hillDwarfSkinColor;
    public string[] volcanoDwarfSkinColor;

    public string[] quarryDwarfHairColor;
    public string[] mountainDwarfHairColor;
    public string[] lumberDwarfHairColor;
    public string[] hillDwarfHairColor;
    public string[] volcanoDwarfHairColor;

    public string[] quarryDwarfEyeColor;
    public string[] mountainDwarfEyeColor;
    public string[] lumberDwarfEyeColor;
    public string[] hillDwarfEyeColor;
    public string[] volcanoDwarfEyeColor;

    public enum orcSubrace
    {
        uninitialized,
        Ash,     // skin covered in ash
        Sand,    // skin like sand
        Plains,  // green skin
        Swamp,   // dark blue-green-brown skin
        Blood    // red skin
    }
    public int[] PercentOccurenceByOrcSubrace;

    public string[] ashOrcSkinColor;
    public string[] sandOrcSkinColor;
    public string[] plainsOrcSkinColor;
    public string[] swampOrcSkinColor;
    public string[] bloodOrcSkinColor;

    public string[] ashOrcHairColor;
    public string[] sandOrcHairColor;
    public string[] plainsOrcHairColor;
    public string[] swampOrcHairColor;
    public string[] bloodOrcHairColor;

    public string[] ashOrcEyeColor;
    public string[] sandOrcEyeColor;
    public string[] plainsOrcEyeColor;
    public string[] swampOrcEyeColor;
    public string[] bloodOrcEyeColor;

    public enum Gender
    {
        uninitialized,
        Female,
        Male,
        nonBinary   // not a default option; set by owner
    }
    public int[] MalePercentageByRace;

    public enum Handedness
    {
        uninitialized,
        Left,
        Right,
        Ambidextrous    // gained after training ambidexterity skill
    }
    public int leftHandednessPercentage = 15;

    public int[] leftHandednessPercentageByRace = new int[] { 0, 50, 50, 50, 50 };

    public int[] baseHeightByRace = new int[] { 0, 50, 50, 50, 50 };

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
