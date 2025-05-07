using NUnit.Framework.Constraints;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anime;
    Rigidbody2D rigid;
    SpriteRenderer M_sprite;
    CapsuleCollider2D M_collider;

    Hp Enemy_hp;
    public Transform Player_ts;
    bool istracing = false;

    public int Monster_nextmove;
    public int Monster_nextmove_y;


    private void Awake()
    {
       Enemy_hp =GetComponent<Hp>();
       rigid = GetComponent<Rigidbody2D>();
       anime = GetComponent<Animator>();
       Invoke("Think", 2);
       M_sprite= GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        //������
        rigid.linearVelocity = new Vector2(Monster_nextmove,Monster_nextmove_y);

        //���� �� ���ϱ�
        Vector2 frontVec = new Vector2(rigid.position.x + Monster_nextmove * 2, rigid.position.y);
        Vector2 Dirvec = new Vector2(Monster_nextmove, 0);
        //Debug.DrawRay(frontVec, Dirvec * 1f, Color.red);
        //RaycastHit2D find_player = Physics2D.Raycast(rigid.position, Dirvec, 2, LayerMask.GetMask("Player"));

        Collider2D player_find = Physics2D.OverlapCircle(rigid.position, 3f, LayerMask.GetMask("Player")); // ���� ����

        if (player_find !=null)   // find_player.collider !=nul ���̽� ���ǹ�
        {
            //Debug.Log("�÷��̾� �߰�");
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


        if(Enemy_hp.hp <=0)
        {
            Enemy_hp.die();
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
        

        
            //���� �������� ������ ���� �ð�
        float next_walk_Random = Random.Range(2f, 5f);
        Invoke("Think", next_walk_Random);
    }


    void Player_trace()
    {
        float dir = Player_ts.position.x - transform.position.x;
        float dir_Y = Player_ts.position.y - transform.position.y;
        Monster_nextmove = dir > 0 ? 1 : -1; //0���� ũ�� 1 �ƴϸ� -1
        Monster_nextmove_y =dir_Y >0 ? 1 : -1;   
        anime.SetInteger("WalkSpeed", Monster_nextmove);

        if (Monster_nextmove != 0)
        {
            M_sprite.flipX = Monster_nextmove == -1;
        }

        CancelInvoke("Think");

        Invoke("Think", 3);
    }
}
