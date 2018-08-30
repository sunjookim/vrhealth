using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 버튼 비활성화 기능 사용하기 위함
using UnityEngine.Video; // 초기화 영상 제어


/* 씬 번호
 * 0 : 메인
 * 1 : 게임 선택 메뉴
 * 2 : 매뉴얼
 * 3 : 외줄타기 게임 설명
 * 4 : 외줄타기 게임
 * 5 : 성공화면
 * 6 : 실패화면
 * 7 : 물로켓 게임
 * 8 : 썰매 게임
 */


public class move_scene : MonoBehaviour {

    public AudioClip sndPause; // 일시정지 버튼 효과음
    public AudioClip sndPauseButton; // 일시정지 메뉴 내의 버튼 효과음

    public Button PauseButton; // 일시정지 버튼
    //int isInitializing = 0; // 초기화 중인지 검사
    //public float done = 20f; // 컨트롤러 초기화 영상 재생될 시간
    public float _time = 0.0f; // 타이머 변수 초기화
    

    // 메인 화면
    public void SceneChange1()
    {
        SceneManager.LoadScene("Main");
    }


    // 게임 선택 화면으로 이동
    public void SceneChange2()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SceneToManual()
    {
        SceneManager.LoadScene("manual");
    }

    public void SceneToGame1()
    {
        SceneManager.LoadScene("썰매 게임");
    }

    // 외줄타기 허리운동게임 선택 버튼(게임 방법부터 보기)
    public void SceneToGame2()
    {
        SceneManager.LoadScene("howto_game_2");
    }

    // 외줄타기 허리운동게임 선택 버튼(게임 시작부터 하기)
    public void SceneToGame2_Play()
    {
        SceneManager.LoadScene("game_2");
    }

    public void SceneToGame3()
    {
        SceneManager.LoadScene("WaterRocket");
    }

    // 종료 버튼 기능
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터 상에서 동작
#else
        Application.Quit(); // pc 및 모바일에서 동작
#endif
    }


    public void Pause()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            GetComponent<AudioSource>().Pause(); // 배경음 일시정지
        }
        

        AudioSource.PlayClipAtPoint(sndPause, transform.position); // 효과음 출력

        /*
        if (null != GameObject.FindWithTag("Initialize"))
        {
            isInitializing = 1; // 초기화 중이었다
            Destroy(GameObject.FindWithTag("Initialize")); // 초기화 영상 삭제
        }
        */

        // 일시정지 버튼 비활성화
        PauseButton.interactable = false;

        // 화면을 어둡게 한다
        Light light = GameObject.FindWithTag("Light").GetComponent<Light>();
        light.intensity = -3; // -1로 할 수록 밝아짐

        // 화면을 정지시킨다 (초기화 영상 나오고 있는 경우는 적용되지 않음)
        Time.timeScale = 0; // 일시정지

        if(SceneManager.GetActiveScene().buildIndex == 8) // 썰매 게임
        {
            GameObject.Find("Game_UI").transform.Find("pauseMenu").gameObject.SetActive(true);
        } else if(SceneManager.GetActiveScene().buildIndex == 4) // 외줄타기 게임
        {
            GameObject.Find("movingPerson").transform.Find("PauseMenu").gameObject.SetActive(true);
        }

    } 

    public void pause_GoOn()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            GetComponent<AudioSource>().UnPause();
        }
            

        /*
        if (isInitializing == 1) // 초기화 중이었다면
        {
            GameObject obj = Instantiate(Resources.Load("Initializing")) as GameObject; // 초기화 영상 다시 불러오기

            if (done > 0f)
            {
                done -= Time.deltaTime;
            }
            else
            {
                Destroy(GameObject.FindWithTag("Initialize"));
            }
        }
        */

        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            GameObject.Find("Game_UI").transform.Find("pauseMenu").gameObject.SetActive(false);
        } else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            GameObject.Find("movingPerson").transform.Find("PauseMenu").gameObject.SetActive(false);
        }
            

        PauseButton.interactable = true;
        Light light = GameObject.FindWithTag("Light").GetComponent<Light>();


        // 라이트 다시 밝게 함
        if(SceneManager.GetActiveScene().buildIndex == 8)
        {
            light.intensity = +0.5f;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 4)
        {

            light.intensity = +1;
        }
        
        Time.timeScale = 1;

        AudioSource.PlayClipAtPoint(sndPauseButton, transform.position);
    }
}
