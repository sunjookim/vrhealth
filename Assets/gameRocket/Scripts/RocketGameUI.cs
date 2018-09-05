using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 캔버스 제어

public class RocketGameUI : MonoBehaviour {

    public Text timerText; // 진행 시간 표시될 텍스트

    public float time = 0.0f; // 타이머 변수 초기화

    void Start()
    {
        // 초기화 영상 재생
        GameObject obj = Instantiate(Resources.Load("InitializingRocket")) as GameObject;
    }

    // Update is called once per frame
    void Update () {
        // 진행 시간 표시
        time += Time.deltaTime;
        int minute = (int)time / 60;
        timerText.text = (minute.ToString());

        // 컨초_허리 영상 끝나는 시간이 되면 영상 삭제하기
        if(time >= 19)
        {
            Destroy(GameObject.FindWithTag("Initialize")); // 컨초_허리 영상 제거하기
        }
    }

}
