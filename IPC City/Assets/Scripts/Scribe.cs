using UnityEngine;
using UnityEngine.UI;

public class Scribe : MonoBehaviour
{
    [SerializeField]
    protected bool autoTranscribe;
    private IpcGetService ipc;
    private Interpreter interpreter;
    public Text ipcId;
    public Text ipcName;
    public Text timeOfBirth;
    public Text xp;
    public Text price;
    public Image QR;
    public Text strength;
    public Text dexterity;
    public Text intelligence;
    public Text constitution;
    public Text luck;
    public Text explosive;
    public Text sustained;
    public Text tolerance;
    public Text speed;
    public Text precision;
    public Text reaction;
    public Text memory;
    public Text calculus;
    public Text simulation;
    public Text healing;
    public Text fortitude;
    public Text growth;
    public Text race;
    public Text subrace;
    public Text gender;
    public Text height;
    public Text skinColor;
    public Text hairColor;
    public Text eyeColor;
    public Text handedness;
    public Text publicAddress;

    private void Start()
    {
        if (! autoTranscribe)
            return;
         Transcribe();
    }

    [ContextMenu("Scribe")]
    public virtual void Transcribe()
    {
        ipc = GetComponent<IpcGetService>();
        interpreter = GetComponent<Interpreter>();
        if (ipcId) ipcId.text = "IPC#" + ipc.inputIPCID;

        if (ipcName) ipcName.text = ipc.ipcStorage.m_name;

        if (timeOfBirth) timeOfBirth.text = interpreter.timeOfBirth;
        if (xp) xp.text = ipc.ipcStorage.m_xp.ToString();
        CalculatePrice();
        // QR.sprite = Sprite.Create( generateQR(string.Empty + (object) ipc.ipcId), new Rect(0.0f, 0.0f, 256f, 256f), new Vector2());
        if (explosive) explosive.text          = interpreter.attributeBytes[0].ToString();
        if (sustained) sustained.text          = interpreter.attributeBytes[1].ToString();
        if (tolerance) tolerance.text          = interpreter.attributeBytes[2].ToString(); ;
        if (strength) strength.text            = (interpreter.attributeBytes[0] + interpreter.attributeBytes[1] + interpreter.attributeBytes[2]).ToString(); ;
        if (speed) speed.text                  = interpreter.attributeBytes[3].ToString(); ;
        if (precision) precision.text          = interpreter.attributeBytes[4].ToString(); ;
        if (reaction) reaction.text            = interpreter.attributeBytes[5].ToString(); ;
        if (dexterity) dexterity.text          = (interpreter.attributeBytes[3] + interpreter.attributeBytes[4] + interpreter.attributeBytes[5]).ToString(); ;
        if (memory) memory.text                = interpreter.attributeBytes[6].ToString(); ;
        if (calculus) calculus.text            = interpreter.attributeBytes[7].ToString(); ;
        if (simulation) simulation.text        = interpreter.attributeBytes[8].ToString(); ;
        if (intelligence) intelligence.text    = (interpreter.attributeBytes[6] + interpreter.attributeBytes[7] + interpreter.attributeBytes[8]).ToString(); ;
        if (healing) healing.text              = interpreter.attributeBytes[9].ToString(); ;
        if (fortitude) fortitude.text          = interpreter.attributeBytes[10].ToString();
        if (growth) growth.text                = interpreter.attributeBytes[11].ToString(); ;
        if (constitution) constitution.text    = (interpreter.attributeBytes[9] + interpreter.attributeBytes[10] + interpreter.attributeBytes[11]).ToString(); ;
        if (luck) luck.text                    = interpreter.attributeBytes[12].ToString(); ;
        if (race) race.text                    = ((Interpreter.Race)interpreter.dnaBytes[0]).ToString();
        if (gender) gender.text                = ((Interpreter.Gender)interpreter.dnaBytes[2]).ToString();
        if (height) height.text                = (interpreter.dnaBytes[3] / 12) + "'" + (interpreter.dnaBytes[3] % 12) + "\"";
        if (handedness) handedness.text        = ((Interpreter.Handedness)interpreter.dnaBytes[4]).ToString();
        if (publicAddress) publicAddress.text  = ipc.ipcStorage.m_owner;

        if (ipcName && ipc.ipcStorage.m_name == "")
        {
            ipcName.text = gender.text + race.text + ipc.inputIPCID;
        }

            if (this.subrace && skinColor && hairColor && eyeColor)
        {
            switch (interpreter.dnaBytes[0])
            {
                case 1:
                    this.subrace.text = ((Interpreter.elfSubrace)interpreter.dnaBytes[1]).ToString();
                    switch (interpreter.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interpreter.dataHolder.nightElfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.nightElfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.nightElfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interpreter.dataHolder.woodElfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.woodElfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.woodElfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interpreter.dataHolder.highElfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.highElfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.highElfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interpreter.dataHolder.sunElfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.sunElfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.sunElfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interpreter.dataHolder.darkElfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.darkElfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.darkElfEyeColor[interpreter.dnaBytes[7]];
                            break;
                    }
                    break;

                case 2:
                    this.subrace.text = ((Interpreter.humanSubrace)interpreter.dnaBytes[1]).ToString();
                    switch (interpreter.dnaBytes[1])
                    {

                        case 1:
                            skinColor.text = interpreter.dataHolder.mythicalHumanSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.mythicalHumanHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.mythicalHumanEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interpreter.dataHolder.nordicHumanSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.nordicHumanHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.nordicHumanEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interpreter.dataHolder.easternHumanSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.easternHumanHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.easternHumanEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interpreter.dataHolder.coastalHumanSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.coastalHumanHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.coastalHumanEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interpreter.dataHolder.southernHumanSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.southernHumanHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.southernHumanEyeColor[interpreter.dnaBytes[7]];
                            break;                        
                    }
                    break;

                case 3:
                    this.subrace.text = ((Interpreter.dwarfSubrace)interpreter.dnaBytes[1]).ToString();
                    switch (interpreter.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interpreter.dataHolder.quarryDwarfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.quarryDwarfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.quarryDwarfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interpreter.dataHolder.mountainDwarfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.mountainDwarfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.mountainDwarfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interpreter.dataHolder.lumberDwarfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.lumberDwarfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.lumberDwarfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interpreter.dataHolder.hillDwarfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.hillDwarfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.hillDwarfEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interpreter.dataHolder.volcanoDwarfSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.volcanoDwarfHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.volcanoDwarfEyeColor[interpreter.dnaBytes[7]];
                            break;
                    }
                    break;

                case 4:
                    this.subrace.text = ((Interpreter.orcSubrace)interpreter.dnaBytes[1]).ToString();
                    switch (interpreter.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interpreter.dataHolder.ashOrcSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.ashOrcHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.ashOrcEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interpreter.dataHolder.sandOrcSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.sandOrcHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.sandOrcEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interpreter.dataHolder.plainsOrcSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.plainsOrcHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.plainsOrcEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interpreter.dataHolder.swampOrcSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.swampOrcHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.swampOrcEyeColor[interpreter.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interpreter.dataHolder.bloodOrcSkinColor[interpreter.dnaBytes[5]];
                            hairColor.text = interpreter.dataHolder.bloodOrcHairColor[interpreter.dnaBytes[6]];
                            eyeColor.text = interpreter.dataHolder.bloodOrcEyeColor[interpreter.dnaBytes[7]];
                            break;
                    }
                    break;
            }

            Text subrace = this.subrace;
            subrace.text = subrace.text + " " + race.text;
        }
        else
        {
            Debug.Log("Subrace or skin color or hair color or eye color text is missing!");
        }

        if(GetComponent<SpriteManagerV2>())
        {
            GetComponent<SpriteManagerV2>().AssignSprites();


            if (!GetComponent<SpriteManagerV2>().bodyRenderer) return;


            if (handedness.text == "Right")
            {
                Transform trans = GetComponent<SpriteManagerV2>().bodyRenderer.transform;

                if(trans.localScale.x > 0f)
                trans.localScale = new Vector3(-trans.localScale.x, trans.localScale.y, trans.localScale.z);
            }
            else if (handedness.text == "Left")
            {
                Transform trans = GetComponent<SpriteManagerV2>().bodyRenderer.transform;

                if (trans.localScale.x < 0f)
                    trans.localScale = new Vector3(-trans.localScale.x, trans.localScale.y, trans.localScale.z);
            }
        }
    }

    protected virtual void CalculatePrice()
    {
        price.text = "$";
        string sellPriceInDollars = "" + ipc.ipcStorage.m_price/100;
        int sellPriceLength = sellPriceInDollars.Length;
        int priceLengthMod = sellPriceLength % 3;
        if (priceLengthMod == 0) { priceLengthMod = 3; }
        for (int i = 0; i < priceLengthMod; ++i)
        {
            price.text += sellPriceInDollars[i];
        }
        if (sellPriceLength > 3)
        {
            price.text += ",";
            for (int i = 0; i < 3; ++i)
            {
                price.text += sellPriceInDollars[i + priceLengthMod];
            }
        }
        if (sellPriceLength > 6)
        {
            price.text += ",";
            for (int i = 0; i < 3; ++i)
            {
                price.text += sellPriceInDollars[i + priceLengthMod + 3];
            }
        }
        price.text += ".";
        if (ipc.ipcStorage.m_price % 100 < 10)
        {
            price.text += "0";
        }
        price.text += (ipc.ipcStorage.m_price % 100);
    }
}

