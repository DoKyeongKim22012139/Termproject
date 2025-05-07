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

    //기본공격 쿨타임 변수
    private float curTime=0f;
    public float coolTime = 0.5f;

    // 공격 박스 변수
    public Vector2 boxSize = new Vector2(2f, 1.5f);
    public float boxdistance = 1.5f;
    public Transform pos;
    public Hp hp;

    private Vector2 lastMoveDir;
    bool isHorizonMove;
    Animator anime;
    Rigidbody2D rigid;
    SpriteRenderer spr;
    Vector2 boxcenter;


    private void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
        anime=GetComponent<Animator>();
        spr=GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        bool attack_key;

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp =  Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");


        if (h != 0 || v != 0)
        {
            lastMoveDir = new Vector2(h, v).normalized;
        }




        if (hDown || vUp)
            isHorizonMove = true;
        else if(vDown || hUp)
            isHorizonMove = false;

        curTime -= Time.deltaTime;

        if (curTime <=0)
        {
            attack_key = Input.GetButtonDown("Jump");
            
            if (attack_key)
            {
                anime.SetTrigger("IsAttack");
                curTime = coolTime;
                Attack();
            }

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

        if(h!=0) //-1일떄는 true 1일때는 false if문을 줄인것
        {
            spr.flipX = h<0;
        }
    }
    private void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2 (h,0) : new Vector2 (0,v);
        rigid.linearVelocity = moveVec * speed;
        
    }


    void Attack()
    {
        Vector2 attackDir = new Vector2(h, v);
        //Debug.Log($"h: {h}, v: {v}");
        if (attackDir == Vector2.zero)
            attackDir = lastMoveDir;
        else
        {
            attackDir.Normalize(); //움직이는 중이면 움직이는 방향
        }

        Vector2 boxcenter = (Vector2)pos.position + attackDir * boxdistance;


        Collider2D[] hits = Physics2D.OverlapBoxAll(boxcenter, boxSize, 0);


        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "Enemy")
            {
                hp = hit.GetComponent<Hp>();
                //Debug.Log("적감지됨" + hit.name);
                //Debug.Log("공격방향: " + attackDir + " | boxcenter: " + boxcenter);
                hp.TakeDamage(5);
               
            }
        }

        
    }


    

}
