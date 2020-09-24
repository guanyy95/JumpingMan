using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateManagerVarsContent")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    public List<Sprite> bgThemeSpriteList= new List<Sprite>();
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public List<Sprite> skinSpriteList = new List<Sprite>();
    public List<string> skinNameList = new List<string>();
    public List<int> skinPrice = new List<int>();
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    
    public GameObject skinChooseItem;
    public GameObject deathEffect;
    public GameObject normalPlatform;
    public GameObject characterPrefab;
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject diamondPrefab;
    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip;
    public Sprite musicOn, musicOff;
}
