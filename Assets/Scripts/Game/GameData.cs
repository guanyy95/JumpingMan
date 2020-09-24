using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static bool IsAgainGame = false; // 重新开始游戏flag

    private bool isFirstGame; // 是否第一次进行游戏
    private bool isMusicOn; //声音是否开启
    private int[] bestScoreArr; // 分数
    private int selectSkin; 
    private bool[] skinUnlocked;
    private int diamondCount;

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }

    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }

    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }

    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }

    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
        
    }

    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }

    public int GetSelectSkin()
    {
        return selectSkin;
    }

    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }

    public int GetDiamondCount()
    {
        return diamondCount;
    }

}
