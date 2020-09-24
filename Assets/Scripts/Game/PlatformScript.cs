using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
   public SpriteRenderer[] SpriteRenderers;
   public GameObject obstacle;
   private bool startTimer; //计时器
   private float fallTime;

   private Rigidbody2D my_Body;

   private void Awake()
   {
      my_Body = GetComponent<Rigidbody2D>();
   }

   public void Init(Sprite sprite,float fallTime, int obstacleDir)
   {
      my_Body.bodyType = RigidbodyType2D.Static;
      this.fallTime = fallTime;
      startTimer = true;
      foreach (var item in SpriteRenderers)
      {
         item.sprite = sprite;
      }
      
      // 0 为障碍物朝阳右
      if (obstacleDir == 0)
      {
         if (obstacle != null)
         {
            obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x,
               obstacle.transform.localPosition.y,
               0);
         }
      }
   }

   private void Update()
   {
      if (GameManager.Instance.isGameStarted == false|| GameManager.Instance.PlayerIsMove == false) return;
      
      if (startTimer)
      {
         fallTime -= Time.deltaTime;
         
         // 倒计时结束
         if (fallTime < 0)
         {
            startTimer = false;
            if (my_Body.bodyType != RigidbodyType2D.Dynamic)
            {
               my_Body.bodyType = RigidbodyType2D.Dynamic;
               StartCoroutine(DealyHide());
            }
         }
      }

      if (transform.position.y - Camera.main.transform.position.y < -6)
      {
         StartCoroutine(DealyHide());
      }
   }
   /// <summary>
   /// 平台延迟掉落， 每一个platform 都会 计算和主摄像头的距离，超过距离就会掉落
   /// </summary>
   /// <returns></returns>
   private IEnumerator DealyHide()
   {
      yield return new WaitForSeconds(1f);
      gameObject.SetActive(false);
   }
}
