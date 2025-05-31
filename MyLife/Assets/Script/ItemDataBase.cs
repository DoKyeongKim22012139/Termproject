using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
   public  Item[] itemlist;

    public Item returnItem(string name)
    {
        Item correct=null;

        for (int i = 0; i < itemlist.Length;i++) 
        {
            if(itemlist[i].item_Name == name)
                correct = itemlist[i];
        }



        return correct;
    }
}
