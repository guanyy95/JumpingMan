using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameoverPanel : MonoBehaviour
{
    public Text txt_Score, txt_MaxScore, txt_Diamond;
    public Button btn_Restart, btn_Rank, btn_Home;
    public Image imge_New;
    private void Awake()
    {
        btn_Restart.onClick.AddListener(OnRestartButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);
    }

    /// <summary>
    /// 在面板上显示数据
    /// </summary>
    private void Show()
    {
        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            imge_New.gameObject.SetActive(true);
            txt_MaxScore.text = "最高分 " + GameManager.Instance.GetGameScore();
        }
        else
        {
            imge_New.gameObject.SetActive(false);
            txt_MaxScore.text = "最高分 " + GameManager.Instance.GetBestScore();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        txt_Diamond.text = "+" + GameManager.Instance.GetGameDiamond().ToString();
        GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetAllDiamond());
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 再来一局按钮事件
    /// </summary>
    private void OnRestartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
    }
    
    /// <summary>
    /// 排行榜事件按钮
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    
    /// <summary>
    /// 主页事件按钮
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }
}
