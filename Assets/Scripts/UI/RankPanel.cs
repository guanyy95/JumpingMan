using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    private Button btn_Back;
    public Text[] txt_Scores;
    private GameObject go_ScoreList;
    
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnCloseButtonClick);
        go_ScoreList = transform.Find("scoreList").gameObject;
        btn_Back.GetComponent<Image>().color = new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g,
            btn_Back.GetComponent<Image>().color.b, 0);
        go_ScoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
    }


    private void Show()
    {
        gameObject.SetActive(true);
        btn_Back.GetComponent<Image>().DOColor(new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g,
            btn_Back.GetComponent<Image>().color.b, 0.3f), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.one, 0.3f);

        int[] arr = GameManager.Instance.GetScoreArr();
        for (int i = 0; i < arr.Length; i++)
        {
            txt_Scores[i].text = arr[i].ToString();
        }

    }

    private void OnCloseButtonClick()
    {
        btn_Back.GetComponent<Image>().DOColor(new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g,
            btn_Back.GetComponent<Image>().color.b, 0), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=>
        {
            gameObject.SetActive(false);
        });
    }
    
}
