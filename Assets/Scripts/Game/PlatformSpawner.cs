using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public enum PlatformGroupType
{
    Grass,
    Winter
}

public class PlatformSpawner : MonoBehaviour
{
    public Vector3 startSpawnPos; // 第一个平台的生成位置
    public Vector3 platformSpawnPosition; //下一个平台的生成位置
    public int mileStoneCount = 10; // 里程碑数
    public float fallTime; //掉落时间
    public float minFallTime; // 最小的掉落时间
    public float factorOfFall; // 掉落系数
    
    private Vector3 spikeDirPlatformPos; // 钉子方向平台的位置
    private int spawnPlatformCount;
    private ManagerVars vars;
    private bool isLeftSpawn = false; // 判断生成方向
    private bool spikeSpwanLeft = false; // 钉子组合平台是否生成在左边，反之生成在右边
    private int afterSpawnSpikeCount; // 生成钉子平台后的平台的数量
    private bool isSpawnSpike;
    
    private Sprite selectPlatformSprite; //选择平台的类型图片
    private PlatformGroupType platformType; // 平台类型


    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = ManagerVars.GetManagerVars();
    }



    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }

        GameObject go = Instantiate(vars.characterPrefab);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver)
        {
            UpdateFallTime();
        }
    }

    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime()
    {
        if (GameManager.Instance.GetGameScore() > mileStoneCount)
        {
            mileStoneCount *= 2;
            fallTime += factorOfFall;
            if (fallTime < minFallTime)
                fallTime = minFallTime;
        }
    }

    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if (ran == 2)
        {
            platformType = PlatformGroupType.Winter;
        }
        else
        {
            platformType = PlatformGroupType.Grass;
        }
    }

    /// <summary>
    /// 路径确认
    /// </summary>
    private void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }

        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {

        int ranObstacleDir = Random.Range(0, 2);
        //生成单个平台
        if (spawnPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        //生成组合平台
        else if (spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            // 生成通用组合平台
            if (ran == 0)
            {
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            // 生成祖逖组合平台
            else if (ran == 1)
            {
                switch (platformType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                }

            }
            //生成钉子组合平台
            else
            {
                int val = -1;
                if (isLeftSpawn)
                {
                    // 生成右手钉子
                    val = 0;
                }
                else
                {
                    // 生成左手钉子
                    val = 1;
                }
                SpawnSpikePlatform(val);
                
                isSpawnSpike = true;
                afterSpawnSpikeCount = 4;
                //钉子生成在左边
                if (spikeSpwanLeft)
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f,
                        platformSpawnPosition.y + vars.nextYPos,
                        0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f,
                        platformSpawnPosition.y + vars.nextYPos,
                        0);
                }
            }
        }

        int ranSpawnDiamond = Random.Range(0, 7);
        
        // 初始化钻石金币
        if (ranSpawnDiamond == 6 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = ObjectPool.Instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPosition.x, platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }

        //向左生成
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos,
                0);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos,
                0);
        }
    }

    /// <summary>
    /// 生成普通平台（单一种类）
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject pt = ObjectPool.Instance.GetNormalPlatform();
        pt.transform.position = platformSpawnPosition;
        pt.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , ranObstacleDir);
        pt.SetActive(true);
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合
    /// </summary>
    private void SpawnGrassPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    /// <param name="directon">方向</param>
    private void SpawnSpikePlatform(int directon)
    {
        GameObject temp = null;
        if (directon == 0)
        {
            spikeSpwanLeft = false;
            temp = ObjectPool.Instance.GetLeftSpikePlatform();
        }
        else
        {
            spikeSpwanLeft = true;
            temp = ObjectPool.Instance.GetRightSpikePlatform();
        }

        temp.transform.position = platformSpawnPosition;
        temp.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , directon);
        temp.SetActive(true);
    }
    
    /// <summary>
    /// 生成钉子平台之后需要生成的平台
    /// 包括钉子方向， 也包括原来的方向
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (afterSpawnSpikeCount > 0)
        {
            afterSpawnSpikeCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject go = ObjectPool.Instance.GetNormalPlatform();
                if (i == 0)// 生成原来方向的平台
                {
                    //如果钉子在左边, 原先路径就是右边
                    if (spikeSpwanLeft)
                    {
                        go.transform.position = platformSpawnPosition;
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                            platformSpawnPosition.y+ vars.nextYPos, 0);
                    }
                    else // 如果钉子在右边，那原先的路径就是在左边
                    {
                        go.transform.position = platformSpawnPosition;
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                            platformSpawnPosition.y+ vars.nextYPos, 0);
                    }
                }
                else // 生成钉子方向的平台
                {
                    go.transform.position = spikeDirPlatformPos;
                    //钉子生在左边
                    if (spikeSpwanLeft)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x  - vars.nextXPos,
                            spikeDirPlatformPos.y+ vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos,
                            spikeDirPlatformPos.y+ vars.nextYPos, 0);
                    }
                }
                
                go.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime , 1);
                go.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
