using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteData", menuName = "SpriteDataHolder", order = 1)]
public class Ipc_SpriteData : ScriptableObject
{

    public string genderRace;

    [Header("Walking Sprites")]
    public List<Sprite> walkingUp;
    public List<Sprite> walkingDown;
    public List<Sprite> walkingLeft;
    public List<Sprite> walkingRight;

    [Header("Emote Sprites")]
    public List<Sprite> pose;
    public List<Sprite> laugh;
    public List<Sprite> shake;
    public List<Sprite> nod;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
