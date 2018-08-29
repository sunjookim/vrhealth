using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 텍스트 표시, 버튼 비활성화에 사용
using UnityEngine.SceneManagement; // 씬 전환에 사용

public class GameUI: MonoBehaviour
{
    public float _time = 0.0f; // 타이머 변수 초기화

    public Text _timerText; // 진행 시간 표시될 텍스트
    public Text txtScore;
    public Text txtBestScore;
    public Text txtCorner;
    public Text txtGameOverMs;

    //public float done = 13.0f; // 컨트롤러 초기화 영상 재생될 시간

    // Use this for initialization
    void Start()
    {
        txtGameOverMs.text = "";
        //PlayerPrefs.SetFloat("BestScore", 300f);
        float bestTime = PlayerPrefs.GetFloat("BestScore");
        txtBestScore.text = "<color=#00ff00ff>Best time : </color> <color=#ff0000>" +
        ((int)bestTime / 60) + " : " + ((int)bestTime % 60) + " : " + (int)((bestTime % 1) * 100) + "</color>";
    }

    public void ChangeCorner(int Corner)
    {
        int toCorner = Corner;

        txtCorner.text = "<color=#00ff00ff>현재 남은 Corner</color> <color=#ff0000>" +
            toCorner.ToString() + "</color>";
    }

    public void ChangeScore(float time)
    {
        txtScore.text = "<color=#00ff00ff>Current time : </color> <color=#ff0000>" +
            ((int)time/60) + " : " + ((int)time%60) + " : " + (int)((time % 1) * 100) + "</color>";
    }

    public void ChangeBest()
    {
        if (_time < PlayerPrefs.GetFloat("BestScore"))
        {
            PlayerPrefs.SetFloat("BestScore", _time);
            PlayerPrefs.Save();
            txtBestScore.text = "<color=#00ff00ff>Best time : </color> <color=#ff0000>" +
            ((int)_time / 60) + " : " + ((int)_time % 60) + " : " + (int)((_time % 1) * 100) + "</color>";
        }
    }

    public void ChangeGameOver(int mode)
    {
        if(mode == 0)   // 라이프가 없음
        {
            txtGameOverMs.text = "목숨을 모두 소진 하셨습니다 ㅜㅜ";
        }
        else if(mode == 1) // 맵을 다 돌음
        {
            txtGameOverMs.text = "맵을 다 돌으셨어요!";
        }
    }

    void Update()
    {
        // 진행 시간 표시
        _time += Time.deltaTime; // 초
        int minute = (int)_time / 60;
        _timerText.text = minute.ToString();
        ChangeScore(_time);
    }
}
