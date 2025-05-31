using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;



    private void Awake()
    {
        
        talkData= new Dictionary<int, string[]>();
        GenerateTalk();
    }
   

    void GenerateTalk()
    {
        talkData.Add(1, new string[] { "���� : ������  ���� : �Ʒ���"});
        talkData.Add(12, new string[] { "���ô밡 �������ִ�.", "���ô븦 ȹ���Ͽ����ϴ�." });
        talkData.Add(1000, new string[] { "�氡�� ���� �������ִ�.", "���� ȹ���Ͽ����ϴ�." });
        talkData.Add(2000, new string[] {"�ʴ� ó�� ���� ����ε�, ��� �Ծ�?", "�ϴ� �츮������ �°� ȯ����" });
        talkData.Add(15, new string[] { "�ȳ�, �ݰ���", "���̸��� �糪�����" });
    }


    public string GetTalk(int id ,int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
