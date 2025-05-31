
using System.Collections.Generic;
using System;

public class SaveData 
{

    [Serializable]
    public class SlotSaveData
    {
        public string itemName;
        public int count;
    }

    [Serializable]
    public class InventorySaveData
    {
        public List<SlotSaveData> slots = new List<SlotSaveData>();
    }
}
