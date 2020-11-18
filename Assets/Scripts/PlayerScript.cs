using UnityEngine;

public class PlayerScript : MonoBehaviour, ITurnReceiver
{
    [SerializeField] protected Mover mover = null;
    [SerializeField] protected TileSelector tileSelector = null;
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private GameObject fallCamera = null;
    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    protected Animator animator = null;
    protected OnTurnEnd onTurnEnd = null;
    protected bool isMyTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            //GetComponent<BoxCollider>().enabled = false;
            //GetComponent<Rigidbody>().isKinematic = true;
            if (isMyTurn && Vector3.Angle(transform.forward, other.transform.forward) < 120)
            {
                animator.SetTrigger("Attack");
            } else
            {
                OnDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallBound")
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        SetPlayerCameraActive(true);
        fallCamera.SetActive(true);
        animator.SetTrigger("Die");
    }

    private void SetPlayerCameraActive(bool isActive)
    {
        if (playerCamera != null)
        {
            playerCamera.SetActive(isActive);
        }
    }
}
