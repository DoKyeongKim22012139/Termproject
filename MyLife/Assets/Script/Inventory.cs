
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;
using System.IO;
using static SaveData;

public class Inventory : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "inventory.json"); //저장 파일 경로

    public GameObject inventoryPanel;
    public List<SlotData> slots = new List<SlotData>();
    bool ActiveInventory = false;
    public GameObject slotPrefab;
    public Transform slotParent;
    public ItemDataBase itemDatabase;

    private void Start()
    {
        CreatSlot();
        LoadInventory();
       
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory = !ActiveInventory;
            inventoryPanel.SetActive(ActiveInventory);
        }
    }



    void CreatSlot()
    {
        for(int i=0;i< 16;i++)
        {
            GameObject making_s = Instantiate(slotPrefab, slotParent);
            SlotData data = new SlotData();
            data.slotObj = making_s;

            slots.Add(data);

            SlotButton sb = making_s.GetComponent<SlotButton>();
            sb.slotdata=data;
        }
    }
    public void AddItem(Item item)
    {

        foreach (SlotData slot in slots)
        {
            if (!slot.isEmpty && slot.name ==item.item_Name)
            {
                
             
                slot.count++;
               
                slot.item = item;
                

                

                Text countText = slot.slotObj.GetComponentInChildren<Text>();
                countText.text = slot.count.ToString();
         
                return;
            }
        }




        foreach (SlotData slot in slots)
        {
            if(slot.isEmpty)
            {
                slot.isEmpty = false;
                slot.name = item.item_Name;
                slot.count++;
                
                slot.item = item;
                Debug.Log(slot.item);
                Image iconImage = slot.slotObj.transform.Find("Item Image").GetComponent<Image>();
                iconImage.sprite = item.item_Sprite;
                iconImage.color = Color.white;

                Text countText = slot.slotObj.GetComponentInChildren<Text>();
                countText.text =slot.count.ToString();
                countText.color = Color.white;
                return;
            }
        }

        Debug.Log("가득참");
    }



    public void SaveInventory()
    {
      
        SaveData.InventorySaveData saveData = new SaveData.InventorySaveData();
        foreach (SlotData slot in slots)
        {
            if (slot != null && !slot.isEmpty && slot.item != null) //슬롯이 비어있는지 확인
            {
                //차있으면 이름과 개수 저장
                saveData.slots.Add(new SaveData.SlotSaveData { itemName = slot.item.item_Name,count = slot.count});
            }
        }
        //json으로 쓰기
        string json = JsonUtility.ToJson(saveData, true);
       

        File.WriteAllText(SavePath, json);
        

    }
  
    public void LoadInventory()
    {
        //없으면 못하게
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        SaveData.InventorySaveData saveData = JsonUtility.FromJson<SaveData.InventorySaveData>(json);

        
        foreach (var savedslot in saveData.slots)
        {
            Item putitem = itemDatabase.returnItem(savedslot.itemName);
            for(int i=0; i<savedslot.count;i++)
                 AddItem(putitem);
        }



    }
}
