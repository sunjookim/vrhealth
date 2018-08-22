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

    private int totScore = 0;
    //public float done = 13.0f; // 컨트롤러 초기화 영상 재생될 시간

    // Use this for initialization
    void Start()
    {
        txtScore.text = "<color=#00ff00ff>Score</color> <color=#ff0000>0</color>";
        txtBestScore.text = "<color=#00ff00ff>BestScore</color> <color=#ff0000>" +
            PlayerPrefs.GetInt("BestScore", 0).ToString() + "</color>";
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
            ((int)time/60) + " : " + ((int)time%60) + " : "  + "</color>";

        if (time < PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetFloat("BestScore", time);

            txtBestScore.text = "<color=#00ff00ff>BestScore</color> <color=#ff0000>" +
                ((int)time / 60) + " : " + ((int)time % 60) + " : " + "</color>" + "</color>";
        }
    }

    public void ChangeLife()
    {
        // 라이프를 깎는 스크립트 넣음
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
