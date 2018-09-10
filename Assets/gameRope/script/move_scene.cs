using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 버튼 비활성화 기능 사용하기 위함
using UnityEngine.Video; // 초기화 영상 제어


/* 씬 번호
 * 0 : 메인(Main)
 * 1 : 게임 선택 메뉴(Menu)
 * 2 : 매뉴얼(manual)
 * 3 : 외줄타기 게임(game_2)
 * 4 : 성공화면(Success)
 * 5 : 실패화면(Fail)
 * 6 : 물로켓 게임(WaterRocket)
 * 7 : 썰매 게임(썰매 게임)
 */


public class move_scene : MonoBehaviour {

    public AudioClip sndPause; // 일시정지 버튼 효과음
    public AudioClip sndPauseButton; // 일시정지 메뉴 내의 버튼 효과음

    public Button PauseButton; // 일시정지 버튼
    //int isInitializing = 0; // 초기화 중인지 검사
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

    // 외줄타기 허리운동게임 선택 버튼(게임 시작부터 하기)
    public void SceneToGame2_Play()
    {
        SceneManager.LoadScene("game_2");
    }

    public void SceneToGame3()
    {
        SceneManager.LoadScene("WaterRocket");
    }

    // 재시작 버튼
    public void ReStart()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3) // 외줄타기
        {

        }
        else if(SceneManager.GetActiveScene().buildIndex == 6) // 물로켓
        {

        }
        else if(SceneManager.GetActiveScene().buildIndex == 7) // 썰매
        {

        }
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
        //Light light = GameObject.FindWithTag("Light").GetComponent<Light>();
        //light.intensity = -3; // -1로 할 수록 밝아짐


        // 화면을 정지시킨다 (초기화 영상 나오고 있는 경우는 적용되지 않음)
        //Time.timeScale = 0; // 일시정지


        if(SceneManager.GetActiveScene().buildIndex == 7) // 썰매 게임
        {
            GameObject.Find("Game_UI").transform.Find("PauseMenu").gameObject.SetActive(true);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3) // 외줄타기 게임
        {
            GetComponent<AudioSource>().Pause(); // 배경음 일시정지

            AudioSource.PlayClipAtPoint(sndPause, transform.position); // 효과음 출력

            GameObject.Find("movingPerson").transform.Find("PauseMenu").gameObject.SetActive(true);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 6) // 물로켓 게임
        {
            GameObject.Find("GameUI").transform.Find("PauseMenu").gameObject.SetActive(true);
        }

    } 

    // 메뉴닫기 버튼
    public void pause_GoOn()
    {

        GetComponent<AudioSource>().UnPause(); // 배경음 다시 재생

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

        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            GameObject.Find("Game_UI").transform.Find("pauseMenu").gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3) // 외줄타기 게임
        {

            GameObject.Find("movingPerson").transform.Find("PauseMenu").gameObject.SetActive(false);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 6)
        {
            GameObject.Find("GameUI").transform.Find("PauseMenu").gameObject.SetActive(false);
        }
            
        /*
        PauseButton.interactable = true;
        Light light = GameObject.FindWithTag("Light").GetComponent<Light>();


        // 라이트 다시 밝게 함
        if(SceneManager.GetActiveScene().buildIndex == 7)
        {
            light.intensity = +0.5f;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 6)
        {

            light.intensity = +1;
        } 
        */

        //Time.timeScale = 1;
        
        AudioSource.PlayClipAtPoint(sndPauseButton, transform.position);
    }
}
