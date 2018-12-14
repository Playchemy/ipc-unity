using UnityEngine;
using UnityEngine.UI;

public class ScribeFromCache : Scribe
{
    private IPC_Data ipcData;
    private InterpretFromCache interp;
    public SubraceSymbolData symbolData;
    public Image subraceSymbolImage;

    private void Start()
    {
        if (!autoTranscribe)
            return;
        Transcribe();
    }

    public override void Transcribe()
    {
        ipcData = GetComponent<InterpretFromCache>().ipcData;
        interp = GetComponent<InterpretFromCache>();
        if (ipcId) ipcId.text = "#" + ipcData.ipc_id;

        if (ipcName) ipcName.text = ipcData.ipc_name;

        if (timeOfBirth) timeOfBirth.text = interp.timeOfBirth;
        if (xp) xp.text = ipcData.ipc_xp.ToString();
        CalculatePrice();
        // QR.sprite = Sprite.Create( generateQR(string.Empty + (object) ipc.ipcId), new Rect(0.0f, 0.0f, 256f, 256f), new Vector2());
        if (explosive) explosive.text = interp.attributeBytes[0].ToString();
        if (sustained) sustained.text = interp.attributeBytes[1].ToString();
        if (tolerance) tolerance.text = interp.attributeBytes[2].ToString(); ;
        if (strength) strength.text = (interp.attributeBytes[0] + interp.attributeBytes[1] + interp.attributeBytes[2]).ToString(); ;
        if (speed) speed.text = interp.attributeBytes[3].ToString(); ;
        if (precision) precision.text = interp.attributeBytes[4].ToString(); ;
        if (reaction) reaction.text = interp.attributeBytes[5].ToString(); ;
        if (dexterity) dexterity.text = (interp.attributeBytes[3] + interp.attributeBytes[4] + interp.attributeBytes[5]).ToString(); ;
        if (memory) memory.text = interp.attributeBytes[6].ToString(); ;
        if (calculus) calculus.text = interp.attributeBytes[7].ToString(); ;
        if (simulation) simulation.text = interp.attributeBytes[8].ToString(); ;
        if (intelligence) intelligence.text = (interp.attributeBytes[6] + interp.attributeBytes[7] + interp.attributeBytes[8]).ToString(); ;
        if (healing) healing.text = interp.attributeBytes[9].ToString(); ;
        if (fortitude) fortitude.text = interp.attributeBytes[10].ToString();
        if (growth) growth.text = interp.attributeBytes[11].ToString(); ;
        if (constitution) constitution.text = (interp.attributeBytes[9] + interp.attributeBytes[10] + interp.attributeBytes[11]).ToString(); ;
        if (luck) luck.text = interp.attributeBytes[12].ToString(); ;
        if (race) race.text = ((Interpreter.Race)interp.dnaBytes[0]).ToString();
        if (gender) gender.text = ((Interpreter.Gender)interp.dnaBytes[2]).ToString();
        if (height) height.text = (interp.dnaBytes[3] / 12) + "'" + (interp.dnaBytes[3] % 12) + "\"";
        if (handedness) handedness.text = ((Interpreter.Handedness)interp.dnaBytes[4]).ToString();
        if (publicAddress) publicAddress.text = ipcData.ipc_owner;

        if (ipcName && ipcData.ipc_name == "")
        {
            ipcName.text = gender.text + race.text + ipcData.ipc_id;
        }

        if (this.subrace && skinColor && hairColor && eyeColor)
        {
            switch (interp.dnaBytes[0])
            {
                case 1:
                    this.subrace.text = ((Interpreter.elfSubrace)interp.dnaBytes[1]).ToString();
                    switch (interp.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interp.dataHolder.nightElfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.nightElfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.nightElfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interp.dataHolder.woodElfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.woodElfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.woodElfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interp.dataHolder.highElfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.highElfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.highElfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interp.dataHolder.sunElfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.sunElfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.sunElfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interp.dataHolder.darkElfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.darkElfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.darkElfEyeColor[interp.dnaBytes[7]];
                            break;
                    }
                    break;

                case 2:
                    this.subrace.text = ((Interpreter.humanSubrace)interp.dnaBytes[1]).ToString();
                    switch (interp.dnaBytes[1])
                    {

                        case 1:
                            skinColor.text = interp.dataHolder.mythicalHumanSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.mythicalHumanHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.mythicalHumanEyeColor[interp.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interp.dataHolder.nordicHumanSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.nordicHumanHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.nordicHumanEyeColor[interp.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interp.dataHolder.easternHumanSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.easternHumanHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.easternHumanEyeColor[interp.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interp.dataHolder.coastalHumanSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.coastalHumanHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.coastalHumanEyeColor[interp.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interp.dataHolder.southernHumanSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.southernHumanHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.southernHumanEyeColor[interp.dnaBytes[7]];
                            break;
                    }
                    break;

                case 3:
                    this.subrace.text = ((Interpreter.dwarfSubrace)interp.dnaBytes[1]).ToString();
                    switch (interp.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interp.dataHolder.quarryDwarfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.quarryDwarfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.quarryDwarfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interp.dataHolder.mountainDwarfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.mountainDwarfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.mountainDwarfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interp.dataHolder.lumberDwarfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.lumberDwarfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.lumberDwarfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interp.dataHolder.hillDwarfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.hillDwarfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.hillDwarfEyeColor[interp.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interp.dataHolder.volcanoDwarfSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.volcanoDwarfHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.volcanoDwarfEyeColor[interp.dnaBytes[7]];
                            break;
                    }
                    break;

                case 4:
                    this.subrace.text = ((Interpreter.orcSubrace)interp.dnaBytes[1]).ToString();
                    switch (interp.dnaBytes[1])
                    {
                        case 1:
                            skinColor.text = interp.dataHolder.ashOrcSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.ashOrcHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.ashOrcEyeColor[interp.dnaBytes[7]];
                            break;
                        case 2:
                            skinColor.text = interp.dataHolder.sandOrcSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.sandOrcHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.sandOrcEyeColor[interp.dnaBytes[7]];
                            break;
                        case 3:
                            skinColor.text = interp.dataHolder.plainsOrcSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.plainsOrcHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.plainsOrcEyeColor[interp.dnaBytes[7]];
                            break;
                        case 4:
                            skinColor.text = interp.dataHolder.swampOrcSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.swampOrcHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.swampOrcEyeColor[interp.dnaBytes[7]];
                            break;
                        case 5:
                            skinColor.text = interp.dataHolder.bloodOrcSkinColor[interp.dnaBytes[5]];
                            hairColor.text = interp.dataHolder.bloodOrcHairColor[interp.dnaBytes[6]];
                            eyeColor.text = interp.dataHolder.bloodOrcEyeColor[interp.dnaBytes[7]];
                            break;
                    }
                    break;
            }

            Text subrace = this.subrace;
            subrace.text = subrace.text + " " + race.text;

            if(symbolData && subraceSymbolImage)
            {
                subraceSymbolImage.sprite = symbolData.GetSubraceSymbol(subrace.text);
            }
        }
        else
        {
            Debug.Log("Subrace or skin color or hair color or eye color text is missing!");
        }

        if (GetComponent<SpriteManagerV2>())
        {
            GetComponent<SpriteManagerV2>().AssignSprites();


            if (!GetComponent<SpriteManagerV2>().bodyRenderer) return;


            if (handedness.text == "Right")
            {
                Transform trans = GetComponent<SpriteManagerV2>().bodyRenderer.transform;

                if (trans.localScale.x > 0f)
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

    protected override void CalculatePrice()
    {
        price.text = "$";
        string sellPriceInDollars = "" + ipcData.ipc_price / 100;
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
        if (ipcData.ipc_price % 100 < 10)
        {
            price.text += "0";
        }
        price.text += (ipcData.ipc_price % 100);
    }
}

