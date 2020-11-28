using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, ITurnReceiver
{
    [SerializeField] protected Mover mover = null;
    [SerializeField] protected TileSelector tileSelector = null;
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private GameObject fallCamera = null;
    [SerializeField] public string nameOfCharacter = null;
    [SerializeField] private Text heartIndicator = null;
    [SerializeField] private Text goldIndicator = null;
    
    public bool isDead = false;
    public bool worldCollision = false;

    protected Animator animator = null;
    protected Rigidbody rigid;
    protected OnTurnEnd onTurnEnd = null;
    protected bool isMyTurn = false;
    protected bool pressR = false;
    [SerializeField] public int Gold;
    [SerializeField] public int MaxGold;
    [SerializeField] public int Heart;
    [SerializeField] public int MaxHeart;
    GameObject nearItem;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        Gold = 0;
        UpdateHeartIndicator();
        UpdateGoldIndicator();
    }

    public void ReceiveTurn(OnTurnEnd onTurnEnd)
    {
        if (!isMyTurn)
        {
            this.onTurnEnd = onTurnEnd;
            isMyTurn = true;
            InitTurn();
        }
    }

    protected virtual void InitTurn()
    {
        tileSelector.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isMyTurn)
        {  
            GetInput();
            Interaction();
            if (Input.GetKeyDown(KeyCode.W))
            {
                tileSelector.SetDir("W");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tileSelector.SetDir("A");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tileSelector.SetDir("S");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tileSelector.SetDir("D");
            }

            if (tileSelector.enabled && tileSelector.SelectedTile != null)
            {
                Vector3 lookAtPos = tileSelector.SelectedTile.position;
                lookAtPos.y = transform.position.y;
                 transform.LookAt(lookAtPos);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetPlayerCameraActive(true);
                    mover.Move(tileSelector.SelectedTile.position, OnArrival);
                    animator.SetTrigger("Walk");
                    tileSelector.enabled = false;
                }
            }
        }
    }

    void Interaction()// 아이템 먹기
    {
        if(nearItem != null)
        {
            if ((pressR) && (nearItem.tag == "Item"))
            {
                Item item = nearItem.GetComponent<Item>();
                switch (item.type)
                {
                    case Item.Type.Gold:
                        Gold += item.value;
                        UpdateGoldIndicator();
                        break;
                    case Item.Type.Heart:
                        Heart += item.value;
                        if (Heart > MaxHeart)
                            Heart = MaxHeart;
                        UpdateHeartIndicator();
                        break;
                }
                Destroy(nearItem);
            }
        }  
    }

    void GetInput() // 아이템 먹는 키 R을 누르는지 확인하기
    {
        pressR = Input.GetButtonDown("getItem");
    }
    protected virtual void OnArrival()
    {
        SetPlayerCameraActive(false);
        animator.SetTrigger("Idle");
        isMyTurn = false;
        onTurnEnd();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Monster")
        {
            mover.Stop();
            /*
            if (isMyTurn && Vector3.Angle(transform.forward, other.transform.forward) < 120)
            {
                animator.SetTrigger("Attack");
            } 
            */
            if(Gold >=300){
                Debug.Log("공격");
                animator.SetTrigger("Attack");
            }
            else
            {
                OnDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallBound")
        {
            if (Heart <= 0)
            {
                OnDeath();
            }
            else
            {
                --Heart;
                UpdateHeartIndicator();
                revive();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            nearItem = other.gameObject;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            nearItem = null;
        }
    }

    protected virtual void OnDeath()
    {
        SetPlayerCameraActive(true);
        fallCamera.SetActive(true);
        animator.SetTrigger("Die");
        worldCollision = true;
        Debug.Log("worldCollision : " + worldCollision);
        isDead = true;
    }
    protected virtual void revive()
    {
        Debug.Log("revive");
        transform.position = new Vector3(0, 7.5f, -8);

    }

    private void SetPlayerCameraActive(bool isActive)
    {
        if (playerCamera != null)
        {
            playerCamera.SetActive(isActive);
        }
    }

    public string GetCharacterName()
    {
        return nameOfCharacter;
    }

    public int GetGold()
    {
        return Gold;
    }


    private void UpdateHeartIndicator()
    {
        heartIndicator.text = Heart.ToString() + '/' + MaxHeart.ToString();
    }

    private void UpdateGoldIndicator()
    {
        goldIndicator.text = (Gold/100).ToString()+'/'+MaxGold.ToString();
    }
}
