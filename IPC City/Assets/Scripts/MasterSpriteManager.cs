using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpriteManager : MonoBehaviour {

    [Space(8f)]
    public Ipc_SpriteData maleHuman;
    public Ipc_SpriteData femaleHuman;

    [Space(8f)]
    public Ipc_SpriteData maleOrc;
    public Ipc_SpriteData femaleOrc;

    [Space(8f)]
    public Ipc_SpriteData maleDwarf;
    public Ipc_SpriteData femaleDwarf;

    [Space(8f)]
    public Ipc_SpriteData maleElf;
    public Ipc_SpriteData femaleElf;

    [Space(8f)]
    public Ipc_SpriteData maleHair;
    public Ipc_SpriteData femaleHair;

    [Space(8f)]
    public List<Ipc_SpriteData> maleClothes;
    public List<Ipc_SpriteData> femaleClothes;


    [Space(8f)]
    public List<Color> colorList;


    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void ChooseSprite(string gender, string race, SpriteHandler assignTo)
    {
        Ipc_SpriteData tempData = maleHuman;

        if(gender == "Male")
        {
            assignTo.clothesData = maleClothes[0];
            assignTo.hairData = maleHair;

            if (race == "Human") tempData = maleHuman;

            if (race == "Orc")
            {
                tempData = maleOrc;
                assignTo.hairRend.enabled = false;
                assignTo.clothesRend.enabled = false;

            }

            if (race == "Dwarf") tempData = maleDwarf;

            if (race == "Elf") tempData = maleElf;
        }
        else if (gender == "Female")
        {
            assignTo.hairData = femaleHair;
            assignTo.clothesData = femaleClothes[0];

            if (race == "Human") tempData = femaleHuman;

            if (race == "Orc")
            {
                tempData = femaleOrc;
                assignTo.hairRend.enabled = false;
                assignTo.clothesRend.enabled = false;
            }

            if (race == "Dwarf") tempData = femaleDwarf;

            if (race == "Elf")
            {
                assignTo.clothesData = femaleClothes[1];
                tempData = femaleElf;
            }
        }

        if(race == "Dwarf")
        {
            Vector3 scale = new Vector3(assignTo.transform.localScale.x, assignTo.transform.localScale.y*0.9f, assignTo.transform.localScale.z);
            assignTo.transform.localScale = scale;


            Transform nameobj = assignTo.GetComponent<IPC_Stats>().overheadName.transform;
            nameobj.localScale = new Vector3(nameobj.localScale.x, nameobj.localScale.y * 1.1f, nameobj.localScale.z);
        }

        assignTo.skinData = tempData;
        assignTo.hairRend.color = colorList[Random.Range(0, colorList.Count)];

        /*
        assignTo.walkingUp.Clear();
        assignTo.walkingRight.Clear();
        assignTo.walkingLeft.Clear();
        assignTo.walkingDown.Clear();

        assignTo.pose.Clear();
        assignTo.laugh.Clear();
        assignTo.shake.Clear();
        assignTo.nod.Clear();

        assignTo.walkingUp = tempData.walkingUp;
        assignTo.walkingRight = tempData.walkingRight;
        assignTo.walkingLeft = tempData.walkingLeft;
        assignTo.walkingDown = tempData.walkingDown;

        assignTo.pose = tempData.pose;
        assignTo.laugh = tempData.laugh;
        assignTo.shake = tempData.shake;
        assignTo.nod = tempData.nod;
        */



    }
}
