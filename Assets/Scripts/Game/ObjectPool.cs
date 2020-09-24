using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
   public static ObjectPool Instance;
   
   public int initSpawnCount = 5;
   private List<GameObject> normalPlatformList = new List<GameObject>();
   private List<GameObject> commonPlatformList = new List<GameObject>();
   private List<GameObject> grassPlatformList = new List<GameObject>();
   private List<GameObject> winterlPlatformList = new List<GameObject>();
   private List<GameObject> spikeLeftPlatformList = new List<GameObject>();
   private List<GameObject> spikeRightPlatformList = new List<GameObject>();
   private List<GameObject> deathEffectList = new List<GameObject>();
   private List<GameObject> diamondList = new List<GameObject>();
   private ManagerVars vars;
   private void Awake()
   {     
      Instance = this;
      vars = ManagerVars.GetManagerVars();
      Init();
   }

   private void Init()
   {
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         InstantiateObject(vars.normalPlatform, ref normalPlatformList);
      }
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
         {
            InstantiateObject(vars.commonPlatformGroup[j], ref commonPlatformList);
         }
      }
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
         {
            InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);
         }
      }
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
         {
            InstantiateObject(vars.winterPlatformGroup[j], ref winterlPlatformList);
         }
      }

      for (int i = 0; i < initSpawnCount; i++)
      {
         InstantiateObject(vars.spikePlatformLeft, ref  spikeLeftPlatformList);
      }
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         InstantiateObject(vars.spikePlatformRight, ref  spikeRightPlatformList);
      }
      
      for (int i = 0; i < initSpawnCount; i++)
      {
         InstantiateObject(vars.deathEffect, ref  deathEffectList);
      }
      for (int i = 0; i < initSpawnCount; i++)
      {
         InstantiateObject(vars.diamondPrefab, ref  diamondList);
      }
   }
   
   /// <summary>
   /// 初始化List
   /// </summary>
   /// <param name="prefab"></param>
   /// <param name="addList"></param>
   private GameObject InstantiateObject(GameObject prefab, ref List<GameObject> addList)
   {
      GameObject go = Instantiate(prefab, transform);
      go.SetActive(false);
      addList.Add(go);
      return go;
   }

   /// <summary>
   /// 获取对象池中的 noraml平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetNormalPlatform()
   {
      foreach (var go in normalPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }

      return InstantiateObject(vars.normalPlatform, ref normalPlatformList);
   }
   /// <summary>
   /// 获取common平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetCommonPlatformGroup()
   {
      foreach (var go in commonPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }
      int ran = Random.Range(0, vars.commonPlatformGroup.Count);
      return InstantiateObject(vars.commonPlatformGroup[ran], ref commonPlatformList);
   }
   
   /// <summary>
   /// 获取grass平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetGrassPlatformGroup()
   {
      foreach (var go in grassPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }
      int ran = Random.Range(0, vars.grassPlatformGroup.Count);
      return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
   } 
   /// <summary>
   /// 获取winter平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetWinterPlatformGroup()
   {
      foreach (var go in winterlPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }
      int ran = Random.Range(0, vars.winterPlatformGroup.Count);
      return InstantiateObject(vars.winterPlatformGroup[ran], ref winterlPlatformList);
   } 
   
   /// <summary>
   /// 获取spike Left平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetLeftSpikePlatform()
   {
      foreach (var go in spikeLeftPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }

      return InstantiateObject(vars.spikePlatformLeft, ref spikeLeftPlatformList);
   }
   
   /// <summary>
   /// 获取spike Right平台
   /// </summary>
   /// <returns></returns>
   public GameObject GetRightSpikePlatform()
   {
      foreach (var go in spikeRightPlatformList)
      {
         if (go.activeInHierarchy == false)
            return go;
      }

      return InstantiateObject(vars.spikePlatformRight, ref spikeRightPlatformList);
   }
   
   /// <summary>
   /// 获取死亡特效
   /// </summary>
   /// <returns></returns>
   public GameObject GetDeathEffect()
   {
      foreach (var effect in deathEffectList)
      {
         if (effect.activeInHierarchy == false)
         {
            return effect;
         }
      }
      return InstantiateObject(vars.deathEffect, ref deathEffectList);
   }
   /// <summary>
   /// 获取钻石
   /// </summary>
   /// <returns></returns>
   public GameObject GetDiamond()
   {
      foreach (var diamond in diamondList)
      {
         if (diamond.activeInHierarchy == false)
         {
            return diamond;
         }
      }
      return InstantiateObject(vars.diamondPrefab, ref diamondList);
   }
}
