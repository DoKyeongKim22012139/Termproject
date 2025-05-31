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
        talkData.Add(1, new string[] { "마을 : 오른쪽  동굴 : 아래쪽"});
        talkData.Add(12, new string[] { "낚시대가 떨어져있다.", "낚시대를 획득하였습니다." });
        talkData.Add(1000, new string[] { "길가에 검이 떨어져있다.", "검을 획득하였습니다." });
        talkData.Add(2000, new string[] {"너는 처음 보는 사람인데, 어디서 왔어?", "일단 우리마을에 온걸 환영해" });
        talkData.Add(15, new string[] { "안녕, 반가워", "내이름은 루나라고해" });
    }


    public string GetTalk(int id ,int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
