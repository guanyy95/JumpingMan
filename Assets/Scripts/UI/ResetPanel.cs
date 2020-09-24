using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class ResetPanel : MonoBehaviour
{
    private Button btn_Yes;
    private Button btn_No;
    private Image img_Bg;
    private GameObject dialog;
    
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPanel, Show);
        img_Bg = transform.Find("bg").GetComponent<Image>();
        btn_Yes = transform.Find("Dialog/btn_Yes").GetComponent<Button>();
        btn_No = transform.Find("Dialog/btn_No").GetComponent<Button>();
        dialog = transform.Find("Dialog").gameObject;
        
        btn_Yes.onClick.AddListener(OnYesButtonClick);
        btn_No.onClick.AddListener(OnNoButtonClick);
        
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3F);
    }
    
    /// <summary>
    /// yes按钮点击
    /// </summary>
    private void OnYesButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    /// <summary>
    /// No按钮点击
    /// </summary>
    private void OnNoButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
