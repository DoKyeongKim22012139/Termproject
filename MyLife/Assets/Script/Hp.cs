using UnityEngine;

public class Hp : MonoBehaviour
{
    public float max_hp;
    public float current_hp;
    Animator animator;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        current_hp= max_hp;
    }

   
    public void TakeDamage(int damage)
    {
        if (0 <= current_hp)
        {
            current_hp -= damage;
        }

    }


    public void die()
    {
        animator.SetTrigger("IsDie");
    }

    
}
