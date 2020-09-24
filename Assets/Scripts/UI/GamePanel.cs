using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
  private Button btn_Pause;
  private Button btn_Continue;

  private Text txt_Score;
  private Text txt_DiamondCount;

  private void Awake()
  {
    EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
    EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
    EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
    Init();
  }
  
  private void Init()
  {
    btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
    btn_Continue = transform.Find("btn_Continue").GetComponent<Button>();
    txt_Score = transform.Find("txt_Score").GetComponent<Text>();
  
    txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();

    btn_Continue.onClick.AddListener(OnPlayButtonClick);
    btn_Pause.onClick.AddListener(OnPauseButtonClick);
    
    btn_Continue.gameObject.SetActive(false);
    gameObject.SetActive(false);
  }

  private void OnDestroy()
  {
    EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
    EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
    EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
  }
  
  private void Show()
  {
    gameObject.SetActive(true);
  }

  /// <summary>
  /// 更新钻石数量显示
  /// </summary>
  private void UpdateDiamondText(int diamond)
  {
    if(txt_DiamondCount != null)
      txt_DiamondCount.text = diamond.ToString();
  }

  /// <summary>
  /// 更新成绩显示text
  /// </summary>
  /// <param name="score"></param>
  private void UpdateScoreText(int score)
  {
    txt_Score.text = score.ToString();
  }

  /// <summary>
  /// 暂停按钮点击
  /// </summary>
  private void OnPauseButtonClick()
  {
    EventCenter.Broadcast(EventDefine.PlayClickAudio);
    btn_Continue.gameObject.SetActive(true);
    btn_Pause.gameObject.SetActive(false);
    // 游戏暂停
    
    Time.timeScale = 0;
    GameManager.Instance.isPause = true;
  }
  /// <summary>
  /// 开始按钮点击
  /// </summary>
  private void OnPlayButtonClick()
  {
    EventCenter.Broadcast(EventDefine.PlayClickAudio);
    btn_Continue.gameObject.SetActive(false);
    btn_Pause.gameObject.SetActive(true);
    
    // 继续游戏
    Time.timeScale = 1;
    GameManager.Instance.isPause = false;
  }
}
