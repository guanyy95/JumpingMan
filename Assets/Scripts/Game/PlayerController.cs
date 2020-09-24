using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private bool isMoveLeft = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private bool isJumping = false; // 是否正在跳跃
    private Rigidbody2D m_body;
    private SpriteRenderer _spriteRenderer;
    private bool isMove = false;
    private AudioSource m_AudioSource;
    
    public LayerMask platformLayer, obstacleLayer;
    public Transform ray_down, ray_left, ray_right;
    private void Awake()
    {
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        vars = ManagerVars.GetManagerVars();
        m_body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void ChangeSkin(int skinIndex)
    {
        _spriteRenderer.sprite = vars.characterSkinSpriteList[skinIndex];
    }

    void Update()
    {
        Debug.DrawRay(ray_down.position, Vector2.down * 1, Color.red);
        Debug.DrawRay(ray_left.position, Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(ray_right.position, Vector2.right * 0.15f, Color.red);
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (GameManager.Instance.isGameStarted == false || GameManager.Instance.isGameOver)
            return;
        
        if (GameManager.Instance.isGameStarted == false || GameManager.Instance.isGameOver ||
            GameManager.Instance.isPause)
            return;
        
        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (isMove == false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath);
            Vector3 mousePos = Input.mousePosition;
            isJumping = true;
            //点击左半屏幕
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            //点击屏幕右半边
            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }
        
        // 走错路跳下去的判断
        if (m_body.velocity.y < -10f && !isRayPlatform() && GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            Debug.Log("触发了掉落");
            _spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.isGameOver = true;
            //调出结束面板
            // todo
            StartCoroutine(DelayShowGameOverPanel());
        }

        if (isJumping && isRayObstacle() && GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.hitClip);
            Debug.Log("发生了触发障碍物判断");
            GameObject effect = ObjectPool.Instance.GetDeathEffect();
            effect.SetActive(true);
            effect.transform.position = transform.position;
            GameManager.Instance.isGameOver = true;
            _spriteRenderer.enabled = false;
            StartCoroutine(DelayShowGameOverPanel());
            // Destroy(gameObject);
  
        }

        if (transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            print("平台到掉落游戏结束");
            GameManager.Instance.isGameOver = true;
            gameObject.SetActive(false);
            StartCoroutine(DelayShowGameOverPanel());
        }
    }
    
    /// <summary>
    /// 延迟调出结束面板
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.0f);
        // 调出结束面板
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }

    private GameObject lastHitGO = null;
    
    /// <summary>
    /// 检测玩家是否掉落，进行向下的射线检测
    /// </summary>
    /// <returns></returns>
    private bool isRayPlatform()
    {
        RaycastHit2D hit= Physics2D.Raycast(ray_down.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                // if (lastHitGO != hit.collider.gameObject)
                // {
                //     // if (lastHitGO == null)
                //     // {
                //     //     return true;
                //     // }
                
                    lastHitGO = hit.collider.gameObject;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检测玩家是否碰撞到obstacle
    /// </summary>
    /// <returns></returns>
    private bool isRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(ray_left.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(ray_right.position, Vector2.right, 0.15f, obstacleLayer);
        
        
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        return false;
    }

    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1,1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f,0.15f);
        }
        else
        {
            //保持原状不动(1, 1, 1)
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f,0.15f);
            transform.localScale = Vector3.one;
        }

        if (!GameManager.Instance.isGameOver)
        {
            EventCenter.Broadcast(EventDefine.AddSocre);
        }
    }
    
    /// <summary>
    /// 玩家和平台的碰撞检测
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos= other.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos,
                                             currentPlatformPos.y + vars.nextYPos,
                                                0);
            
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos,
                currentPlatformPos.y + vars.nextYPos,
                0);
        }
    }
    /// <summary>
    /// 检测金币碰撞
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Pickup")
        {
            m_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            //吃到钻石
            other.gameObject.SetActive(false);
        }
    }
    
    private void IsMusicOn(bool val)
    {
        m_AudioSource.mute = !val;
    }
}
