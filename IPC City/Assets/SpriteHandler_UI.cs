using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SpriteHandler_UI : MonoBehaviour
{

    public Image skinRenderer;
    public Image clothesRend;
    public Image hairRend;
    public Image accessoryRend;

    public Ipc_SpriteData clothesData;
    public Ipc_SpriteData skinData;
    public Ipc_SpriteData hairData;
    public Ipc_SpriteData accessoryData;

    public List<Sprite> masterSpriteList;





    void Start()
    {
    }

    [ContextMenu("Sort List")]
    void SortList()
    {
        /*
        walkingUp = data.walkingUp;
        walkingDown = data.walkingDown;
        walkingLeft = data.walkingLeft;
        walkingRight = data.walkingRight;

        pose = data.walkingRight;
        laugh = data.walkingRight;
        shake = data.walkingRight;
        nod = data.walkingRight;

        return;

        masterSpriteList = masterSpriteList.OrderBy(tile => tile.name).ToList();
        */
    }

    [ContextMenu("Assign Sprites")]
    void AssignSprites()
    {

        for (int i = 60; i < 69; i++)
        {
            clothesData.walkingUp.Add(masterSpriteList[i]);
        }

        for (int i = 78; i < 87; i++)
        {
            clothesData.walkingDown.Add(masterSpriteList[i]);
        }

        for (int i = 69; i < 78; i++)
        {
            clothesData.walkingLeft.Add(masterSpriteList[i]);
        }

        for (int i = 87; i < 96; i++)
        {
            clothesData.walkingRight.Add(masterSpriteList[i]);
        }
    }

    Sprite FindInList(string name)
    {
        Sprite temp = masterSpriteList.Where(obj => obj.name == name).SingleOrDefault();
        return temp;
    }

    public void WalkUp(int value)
    {
        if (clothesData) clothesRend.sprite = clothesData.walkingUp[value];
        if (hairData) hairRend.sprite = hairData.walkingUp[value];
        if (skinData) skinRenderer.sprite = skinData.walkingUp[value];
        if (accessoryData) accessoryRend.sprite = accessoryData.walkingUp[value];
    }

    public void WalkDown(int value)
    {
        if (clothesData) clothesRend.sprite = clothesData.walkingDown[value];
        if (hairData) hairRend.sprite = hairData.walkingDown[value];
        if (skinData) skinRenderer.sprite = skinData.walkingDown[value];
        if (accessoryData) accessoryRend.sprite = accessoryData.walkingDown[value];

    }

    public void WalkLeft(int value)
    {
        if (clothesData) clothesRend.sprite = clothesData.walkingLeft[value];
        if (hairData) hairRend.sprite = hairData.walkingLeft[value];
        if (skinData) skinRenderer.sprite = skinData.walkingLeft[value];
        if (accessoryData) accessoryRend.sprite = accessoryData.walkingLeft[value];

    }

    public void WalkRight(int value)
    {
        if (clothesData) clothesRend.sprite = clothesData.walkingRight[value];
        if (hairData) hairRend.sprite = hairData.walkingRight[value];
        if (skinData) skinRenderer.sprite = skinData.walkingRight[value];
        if (accessoryData) accessoryRend.sprite = accessoryData.walkingRight[value];

    }

    public void CharSheetPose()
    {
        if (clothesData) clothesRend.sprite = clothesData.walkingLeft[4];
        if (hairData) hairRend.sprite = hairData.walkingLeft[4];
        if (skinData) skinRenderer.sprite = skinData.walkingLeft[4];
        if (accessoryData) accessoryRend.sprite = accessoryData.walkingLeft[4];

    }
    ///////

    public void Pose(int value)
    {
        return;

    }

    public void Laugh(int value)
    {
        return;
    }

    public void Shake(int value)
    {
        return;
    }

    public void Nod(int value)
    {
        return;
    }


    void Update()
    {

    }
}
