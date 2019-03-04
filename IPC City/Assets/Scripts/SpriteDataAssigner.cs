using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDataAssigner", menuName = "SpriteDataAssigner", order = 1)]
public class SpriteDataAssigner : ScriptableObject
{

    public Ipc_SpriteData Destination;

    public List<Sprite> masterSpriteList;

    [ContextMenu("Assign Sprites")]
    void AssignSprites()
    {

        for (int i = 60; i < 69; i++)
        {
            Destination.walkingUp.Add(masterSpriteList[i]);
        }

        for (int i = 78; i < 87; i++)
        {
            Destination.walkingDown.Add(masterSpriteList[i]);
        }

        for (int i = 69; i < 78; i++)
        {
            Destination.walkingLeft.Add(masterSpriteList[i]);
        }

        for (int i = 87; i < 96; i++)
        {
            Destination.walkingRight.Add(masterSpriteList[i]);
        }
    }
}
