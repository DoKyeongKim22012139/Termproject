using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    Animator anime;
    Rigidbody2D rigid;
    SpriteRenderer M_sprite;
    CapsuleCollider2D M_collider;

    bool isDead = false;
    Hp Enemy_hp;
    public Transform Player_ts;
    bool istracing = false;

    public int Monster_nextmove;
    public int Monster_nextmove_y;

    public GameObject item_Potion;
    public GameObject item_Ashes;
    public ObjManager Obj_Mananger;



    private void Awake()
    {
       Enemy_hp =GetComponent<Hp>();
       rigid = GetComponent<Rigidbody2D>();
       anime = GetComponent<Animator>();
       Invoke("Think", 2);
       M_sprite= GetComponent<SpriteRenderer>();
    }

    private void OnEnable() //생성될때 초기설정
    {
        Enemy_hp.current_hp= 10;
        isDead = false;
        rigid.simulated = true;
    }

    void FixedUpdate()
    {
        //움직임
        rigid.linearVelocity = new Vector2(Monster_nextmove,Monster_nextmove_y);

        //몬스터 앞 구하기
        Vector2 frontVec = new Vector2(rigid.position.x + Monster_nextmove * 2, rigid.position.y);
        Vector2 Dirvec = new Vector2(Monster_nextmove, 0);
        //Debug.DrawRay(frontVec, Dirvec * 1f, Color.red);
        //RaycastHit2D find_player = Physics2D.Raycast(rigid.position, Dirvec, 2, LayerMask.GetMask("Player"));

        Collider2D player_find = Physics2D.OverlapCircle(rigid.position, 2f, LayerMask.GetMask("Player")); // 범위 추적

        if (player_find !=null)   // find_player.collider !=nul 레이쏠때 조건문
        {
            //Debug.Log("플레이어 발견");
            if (!istracing)
            {
                istracing = true;
                Player_trace();
            }

            else
            {
                Player_trace();
            }

        }

        else
        {
            istracing = false;
        }


        if(!isDead && Enemy_hp.current_hp <=0)
        {
            isDead = true;
            Enemy_hp.die();
            rigid.simulated = false;
            Drop_item();
            Invoke("Disapper", 1f);
        }
    }


    void Think()
    {
       Monster_nextmove = Random.Range(-1, 2);
        Monster_nextmove_y = 0;
        anime.SetInteger("WalkSpeed", Monster_nextmove);

         if (Monster_nextmove != 0)
         {
            M_sprite.flipX = Monster_nextmove == -1;
         }
        

        
            //다음 움직임을 실행할 랜덤 시간
        float next_walk_Random = Random.Range(2f, 5f);
        Invoke("Think", next_walk_Random);
    }


    void Player_trace()
    {
        float dir = Player_ts.position.x - transform.position.x;
        float dir_Y = Player_ts.position.y - transform.position.y;
        Monster_nextmove = dir > 0 ? 1 : -1; //0보다 크면 1 아니면 -1
        Monster_nextmove_y =dir_Y >0 ? 1 : -1;   
        anime.SetInteger("WalkSpeed", Monster_nextmove);

        if (Monster_nextmove != 0)
        {
            M_sprite.flipX = Monster_nextmove == -1;
        }

        CancelInvoke("Think");

        Invoke("Think", 3);
    }

    void Drop_item()
    {
        int ran = Random.Range(0, 10);

        if(ran <3)
        {
            Debug.Log("NO, item");
        }

        else if(ran <7)
        {
            GameObject item_Ashes = Obj_Mananger.MakeObj("Skeleton_ashes");
            item_Ashes.transform.position= transform.position;
        }

        else if(ran <10)
        {
            GameObject item_potion = Obj_Mananger.MakeObj("Enemy_potion");
            item_potion.transform.position= transform.position; 
        }
    }

    void Disapper()
    {
        gameObject.SetActive(false);
        Obj_Mananger.Enemy_now_Spawn--;
    }
}
