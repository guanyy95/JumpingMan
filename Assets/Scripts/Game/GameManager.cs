using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ManagerVars vars;
    // 游戏是否开始
    public bool isGameStarted { get; set; }
    
    // 游戏是否结束
    public bool isGameOver { get; set; }
    // 游戏是否暂停
    public bool isPause { get; set; }
    //游戏成绩
    private int gameSocre = 0;
    // 判断玩家是否开始进行跳跃
    public bool PlayerIsMove { get; set; }
    private int gameDiamond;
    
    /// <summary>
    /// 数据类所有类型
    /// </summary>
    private GameData data;
    public static bool IsAgainGame = false; // 重新开始游戏flag
    private bool isFirstGame; // 是否第一次进行游戏
    private bool isMusicOn; //声音是否开启
    private int[] bestScoreArr; // 分数
    private int selectSkin; 
    private bool[] skinUnlocked;
    private int diamondCount;
    
    
    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.AddSocre, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);
        if (GameData.IsAgainGame)
        {
            isGameStarted = true;
        }
        InitGameData();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddSocre, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
    }
    
    /// <summary>
    /// 保存最高分
    /// </summary>
    /// <param name="socre"></param>
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        // 从大到小排序
        list.Sort((x,y) =>(-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }

        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }

        bestScoreArr[index] = score;
        Save();
    }

    /// <summary>
    /// 获得最高分
    /// </summary>
    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }
    
    /// <summary>
    /// 获取排名数组
    /// </summary>
    /// <returns></returns>
    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        // 从大到小排序
        list.Sort((x,y) =>(-x.CompareTo(y)));
        bestScoreArr = list.ToArray();
        return bestScoreArr;
    }

    /// <summary>
    /// 玩家是否移动
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }

    /// <summary>
    /// 增加游戏分数
    /// </summary>
    private void AddGameScore()
    {
         if(isGameStarted == false || isGameOver || isPause) return;
         gameSocre++;
         EventCenter.Broadcast(EventDefine.UpdateScoreText, gameSocre);
    }
    /// <summary>
    /// 获取游戏成绩
    /// </summary>
    /// <returns></returns>
    public int GetGameScore()
    {
        return gameSocre;
    }

    /// <summary>
    /// 更新游戏钻石数量
    /// </summary>
    private void AddGameDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);
    }

    /// <summary>
    /// 获得已有钻石数
    /// </summary>
    /// <returns></returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }
    
    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }
    
    /// <summary>
    /// 设置皮肤解锁
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }
    
    /// <summary>
    /// 获得所有的钻石数量
    /// </summary>
    /// <returns></returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    
    /// <summary>
    /// 更新购买后的总钻石数量
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    
    /// <summary>
    /// 设置当前选择的皮肤序号
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }

    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }
    
    /// <summary>
    /// 设置音效是否开启
    /// </summary>
    /// <param name="val"></param>
    public void SetIsMusicOn(bool val)
    {
        isMusicOn = val;
        Save();
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }
        
        //如果第一次开始游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            
            data = new GameData();
            Save();
        }
        else
        {
            isMusicOn = data.GetIsMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
        }
    }

    /// <summary>
    /// 存储游戏数据
    /// </summary>
    private void Save()
    {
        try
        {
            // 初始化二进制转换器
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetIsFirstGame(isFirstGame);
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                data.SetIsMusicOn(isMusicOn);
                bf.Serialize(fs, data);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 读取游戏数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;
        
        Save();
    }
}
