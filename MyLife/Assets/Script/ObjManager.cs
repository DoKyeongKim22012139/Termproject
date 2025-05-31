using NUnit.Framework.Constraints;
using System.CodeDom.Compiler;
using System.Collections;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public GameObject Enemy_potion_prefab;
    public GameObject Skeleton_ashes_prefab;
    public GameObject Skeleton_prefab;

    public int Enemy_now_Spawn = 0;

    GameObject[] Enemy_potion;
    GameObject[] Skeleton_ashes;
    GameObject[] Skeleton;
    GameObject[] targetPool;



    private void Awake()
    {
        Enemy_potion = new GameObject[10];
        Skeleton_ashes = new GameObject[10];
        Skeleton = new GameObject[10];

        Generate(); //생산
    }




    void Generate()
    {

        for (int i=0;i<Enemy_potion.Length;i++)
        {
            Enemy_potion[i]=Instantiate(Enemy_potion_prefab);
            Enemy_potion[i].SetActive(false);
        }

        for (int i = 0; i < Skeleton_ashes.Length; i++)
        {
            Skeleton_ashes[i] = Instantiate(Skeleton_ashes_prefab);
            Skeleton_ashes[i].SetActive(false);
        }

        for (int i = 0; i < Skeleton.Length; i++)
        {
            Skeleton[i] = Instantiate(Skeleton_prefab);
            Skeleton[i].SetActive(false);
        }
    }


    public GameObject MakeObj(string  name)
    {
        switch (name)
        {
            case "Enemy_potion":
                targetPool = Enemy_potion;
                break;

            case "Skeleton_ashes":
                targetPool = Skeleton_ashes;
                break;
            case "Skeleton":
                targetPool = Skeleton;
                break;
        }

        for(int i=0; i<targetPool.Length;i++)
        {
            if (!targetPool[i].gameObject.activeSelf) // 상태가 false면 전환 후 반환
            {
                targetPool[i].gameObject.SetActive(true);

                if (name == "Skeleton")
                    Enemy_now_Spawn++;
                return targetPool[i]; 
            }
        }

        return null;
    }


    public GameObject[] GetPool(string name)
    {
        switch (name)
        {
            case "Enemy_potion":
                targetPool = Enemy_potion;
                break;

            case "Skeleton_ashes":
                targetPool = Skeleton_ashes;
                break;

            case "Skeleton":
                targetPool = Skeleton;
                break;

        }

        return targetPool;
    }
}
