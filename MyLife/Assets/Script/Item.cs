using UnityEngine;

public class Item : MonoBehaviour
{
    public int item_number; //번호
    public string item_Name; // 이름
    public Sprite item_Sprite; // 이미지 
    public int item_type; //아이템 타입

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
