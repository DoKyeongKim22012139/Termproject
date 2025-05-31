using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Player : MonoBehaviour
{
    float v;
    float h;
    public int speed;

    //��ȭ�� �����ɽ�Ʈ ����
    Vector3 dirVec;
    GameObject scanObject;
    //�⺻���� ��Ÿ�� ����
    private float curTime=0f;
    public float coolTime = 0.5f;

    public bool getRod = false;
    public bool getSword=false;
    public bool isEquipped=false;

    // ���� �ڽ� ����
    public Vector2 boxSize = new Vector2(2f, 1.5f);
    public float boxdistance = 1.5f;
    public Transform pos;

    //��ũ��Ʈ
    public Hp hp;
    public GameManager gameManager;

    //���� ����
    public float waitTime = 3f;
    public float castDistance = 2f;
    public Transform rodTip;         // ���˴� ��(�� ����߸� �����)
    public LayerMask waterLayer;


    private Vector2 lastMoveDir;
    bool isHorizonMove;
    Animator anime;
    Rigidbody2D rigid;
    SpriteRenderer spr;
    Vector2 boxcenter;
   

    private void Awake()
    {
        hp=GetComponent<Hp>();
        rigid= GetComponent<Rigidbody2D>();
        anime=GetComponent<Animator>();
        spr=GetComponent<SpriteRenderer>();
        
}


    private void Update()
    {
        rodTip = transform;
        // ���� ��ȣ�ۿ����϶� �������̰�


        h = gameManager.isAction ? 0: Input.GetAxisRaw("Horizontal");
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");
        bool attack_key;

        bool hDown = gameManager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = gameManager.isAction ? false :Input.GetButtonDown("Vertical");
        bool hUp = gameManager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = gameManager.isAction ? false : Input.GetButtonUp("Vertical");


        if (h != 0 || v != 0)
        {
            lastMoveDir = new Vector2(h, v).normalized;
        }




        if (hDown || vUp)
            isHorizonMove = true;
        else if(vDown || hUp)
            isHorizonMove = false;

        curTime -= Time.deltaTime;



        
        if (curTime <=0 && getSword)
        {
            attack_key = Input.GetButtonDown("Jump");
            
            if (attack_key)
            {
                anime.SetTrigger("IsAttack");
                curTime = coolTime;
                Attack();
            }

        }
        
        if(getRod && Input.GetButtonDown("Jump"))
        {
            Fishing();
        }
       

        if (anime.GetInteger("hAxisRaw") != h)
        {
            anime.SetBool("isChange", true);
            anime.SetInteger("hAxisRaw", (int)h);
        }
        else if (anime.GetInteger("vAxisRaw") != v)
        {
            anime.SetBool("isChange", true);
            anime.SetInteger("vAxisRaw", (int)v);
        }

        else
        {
            anime.SetBool("isChange", false);
        }

        if(h!=0) //-1�ϋ��� true 1�϶��� false if���� ���ΰ�
        {
            spr.flipX = h<0;
        }

        
        //dir ����Ȯ��
        if (vDown && v == 1)
        {
            dirVec = Vector3.up;
        }

        else if (vDown && v == -1)
        {
            dirVec = Vector3.down;
        }
        else if (hDown && h == -1)
        {
            dirVec = Vector3.left;
        }
        else if (hDown && h == 1)
        {
            dirVec = Vector3.right;
        }

        if(Input.GetButtonDown("Jump") && scanObject!=null)
        {
            gameManager.Action(scanObject);
        }


    }
    private void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2 (h,0) : new Vector2 (0,v);
        rigid.linearVelocity = moveVec * speed;

        
        Interaction();
       
    }


    void Attack()
    {
        Vector2 attackDir = new Vector2(h, v);
        if (attackDir == Vector2.zero)
            attackDir = lastMoveDir;
        else
        {
            attackDir.Normalize(); //�����̴� ���̸� �����̴� ����
        }

        Vector2 boxcenter = (Vector2)pos.position + attackDir * boxdistance;


        Collider2D[] hits = Physics2D.OverlapBoxAll(boxcenter, boxSize, 0);


        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "Enemy")
            {
                Hp enemy_hp = hit.GetComponent<Hp>();
                //Debug.Log("��������" + hit.name);
                //Debug.Log("���ݹ���: " + attackDir + " | boxcenter: " + boxcenter);
                enemy_hp.TakeDamage(5);
               
            }
        }

        
    }


    void Interaction()
    {
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }


    void Fishing()
    {
        gameManager.isAction = true;
        //1. ĳ����
        anime.SetTrigger("Cast");

        //������ �������� Ȯ��
        bool OnWater = Physics2D.Raycast(rodTip.position, dirVec, castDistance, waterLayer);

        if (!OnWater)
        {
            Invoke("Reel", 1.5f);
            gameManager.isAction = false;
            return;
        }

        Invoke("DoingFishing", 1.5f);
        

        Invoke("EndFishing", 3f);

        Invoke("Movecant", 3f);


        int indx = Random.Range(0, gameManager.fishlist.Length);
        Item caught = gameManager.fishlist[indx];
        gameManager.GetItem(caught);
    }

    void Reel()
    {
            anime.SetTrigger("Reel");
    }

    void DoingFishing()
    {
        anime.SetBool("IsFishing", true);
    }

    void EndFishing()
    {
        anime.SetBool("IsFishing", false);
    }

    void Movecant()
    {
        gameManager.isAction = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            hp.TakeDamage(5);
        }
    }
}
