using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Volume;
    private Button btn_Reset;
    private ManagerVars vars;
    
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        Init();
    }
    
    /// <summary>
    /// 更换主页面图标 UI
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int skinIndex)
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite =
            vars.skinSpriteList[skinIndex];
    }

    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            gameObject.SetActive(false);
        }
        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());
        Sound();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }

    /// <summary>
    /// 显示出面板Event 事件
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Init()
    {
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
        btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
        btn_Volume = transform.Find("Btns/btn_Volume").GetComponent<Button>();
        btn_Reset = transform.Find("Btns/btn_Reset").GetComponent<Button>();
        
        btn_Start.onClick.AddListener(OnStartButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Shop.onClick.AddListener(OnShopButtonClick);
        btn_Volume.onClick.AddListener(OnVolumeButtonClick);
        btn_Reset.onClick.AddListener(OnResetButtonClick);
    }
    
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.isGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 商店按钮点击
    /// </summary>
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    
    /// <summary>
    /// 音效按钮点击
    /// </summary>
    private void OnVolumeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        Sound();
    }

    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Volume.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Volume.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());
    }

    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }
}
