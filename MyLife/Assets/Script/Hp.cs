using UnityEngine;

public class Hp : MonoBehaviour
{
    public int hp;
    Animator animator;



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        if (0 < hp)
        {
            hp -= damage;
        }

    }


    public void die()
    {
        animator.SetTrigger("IsDie");
        Destroy(gameObject, 0.4f);
    }

    
}
