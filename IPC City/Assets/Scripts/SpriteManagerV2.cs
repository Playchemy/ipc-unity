using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManagerV2 : MonoBehaviour
{

    public Image bodyRenderer;
    public Image hairRenderer;
    public Image clothesRenderer;
    public Image accessoryRenderer;

    [Space(8f)]
    public SpriteRenderer bodyRenderer_sprite;
    public SpriteRenderer hairRenderer_sprite;
    public SpriteRenderer clothesRenderer_sprite;
    public SpriteRenderer accessoryRenderer_sprite;

    [Space(8f)]
    public Ipc_SpriteData assignedBodyData;
    public Ipc_SpriteData assignedHairData;
    public Ipc_SpriteData assignedClothesData;
    public Ipc_SpriteData assignedAccessoryData;

    [Space(8f)]
    public MasterSpriteDataHolder masterSpriteData;

    [Space(8f)]
    public ColorData colorData;

    private Scribe scribe;

    public AdminAccounts adminAccountData;

    public bool useSpriteRenderer;

    public ParticleSystem effect;

    public bool addToAnimator = false;

    public SpriteHandler_UI uiSpriteHandler;

    void Start ()
    {
        scribe = GetComponent<Scribe>();
	}

    public void AssignSprites()
    {
        if (!useSpriteRenderer)
        {
            clothesRenderer.enabled = true;
            hairRenderer.enabled = true;
            accessoryRenderer.enabled = false;
        }
        else
        {
            clothesRenderer_sprite.enabled = true;
            hairRenderer_sprite.enabled = true;
            accessoryRenderer_sprite.enabled = false;
        }

        PickBody();
        PickHairColor();
        PickSkinColor();
        PickAccessory();

        Pose();




        /*
        foreach (string id in adminAccountData.adminAccounts)
        {
            if(scribe.ipcId.text == id)
            {
                accessoryRenderer.enabled = true;
                id.IndexOf();
            }
        }
        */

        if(useSpriteRenderer)
        {
            SpriteHandler handler = GetComponent<SpriteHandler>();
            handler.clothesData = assignedClothesData;
            handler.hairData = assignedHairData;
            handler.skinData = assignedBodyData;
            handler.accessoryData = assignedAccessoryData;
        }

        if(GetComponent<IPC_Stats>())
        {
            GetComponent<IPC_Stats>().AssignValues();
        }


        //IF STATIC BOOL ANIMATION IS ON

        if (addToAnimator)
        {
            //SpriteHandler_UI handler = transform.GetComponentInChildren<SpriteHandler_UI>();

            uiSpriteHandler.skinData = assignedBodyData;
            uiSpriteHandler.clothesData = assignedClothesData;
            uiSpriteHandler.hairData = assignedHairData;
            uiSpriteHandler.accessoryData = assignedAccessoryData;

            uiSpriteHandler.transform.GetComponent<Animator>().enabled = true;
            uiSpriteHandler.transform.GetComponent<Animator>().Play("WalkLeft");
        }
        else
        {
            if (!uiSpriteHandler)
                return;

            uiSpriteHandler.skinData = null;
            uiSpriteHandler.clothesData = null;
            uiSpriteHandler.hairData = null;
            uiSpriteHandler.accessoryData = null;

            uiSpriteHandler.transform.GetComponent<Animator>().enabled = false;
        }
        
    }

    void PickAccessory()
    {
        for (int i = 0; i < adminAccountData.adminAccounts.Count; i++)
        {
            if (scribe.ipcId.text == adminAccountData.adminAccounts[i].adminAccountID)
            {
                if (useSpriteRenderer)
                {
                    accessoryRenderer_sprite.enabled = true;
                }
                else
                {
                    accessoryRenderer.enabled = true;
                }

                assignedAccessoryData = adminAccountData.adminAccounts[i].assignedAccessory;
                break;
            }
        }
    }

    void PickBody()
    {
        if(scribe.gender.text == "Male")
        {
            assignedHairData = masterSpriteData.maleHair;

            assignedBodyData = masterSpriteData.maleHuman;

            if (scribe.race.text == "Orc")
            {
                assignedBodyData = masterSpriteData.maleOrc;

                if (!useSpriteRenderer)
                {
                    clothesRenderer.enabled = false;
                    hairRenderer.enabled = false;
                }
                else
                {
                    clothesRenderer_sprite.enabled = false;
                    hairRenderer_sprite.enabled = false;
                }

            }
            else if(scribe.race.text == "Human")
            {
                assignedClothesData = masterSpriteData.maleHumanClothes;
            }
            else if (scribe.race.text == "Dwarf")
            {
                assignedClothesData = masterSpriteData.maleDwarfClothes;
            }
            else if (scribe.race.text == "Elf")
            {
                assignedClothesData = masterSpriteData.maleElfClothes;
            }

        }
        else
        {
            assignedHairData = masterSpriteData.femaleHair;

            assignedBodyData = masterSpriteData.femaleHuman;

            if (scribe.race.text == "Orc")
            {
                assignedBodyData = masterSpriteData.femaleOrc;
                assignedClothesData = masterSpriteData.femaleOrcClothes;

                if (!useSpriteRenderer)
                {
                    clothesRenderer.enabled = true;
                    hairRenderer.enabled = false;
                }
                else
                {
                    clothesRenderer_sprite.enabled = true;
                    hairRenderer_sprite.enabled = false;
                }
            }
            else if (scribe.race.text == "Human")
            {
                assignedClothesData = masterSpriteData.femaleHumanClothes;
            }
            else if (scribe.race.text == "Dwarf")
            {
                assignedClothesData = masterSpriteData.femaleDwarfClothes;
            }
            else if (scribe.race.text == "Elf")
            {
                assignedClothesData = masterSpriteData.femaleElfClothes;
            }


        }
    }

    void PickClothes()
    {

    }

    void PickSkinColor()
    {
        if (!useSpriteRenderer)
        {
            if (scribe.skinColor.text == "White") bodyRenderer.color = colorData.white;
            if (scribe.skinColor.text == "Blue-Grey") bodyRenderer.color = colorData.blueGrey;
            if (scribe.skinColor.text == "Midnight Blue") bodyRenderer.color = colorData.midnightBlue;
            if (scribe.skinColor.text == "Blue") bodyRenderer.color = colorData.blue;
            if (scribe.skinColor.text == "Dark Blue") bodyRenderer.color = colorData.darkBlue;
            if (scribe.skinColor.text == "Blue-Black") bodyRenderer.color = colorData.blueBlack;
            if (scribe.skinColor.text == "Icy") bodyRenderer.color = colorData.icy;
            if (scribe.skinColor.text == "Pale") bodyRenderer.color = colorData.pale;
            if (scribe.skinColor.text == "Beige") bodyRenderer.color = colorData.beige;
            if (scribe.skinColor.text == "Golden") bodyRenderer.color = colorData.golden;
            if (scribe.skinColor.text == "Tan") bodyRenderer.color = colorData.tan;
            if (scribe.skinColor.text == "Light Brown") bodyRenderer.color = colorData.lightBrown;
            if (scribe.skinColor.text == "Brown") bodyRenderer.color = colorData.brown;
            if (scribe.skinColor.text == "Dark Brown") bodyRenderer.color = colorData.darkBrown;
            if (scribe.skinColor.text == "Obsidian") bodyRenderer.color = colorData.obsidian;
            if (scribe.skinColor.text == "Red") bodyRenderer.color = colorData.red;
            if (scribe.skinColor.text == "Grey") bodyRenderer.color = colorData.grey;
            if (scribe.skinColor.text == "Black") bodyRenderer.color = colorData.black;
            if (scribe.skinColor.text == "Ice") bodyRenderer.color = colorData.ice;
            if (scribe.skinColor.text == "Green") bodyRenderer.color = colorData.green;
            if (scribe.skinColor.text == "Forest Green") bodyRenderer.color = colorData.forestGreen;
            if (scribe.skinColor.text == "Dark Blue-Green") bodyRenderer.color = colorData.darkBlueGreen;
            if (scribe.skinColor.text == "Blue-Green") bodyRenderer.color = colorData.blueGreen;
            if (scribe.skinColor.text == "Pale Green") bodyRenderer.color = colorData.paleGreen;
            if (scribe.skinColor.text == "Purple") bodyRenderer.color = colorData.purple;
            if (scribe.skinColor.text == "Orange") bodyRenderer.color = colorData.orange;
            if (scribe.skinColor.text == "Gold") bodyRenderer.color = colorData.gold;
            if (scribe.skinColor.text == "Amber") bodyRenderer.color = colorData.amber;
            if (scribe.skinColor.text == "Dark Grey") bodyRenderer.color = colorData.darkGrey;
            if (scribe.skinColor.text == "Light Yellow") bodyRenderer.color = colorData.lightYellow;
            if (scribe.skinColor.text == "Yellow") bodyRenderer.color = colorData.yellow;
            if (scribe.skinColor.text == "DarkYellow") bodyRenderer.color = colorData.darkYellow;
            if (scribe.skinColor.text == "Platinum") bodyRenderer.color = colorData.platinum;
            if (scribe.skinColor.text == "Blonde") bodyRenderer.color = colorData.blonde;

            if (scribe.skinColor.text == "Auburn") bodyRenderer.color = colorData.auburn;
            if (scribe.skinColor.text == "Dark Red") bodyRenderer.color = colorData.darkRed;
            if (scribe.skinColor.text == "Marbled White") bodyRenderer.color = colorData.marbledWhite;
            if (scribe.skinColor.text == "Marbled Black") bodyRenderer.color = colorData.marbledBlack;


        }
        else
        {
            if (scribe.skinColor.text == "White") bodyRenderer_sprite.color = colorData.white;
            if (scribe.skinColor.text == "Blue-Grey") bodyRenderer_sprite.color = colorData.blueGrey;
            if (scribe.skinColor.text == "Midnight Blue") bodyRenderer_sprite.color = colorData.midnightBlue;
            if (scribe.skinColor.text == "Blue") bodyRenderer_sprite.color = colorData.blue;
            if (scribe.skinColor.text == "Dark Blue") bodyRenderer_sprite.color = colorData.darkBlue;
            if (scribe.skinColor.text == "Blue-Black") bodyRenderer_sprite.color = colorData.blueBlack;
            if (scribe.skinColor.text == "Icy") bodyRenderer_sprite.color = colorData.icy;
            if (scribe.skinColor.text == "Pale") bodyRenderer_sprite.color = colorData.pale;
            if (scribe.skinColor.text == "Beige") bodyRenderer_sprite.color = colorData.beige;
            if (scribe.skinColor.text == "Golden") bodyRenderer_sprite.color = colorData.golden;
            if (scribe.skinColor.text == "Tan") bodyRenderer_sprite.color = colorData.tan;
            if (scribe.skinColor.text == "Light Brown") bodyRenderer_sprite.color = colorData.lightBrown;
            if (scribe.skinColor.text == "Brown") bodyRenderer_sprite.color = colorData.brown;
            if (scribe.skinColor.text == "Dark Brown") bodyRenderer_sprite.color = colorData.darkBrown;
            if (scribe.skinColor.text == "Obsidian") bodyRenderer_sprite.color = colorData.obsidian;
            if (scribe.skinColor.text == "Red") bodyRenderer_sprite.color = colorData.red;
            if (scribe.skinColor.text == "Grey") bodyRenderer_sprite.color = colorData.grey;
            if (scribe.skinColor.text == "Black") bodyRenderer_sprite.color = colorData.black;
            if (scribe.skinColor.text == "Ice") bodyRenderer_sprite.color = colorData.ice;
            if (scribe.skinColor.text == "Green") bodyRenderer_sprite.color = colorData.green;
            if (scribe.skinColor.text == "Forest Green") bodyRenderer_sprite.color = colorData.forestGreen;
            if (scribe.skinColor.text == "Dark Blue-Green") bodyRenderer_sprite.color = colorData.darkBlueGreen;
            if (scribe.skinColor.text == "Blue-Green") bodyRenderer_sprite.color = colorData.blueGreen;
            if (scribe.skinColor.text == "Pale Green") bodyRenderer_sprite.color = colorData.paleGreen;
            if (scribe.skinColor.text == "Purple") bodyRenderer_sprite.color = colorData.purple;
            if (scribe.skinColor.text == "Orange") bodyRenderer_sprite.color = colorData.orange;
            if (scribe.skinColor.text == "Gold") bodyRenderer_sprite.color = colorData.gold;
            if (scribe.skinColor.text == "Amber") bodyRenderer_sprite.color = colorData.amber;
            if (scribe.skinColor.text == "Dark Grey") bodyRenderer_sprite.color = colorData.darkGrey;
            if (scribe.skinColor.text == "Light Yellow") bodyRenderer_sprite.color = colorData.lightYellow;
            if (scribe.skinColor.text == "Yellow") bodyRenderer_sprite.color = colorData.yellow;
            if (scribe.skinColor.text == "DarkYellow") bodyRenderer_sprite.color = colorData.darkYellow;
            if (scribe.skinColor.text == "Platinum") bodyRenderer_sprite.color = colorData.platinum;
            if (scribe.skinColor.text == "Blonde") bodyRenderer_sprite.color = colorData.blonde;

            if (scribe.skinColor.text == "Auburn") bodyRenderer_sprite.color = colorData.auburn;
            if (scribe.skinColor.text == "Dark Red") bodyRenderer_sprite.color = colorData.darkRed;
            if (scribe.skinColor.text == "Marbled White") bodyRenderer_sprite.color = colorData.marbledWhite;
            if (scribe.skinColor.text == "Marbled Black") bodyRenderer_sprite.color = colorData.marbledBlack;
        }
    }

    void PickHairColor()
    {
        if (!useSpriteRenderer)
        {
            if (scribe.hairColor.text == "White") hairRenderer.color = colorData.white;
            if (scribe.hairColor.text == "Blue-Grey") hairRenderer.color = colorData.blueGrey;
            if (scribe.hairColor.text == "Midnight Blue") hairRenderer.color = colorData.midnightBlue;
            if (scribe.hairColor.text == "Blue") hairRenderer.color = colorData.blue;
            if (scribe.hairColor.text == "Dark Blue") hairRenderer.color = colorData.darkBlue;
            if (scribe.hairColor.text == "Blue-Black") hairRenderer.color = colorData.blueBlack;
            if (scribe.hairColor.text == "Icy") hairRenderer.color = colorData.icy;
            if (scribe.hairColor.text == "Pale") hairRenderer.color = colorData.pale;
            if (scribe.hairColor.text == "Beige") hairRenderer.color = colorData.beige;
            if (scribe.hairColor.text == "Golden") hairRenderer.color = colorData.golden;
            if (scribe.hairColor.text == "Tan") hairRenderer.color = colorData.tan;
            if (scribe.hairColor.text == "Light Brown") hairRenderer.color = colorData.lightBrown;
            if (scribe.hairColor.text == "Brown") hairRenderer.color = colorData.brown;
            if (scribe.hairColor.text == "Dark Brown") hairRenderer.color = colorData.darkBrown;
            if (scribe.hairColor.text == "Obsidian") hairRenderer.color = colorData.obsidian;
            if (scribe.hairColor.text == "Red") hairRenderer.color = colorData.red;
            if (scribe.hairColor.text == "Grey") hairRenderer.color = colorData.grey;
            if (scribe.hairColor.text == "Black") hairRenderer.color = colorData.black;
            if (scribe.hairColor.text == "Ice") hairRenderer.color = colorData.ice;
            if (scribe.hairColor.text == "Green") hairRenderer.color = colorData.green;
            if (scribe.hairColor.text == "Forest Green") hairRenderer.color = colorData.forestGreen;
            if (scribe.hairColor.text == "Dark Blue-Green") hairRenderer.color = colorData.darkBlueGreen;
            if (scribe.hairColor.text == "Blue-Green") hairRenderer.color = colorData.blueGreen;
            if (scribe.hairColor.text == "Pale Green") hairRenderer.color = colorData.paleGreen;
            if (scribe.hairColor.text == "Purple") hairRenderer.color = colorData.purple;
            if (scribe.hairColor.text == "Orange") hairRenderer.color = colorData.orange;
            if (scribe.hairColor.text == "Gold") hairRenderer.color = colorData.gold;
            if (scribe.hairColor.text == "Amber") hairRenderer.color = colorData.amber;
            if (scribe.hairColor.text == "Dark Grey") hairRenderer.color = colorData.darkGrey;
            if (scribe.hairColor.text == "Light Yellow") hairRenderer.color = colorData.lightYellow;
            if (scribe.hairColor.text == "Yellow") hairRenderer.color = colorData.yellow;
            if (scribe.hairColor.text == "DarkYellow") hairRenderer.color = colorData.darkYellow;
            if (scribe.hairColor.text == "Platinum") hairRenderer.color = colorData.platinum;
            if (scribe.hairColor.text == "Blonde") hairRenderer.color = colorData.blonde;

            if (scribe.hairColor.text == "Auburn") hairRenderer.color = colorData.auburn;
            if (scribe.hairColor.text == "Dark Red") hairRenderer.color = colorData.darkRed;
            if (scribe.hairColor.text == "Marbled White") hairRenderer.color = colorData.marbledWhite;
            if (scribe.hairColor.text == "Marbled Black") hairRenderer.color = colorData.marbledBlack;
        }
        else
        {
            if (scribe.hairColor.text == "White") hairRenderer_sprite.color = colorData.white;
            if (scribe.hairColor.text == "Blue-Grey") hairRenderer_sprite.color = colorData.blueGrey;
            if (scribe.hairColor.text == "Midnight Blue") hairRenderer_sprite.color = colorData.midnightBlue;
            if (scribe.hairColor.text == "Blue") hairRenderer_sprite.color = colorData.blue;
            if (scribe.hairColor.text == "Dark Blue") hairRenderer_sprite.color = colorData.darkBlue;
            if (scribe.hairColor.text == "Blue-Black") hairRenderer_sprite.color = colorData.blueBlack;
            if (scribe.hairColor.text == "Icy") hairRenderer_sprite.color = colorData.icy;
            if (scribe.hairColor.text == "Pale") hairRenderer_sprite.color = colorData.pale;
            if (scribe.hairColor.text == "Beige") hairRenderer_sprite.color = colorData.beige;
            if (scribe.hairColor.text == "Golden") hairRenderer_sprite.color = colorData.golden;
            if (scribe.hairColor.text == "Tan") hairRenderer_sprite.color = colorData.tan;
            if (scribe.hairColor.text == "Light Brown") hairRenderer_sprite.color = colorData.lightBrown;
            if (scribe.hairColor.text == "Brown") hairRenderer_sprite.color = colorData.brown;
            if (scribe.hairColor.text == "Dark Brown") hairRenderer_sprite.color = colorData.darkBrown;
            if (scribe.hairColor.text == "Obsidian") hairRenderer_sprite.color = colorData.obsidian;
            if (scribe.hairColor.text == "Red") hairRenderer_sprite.color = colorData.red;
            if (scribe.hairColor.text == "Grey") hairRenderer_sprite.color = colorData.grey;
            if (scribe.hairColor.text == "Black") hairRenderer_sprite.color = colorData.black;
            if (scribe.hairColor.text == "Ice") hairRenderer_sprite.color = colorData.ice;
            if (scribe.hairColor.text == "Green") hairRenderer_sprite.color = colorData.green;
            if (scribe.hairColor.text == "Forest Green") hairRenderer_sprite.color = colorData.forestGreen;
            if (scribe.hairColor.text == "Dark Blue-Green") hairRenderer_sprite.color = colorData.darkBlueGreen;
            if (scribe.hairColor.text == "Blue-Green") hairRenderer_sprite.color = colorData.blueGreen;
            if (scribe.hairColor.text == "Pale Green") hairRenderer_sprite.color = colorData.paleGreen;
            if (scribe.hairColor.text == "Purple") hairRenderer_sprite.color = colorData.purple;
            if (scribe.hairColor.text == "Orange") hairRenderer_sprite.color = colorData.orange;
            if (scribe.hairColor.text == "Gold") hairRenderer_sprite.color = colorData.gold;
            if (scribe.hairColor.text == "Amber") hairRenderer_sprite.color = colorData.amber;
            if (scribe.hairColor.text == "Dark Grey") hairRenderer_sprite.color = colorData.darkGrey;
            if (scribe.hairColor.text == "Light Yellow") hairRenderer_sprite.color = colorData.lightYellow;
            if (scribe.hairColor.text == "Yellow") hairRenderer_sprite.color = colorData.yellow;
            if (scribe.hairColor.text == "DarkYellow") hairRenderer_sprite.color = colorData.darkYellow;
            if (scribe.hairColor.text == "Platinum") hairRenderer_sprite.color = colorData.platinum;
            if (scribe.hairColor.text == "Blonde") hairRenderer_sprite.color = colorData.blonde;

            if (scribe.hairColor.text == "Auburn") hairRenderer_sprite.color = colorData.auburn;
            if (scribe.hairColor.text == "Dark Red") hairRenderer_sprite.color = colorData.darkRed;
            if (scribe.hairColor.text == "Marbled White") hairRenderer_sprite.color = colorData.marbledWhite;
            if (scribe.hairColor.text == "Marbled Black") hairRenderer_sprite.color = colorData.marbledBlack;
        }
    }

    void Pose()
    {
        if (!useSpriteRenderer)
        {
            if (assignedBodyData)bodyRenderer.sprite = assignedBodyData.walkingLeft[2];
            if (assignedClothesData) clothesRenderer.sprite = assignedClothesData.walkingLeft[2];
            if (assignedHairData) hairRenderer.sprite = assignedHairData.walkingLeft[2];
            if (assignedAccessoryData) accessoryRenderer.sprite = assignedAccessoryData.walkingLeft[2];
        }
        else
        {
            if (assignedBodyData) bodyRenderer_sprite.sprite = assignedBodyData.walkingLeft[2];
            if (assignedClothesData) clothesRenderer_sprite.sprite = assignedClothesData.walkingLeft[2];
            if (assignedHairData) hairRenderer_sprite.sprite = assignedHairData.walkingLeft[2];
            if (assignedAccessoryData) accessoryRenderer_sprite.sprite = assignedAccessoryData.walkingLeft[2];
        }

        if (effect)
        effect.Emit(1);
        Destroy(effect, 2f);
    }
}
