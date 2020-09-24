using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    private ManagerVars vars;
    private Transform parent;
    private Text txt_Name;
    private Text txt_Diamond;
    private Button btn_Back;
    private Button btn_Select;
    private Button btn_Buy;
    private int selectIndex;
    
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPanel, Show);

        parent = transform.Find("ScrollReact/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/Diamond_Count").GetComponent<Text>();
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        
        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(onBuyButtonClick);
        vars = ManagerVars.GetManagerVars();
    }
    
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, Show);
    }

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 资源初始化
    /// </summary>
    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 302);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinChooseItem, parent);
            
            //皮肤未解锁
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }

            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i+1) * 160, 0, 0);
        }
        //打开页面 直接定位到选定的皮肤
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetCurrentSelectedSkin() * -160, 0);
    }
    
    private void Update()
    { 
        selectIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160.0f);
       if (Input.GetMouseButtonUp(0))
       {
           parent.transform.DOLocalMoveX(selectIndex * -160, 0.2f);
           // parent.transform.localPosition = new Vector3(current_index * -160, 0);
       }

       SetItemSize(selectIndex);
       RefreshUI(selectIndex);
    }
    
    /// <summary>
    /// ShopPanel的展示监听事件
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }


    /// <summary>
    /// 返回点击事件
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 购买点击事件
    /// </summary>
    private void onBuyButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if (price > GameManager.Instance.GetAllDiamond())
        {
            Debug.Log("钻石不足，无法购买");
            EventCenter.Broadcast(EventDefine.ShowHint, "钻石不足");
            return;
        }
        GameManager.Instance.UpdateAllDiamond(-price);
        GameManager.Instance.SetSkinUnlocked(selectIndex);
        parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// 选择角色按钮点击
    /// </summary>
    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ChangeSkin, selectIndex);
        GameManager.Instance.SetSelectSkin(selectIndex);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 将滑动条选中的皮肤进行放大
    /// </summary>
    /// <param name="index"></param>
    private void SetItemSize(int index)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }
    
    /// <summary>
    ///  更新选中的皮肤名称
    /// </summary>
    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.skinNameList[selectIndex];
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();
        if (GameManager.Instance.GetSkinUnlocked(selectIndex) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }

}
