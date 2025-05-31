using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isAction = false;
    public int talkindex;
    public Text talkText;
    public Transform spawnpoint;
    public GameObject equipui;
    public Text equipButtonText;
    private SlotData currentslot;

    public int Sword;
    public int Rod;
    public GameObject SW;
    public GameObject RD;

    private GameObject rewardTarget; // 아이템을 먹고 없앨거 저장하느곳 
    public Slider hpbar;


    public Hp PlayerHp;
    public TalkManager talkManager;
    public ObjManager Obj_Manager;
    public Player player;
    public Transform[] Enemy_Spawnpoint;
    public Inventory inventory;

    public GameObject menuset;
    public GameObject TalkPanel;
    public GameObject Scanobj;

    public Item[] fishlist;

    private void Awake()
    {
        // 이미 인스턴스가 있으면 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; // 자기 자신을 저장
        DontDestroyOnLoad(gameObject); // 씬 전환에도 살아남게
    }

    private void Start()
    {
        
        hpbar.value = PlayerHp.current_hp / PlayerHp.max_hp;
        PlayerLoad();

        if(Sword ==1)
            SW.SetActive(false);
        if(Rod ==1)
            RD.SetActive(false);
    }


    private void Update()
    {
        HandleHP();
        EnemySpawn();
        if (Input.GetButtonDown("Cancel"))
        {
            submenuactive();
        }

        if (PlayerHp.current_hp <= 0)
            Respawn();



    }

    void EnemySpawn()
    {
        if(Obj_Manager.Enemy_now_Spawn >=5)
        {
            //Debug.Log("최대 소환 수 도달  더이상 소환 x");
            return;
        }


        int ranPoint = Random.Range(0, 5);
        GameObject enemy = Obj_Manager.MakeObj("Skeleton"); //프리펩에서 연결이 안되니 여기서 enemy 스크립트랑 obj를 연결시켜준다.
        Enemy enemy_logic = enemy.GetComponent<Enemy>();
        enemy_logic.Obj_Mananger = Obj_Manager;
        enemy_logic.Player_ts= player.transform; 

        enemy.transform.position = Enemy_Spawnpoint[ranPoint].position;
    }


    public void Action(GameObject scanobj)
    {
        ObjData data = scanobj.GetComponent<ObjData>();

        if(data.id ==1000)
        {
            Sword = 1;
        }
        else if(data.id ==12)
        {
            Rod = 1;
        }
            rewardTarget = scanobj;

        TalkPanel.SetActive(true);
        
     
        Scanobj = scanobj;
     // 그 감지한 오브젝트의 objdata를 가져오자
     Talk(data.id, data.isNpc);
        

    }
    public void GetItem(Item item)
    {
        inventory.AddItem(item);
    }

    void Talk(int id ,bool isNpc)
    {
        string talkData = talkManager.GetTalk(id,talkindex);

        if(talkData == null)
        {
            isAction = false;
            talkindex = 0;


            TalkPanel.SetActive(false);

            if (!isNpc)
            {
                Pickup();
               
            }

            return;
        }


        if(isNpc)
        {
           
            talkText.text=talkData.Split(':')[0];
        }

        else
        {
            talkText.text = talkData;
        }

        isAction = true;
        talkindex++;
    }



    void Pickup()
    {
        if (rewardTarget == null)
        {
            return;
        }

        Item itemCheck =rewardTarget.GetComponent<Item>();

        if(itemCheck != null)
        {
            inventory.AddItem(itemCheck);
            rewardTarget.SetActive(false);
           //Destroy(rewardTarget); //이게 파괴되서 제대로 참조가안됨
        }

        rewardTarget = null;
    }

    public void OnslotClick(SlotData slot)
    {
        currentslot = slot;
        // 아이템 없으면 안열고
        if (slot == null || slot.item == null)
        {
            Debug.Log("slot.item: " + slot.item);
            Debug.Log("item_type: " + slot.item.item_type);
            equipui.SetActive(false);
            return;
        }

        if (slot.item.item_type == 3)
        {
           
            equipui.SetActive(!equipui.activeSelf);
            
        }

        else
        {
           
            equipui.SetActive(false);
        }
    }



    public void Equipped_tool()
    {
       

        if(!player.isEquipped)
        {
            player.isEquipped = true;
            
            if(currentslot.item.item_Name=="Sword")
            {
                player.getSword = true;
                player.getRod = false;
                Debug.Log("검 장착됨");
            }

            else if(currentslot.item.item_Name == "Rod")
            {
                player.getSword = false;
                player.getRod = true;
            }
                equipButtonText.text = "해제하기";
        }

        
        else
        {
            player.isEquipped = false;
            player.getSword = false;

            equipButtonText.text = "장착하기";
            Debug.Log("무기 해제됨");
        }

        equipui.SetActive(false);
    }


    public void submenuactive()
    {
        if (menuset.activeSelf)
            menuset.SetActive(false);

        else
            menuset.SetActive(true);
    }



    private void HandleHP()
    {
      
        hpbar.value= Mathf.Lerp(hpbar.value, PlayerHp.current_hp / PlayerHp.max_hp, Time.deltaTime*10); //선형보간
    }


    public void Save()
    {
        SavePlayer();
        inventory.SaveInventory();
    }


    public void SavePlayer()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("Equip", player.isEquipped ? 1 : 0);
        PlayerPrefs.SetInt("getSword", player.getSword ? 1 : 0);
        PlayerPrefs.SetInt("getRod", player.getRod ? 1 : 0);
        PlayerPrefs.SetInt("Sword", Sword);
        PlayerPrefs.SetInt("Rod",Rod);

        PlayerPrefs.Save();
    }


    public void PlayerLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        
        int s = PlayerPrefs.GetInt("Sword");
        int r = PlayerPrefs.GetInt("Rod");
        player.transform.position = new Vector3(x, y, 0);
        player.getRod = (PlayerPrefs.GetInt("getRod", 0) == 1);
        player.getSword = (PlayerPrefs.GetInt("getSword", 0) == 1);
        player.isEquipped = (PlayerPrefs.GetInt("Equip", 0) == 1);
        Sword = s;
        Rod = r;

        
    }
    public void Continue()
    {
        menuset.SetActive(false);
    }

    public void GameExit()
    {
        UnityEngine.Application.Quit();
    }
   
    public void Respawn()
    {
        player.transform.position = spawnpoint.position;

        PlayerHp.current_hp = PlayerHp.max_hp;
    }


    public void newgame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

