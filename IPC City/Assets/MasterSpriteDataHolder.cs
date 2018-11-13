using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Master Sprite Data Holder", menuName = "Master Sprite Data Holder", order = 1)]
public class MasterSpriteDataHolder : ScriptableObject
{
    [Space(8f)]
    public Ipc_SpriteData maleHuman;
    public Ipc_SpriteData femaleHuman;

    [Space(8f)]
    public Ipc_SpriteData maleOrc;
    public Ipc_SpriteData femaleOrc;

    [Space(8f)]
    public Ipc_SpriteData maleHair;
    public Ipc_SpriteData femaleHair;


    [Space(20f)]
    [Header("Clothes Objects")]
    public Ipc_SpriteData maleHumanClothes;
    public Ipc_SpriteData femaleHumanClothes;
    [Space(8f)]
    public Ipc_SpriteData maleElfClothes;
    public Ipc_SpriteData femaleElfClothes;
    [Space(8f)]
    public Ipc_SpriteData maleDwarfClothes;
    public Ipc_SpriteData femaleDwarfClothes;
    [Space(8f)]
    public Ipc_SpriteData maleOrcClothes;
    public Ipc_SpriteData femaleOrcClothes;

    void Start()
    {

    }

    void Update()
    {

    }
}
