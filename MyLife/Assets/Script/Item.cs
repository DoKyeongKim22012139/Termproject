using UnityEngine;

public class Item : MonoBehaviour
{
    public int item_number; //��ȣ
    public string item_Name; // �̸�
    public Sprite item_Sprite; // �̹��� 
    public int item_type; //������ Ÿ��

    public bool isGet = false;
    public bool autoPickup = true;

    private void OnEnable()
    {
        
        isGet = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!autoPickup)
            return;

        if(collision.gameObject.tag=="Player" && !isGet)
        {
            isGet = true;
            GameManager.instance.GetItem(this);
            HideItem();
        }
    }


    void HideItem()
    {
        gameObject.SetActive(false);
    }



}
