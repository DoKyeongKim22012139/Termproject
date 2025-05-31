using UnityEngine;

public class SlotButton : MonoBehaviour
{

    public SlotData slotdata;
    public void OnClickSlot()
    {
        if(slotdata != null)
        {
            GameManager.instance.OnslotClick(slotdata);
        }
    
    }
}
