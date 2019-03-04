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

    public SpriteRenderer skinSpriteRenderer;
    public SpriteRenderer clothesSpriteRenderer;
    public SpriteRenderer hairSpriteRenderer;
    public SpriteRenderer accessorySpriteRenderer;

    public Ipc_SpriteData clothesData;
    public Ipc_SpriteData skinData;
    public Ipc_SpriteData hairData;
    public Ipc_SpriteData accessoryData;

    public List<Sprite> masterSpriteList;

    [ContextMenu("Assign Sprites")]
    private void AssignSprites()
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

    private Sprite FindInList(string name)
    {
        Sprite temp = masterSpriteList.Where(obj => obj.name == name).SingleOrDefault();
        return temp;
    }

    public void WalkUp(int value)
    {
        if (clothesRend && clothesData) clothesRend.sprite = clothesData.walkingUp[value];
        if (hairRend && hairData) hairRend.sprite = hairData.walkingUp[value];
        if (skinRenderer && skinData) skinRenderer.sprite = skinData.walkingUp[value];
        if (accessoryRend && accessoryData) accessoryRend.sprite = accessoryData.walkingUp[value];

        if (skinSpriteRenderer && skinData) skinSpriteRenderer.sprite = skinData.walkingUp[value];
        if (clothesSpriteRenderer && clothesData) clothesSpriteRenderer.sprite = clothesData.walkingUp[value];
        if (hairSpriteRenderer && hairData) hairSpriteRenderer.sprite = hairData.walkingUp[value];
        if (accessorySpriteRenderer && accessoryData) accessorySpriteRenderer.sprite = accessoryData.walkingUp[value];
    }

    public void WalkDown(int value)
    {
        if (clothesRend && clothesData) clothesRend.sprite = clothesData.walkingDown[value];
        if (hairRend && hairData) hairRend.sprite = hairData.walkingDown[value];
        if (skinRenderer && skinData) skinRenderer.sprite = skinData.walkingDown[value];
        if (accessoryRend && accessoryData) accessoryRend.sprite = accessoryData.walkingDown[value];

        if (skinSpriteRenderer && skinData) skinSpriteRenderer.sprite = skinData.walkingDown[value];
        if (clothesSpriteRenderer && clothesData) clothesSpriteRenderer.sprite = clothesData.walkingDown[value];
        if (hairSpriteRenderer && hairData) hairSpriteRenderer.sprite = hairData.walkingDown[value];
        if (accessorySpriteRenderer && accessoryData) accessorySpriteRenderer.sprite = accessoryData.walkingDown[value];
    }

    public void WalkLeft(int value)
    {
        if (clothesRend && clothesData) clothesRend.sprite = clothesData.walkingLeft[value];
        if (hairRend && hairData) hairRend.sprite = hairData.walkingLeft[value];
        if (skinRenderer && skinData) skinRenderer.sprite = skinData.walkingLeft[value];
        if (accessoryRend && accessoryData) accessoryRend.sprite = accessoryData.walkingLeft[value];

        if (skinSpriteRenderer && skinData) skinSpriteRenderer.sprite = skinData.walkingLeft[value];
        if (clothesSpriteRenderer && clothesData) clothesSpriteRenderer.sprite = clothesData.walkingLeft[value];
        if (hairSpriteRenderer && hairData) hairSpriteRenderer.sprite = hairData.walkingLeft[value];
        if (accessorySpriteRenderer && accessoryData) accessorySpriteRenderer.sprite = accessoryData.walkingLeft[value];
    }

    public void WalkRight(int value)
    {
        if (clothesRend && clothesData) clothesRend.sprite = clothesData.walkingRight[value];
        if (hairRend && hairData) hairRend.sprite = hairData.walkingRight[value];
        if (skinRenderer && skinData) skinRenderer.sprite = skinData.walkingRight[value];
        if (accessoryRend && accessoryData) accessoryRend.sprite = accessoryData.walkingRight[value];

        if (skinSpriteRenderer && skinData) skinSpriteRenderer.sprite = skinData.walkingRight[value];
        if (clothesSpriteRenderer && clothesData) clothesSpriteRenderer.sprite = clothesData.walkingRight[value];
        if (hairSpriteRenderer && hairData) hairSpriteRenderer.sprite = hairData.walkingRight[value];
        if (accessorySpriteRenderer && accessoryData) accessorySpriteRenderer.sprite = accessoryData.walkingRight[value];
    }

    public void CharSheetPose()
    {
        if (clothesRend && clothesData) clothesRend.sprite = clothesData.walkingLeft[4];
        if (hairRend && hairData) hairRend.sprite = hairData.walkingLeft[4];
        if (skinRenderer && skinData) skinRenderer.sprite = skinData.walkingLeft[4];
        if (accessoryRend && accessoryData) accessoryRend.sprite = accessoryData.walkingLeft[4];

        if (skinSpriteRenderer && skinData) skinSpriteRenderer.sprite = skinData.walkingLeft[4];
        if (clothesSpriteRenderer && clothesData) clothesSpriteRenderer.sprite = clothesData.walkingLeft[4];
        if (hairSpriteRenderer && hairData) hairSpriteRenderer.sprite = hairData.walkingLeft[4];
        if (accessorySpriteRenderer && accessoryData) accessorySpriteRenderer.sprite = accessoryData.walkingLeft[4];
    }

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
}
