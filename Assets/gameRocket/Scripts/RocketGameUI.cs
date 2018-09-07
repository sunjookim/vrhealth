using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 캔버스 제어

public class RocketGameUI : MonoBehaviour {

    public Text timerText; // 진행 시간 표시될 텍스트

    // 추가 부분
    public Text GameMsText; // 게임 메시지 텍스트

    public float time = 0.0f; // 타이머 변수 초기화

    void Start()
    {
        // 초기화 영상 재생
        //GameObject obj = Instantiate(Resources.Load("InitializingRocket")) as GameObject;

        /** 추가된 부분***/
        GameMsText.text = "";
    }

    /** 추가된 부분***/
    public void ChangeMs(float time)
    {
        if (time > 15f && time <= 17f) // 성공 메시지
        {
            GameMsText.text = "성공!";
            Invoke("txtClear", 2f); // 2초 후에 이 함수를 실행함
        }
        else if(time <= 15f && time > 5f) // 아무런 메시지가 없음
        {
            GameMsText.text = "";
        }
        else if(time <= 5f && time >= 1f) // 5초 이하일 때 카운트 다운하여 경고를 함
        {
            GameMsText.text = "제한 시간 " + (int)time + "초 남았습니다.";
        }
        else if(time < 1f) // 실패 메시지
        {
            GameMsText.text = "실패!";
            Invoke("txtClear", 2f);
        }
    }

    // 게임 메시지 초기화하는 함수
    void txtClear()
    {
        GameMsText.text = "";
    }


    /*** 마침 *****/

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
