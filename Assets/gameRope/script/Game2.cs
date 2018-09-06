﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 캔버스 제어
using UnityEngine.SceneManagement; // 씬 전환
//using UnityEngine.AI; // 네비메쉬

namespace Ardunity
{

    public class Game2 : MonoBehaviour
    {
        Animator animator1; // 플레이어 애니메이터
        Animator animator2; // teacher 애니메이터

        //public NavMeshAgent nvAgent; // 네비메쉬 에이전트(플레이어)

        public Transform playerNav;
        public Transform destination3; // 목적지


        public float _time = 0.0f; // 타이머 변수 초기화
        public float slopeGoodtime = 0.0f; // 타이머 변수 초기화 (각 시간 구간 내에서 올바른 동작의 시간을 재는 변수)
        public float slopeArrowTime = 0.0f; // teacher 캐릭터에 화살표 깜박임에 사용됨
        public float CountDown = 6.0f; // 기울이기 동작 카운트 다운 숫자 (시작 전)
        public float CountDownDo = 6.3f; // 기울이기 동작 카운트 다운 숫자 (시작 후)
        public float fail_color_Time = 0.0f; // 플레이어 캐릭터 깜박임에 사용됨
        public float SlopeGoTime = 4.0f; // 각 slope에서 기울여 내려가는 애니메이션의 시간을 재기 위한 변수
        public float TwincleTime = 2.0f; // 실패했을 때 캐릭터 깜박이는 시간
        public float PlayerMove = 0.0f; // 플레이어와 카메라 이동 속도 계산에 사용되는 변수

        public Text _timerText; // 진행 시간 표시될 텍스트
        public Text upText; // 디버깅용
        public Text txtSound; // 음성 안내 자막
        public Text txtSoundCountDown; // 기울이기 동작 카운트 다운 출력 (시작 전)
        public Text txtSoundCountDownDo; // 기울이기 동작 카운트 다운 출력 (시작 후)
        public Text txtLifeReason; // 별 깎인 이유 출력

        
        bool IsCountDown = false; // 기울이기 동작 카운트 다운이 진행되고 있는지의 여부
        bool IsCountDownDo = false; // 기울이기 동작 남은 시간을 보여줄지의 여부
        bool IsSlopeArrowLeft = false;
        bool IsSlopeArrowRight = false;
        bool leftGood = false; // 사용자가 왼쪽 동작을 올바르게 했는가?
        bool rightGood = false; // 사용자가 오른쪽 동작을 올바르게 했는가?
        bool isFail = false; // 실패를 해서 플레이어 캐릭터를 깜박이게 해야하는가?
        bool isDeath = false; // 실패를 해서 별을 깎아야 하는가?
        bool AudioPlay = false; // 오디오 출력을 한 번만 하게 하기 위한 변수
        bool isSlope = false;
        bool isSlopeGoTime = false; // 각 slope에서 기울여 내려가는 애니메이션의 시간을 재기 위한 변수
        bool isLeftSlope = false; // 왼쪽 slope인지?
        bool isRightSlope = false; // 오른쪽 slope인지?
        bool isNotSlope = true; // slope 바깥에 있는지?
        

        public GameObject slopeArrowLeft; // teacher 캐릭터에 화살표 출력
        public GameObject slopeArrowRight; // teacher 캐릭터에 화살표 출력
        
        
        int countdowntime; // 운동 동작 중 카운트 다운 숫자
        int Life = 4; // 목숨의 개수
        int PlayerSpeed = 4; // 플레이어 걸어가는 속도


        float speed = 0.68f; // 플레이어 상태바가 움직이는 속도              
        float turnspeed = 2f; // 마우스 커서로 회전하는 속도


        Vector3 v3; // 마우스 커서 회전에 사용되는 변수


        public AudioClip sndLeft; // 왼쪽으로 기울기 안내 음성
        public AudioClip sndRight; // 오른쪽으로 기울기 안내 음성
        public AudioClip sndLeftPrev; // 왼쪽으로 기울기 안내 음성 (시작 전)
        public AudioClip sndRightPrev; // 오른쪽으로 기울기 안내 음성 (시작 후)


        public Projector fail_color; // 실패 시 플레이어를 깜박이게 하기 위한 프로젝터 컴포넌트


        //int playerSlope = 0; // 플레이어가 기울인 횟수(up 키보드 누른 횟수)
        //int playerSlopeGood = 1; // 플레이어가 기울인 횟수와 비교할 올바른 누름 횟수
        //public float CountDownDo = 8.0f; // 기울이기 동작 카운트 다운 숫자 (시작 후)
        //int bird_speed = 100; // 새가 날아가는 속도
        //bool Bird_01 = false; // bird_01 움직이기 시작하는가?



        void Start()
        {
            // 실패 시 깜박임 처리하기 위해 프로젝터를 사용하였고 이 컴포넌트를 변수에 저장
            fail_color = GameObject.FindWithTag("fail_color").GetComponent<Projector>();

            // 처음 시작할 땐 화살표들 안보이게 처리
            slopeArrowLeft.SetActive(false);
            slopeArrowRight.SetActive(false);

            Time.timeScale = 1; // 일시정지 버튼 - 재시작 버튼을 눌렀을 때 time이 멈추는 현상 방지

            // 초기화 영상 재생
            //GameObject obj = Instantiate(Resources.Load("Initializing")) as GameObject; 

            // 애니메이터 설정
            animator1 = GameObject.Find("unitychan").GetComponent<Animator>();
            animator2 = GameObject.Find("teacher").GetComponent<Animator>();
            
        }
        

        // 외줄 위를 이동하다가 정지, 기울이기 동작 처리에 관한 부분
        void OnTriggerEnter(Collider other)
        {
            // 첫 번째 기울이기
            if (other.gameObject.tag == "slope_01")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }


                IsCountDown = false; // (UI)카운트 다운 중이 아니다

                ///*
                //nvAgent.velocity = Vector3.zero; // 네비메쉬
                //nvAgent.Stop(true);
                //*/

                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_02")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
                }
                  

                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                animator1.SetBool("IsSlopeRight", true);
                animator2.SetBool("IsSlopeRight", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_03")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }


                IsCountDown = false; // (UI)카운트 다운 중이 아니다
                

                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

            }
            else if (other.gameObject.tag == "slope_04")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
                }


                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                animator1.SetBool("IsSlopeRight", true);
                animator2.SetBool("IsSlopeRight", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

            }
            else if (other.gameObject.tag == "slope_05")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }


                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

            }
            else if (other.gameObject.tag == "slope_06")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
                }


                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                animator1.SetBool("IsSlopeRight", true);
                animator2.SetBool("IsSlopeRight", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_07")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }


                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_08")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
                }


                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                animator1.SetBool("IsSlopeRight", true);
                animator2.SetBool("IsSlopeRight", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_09")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }


                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림
            }
            else if (other.gameObject.tag == "slope_10")
            {
                isNotSlope = false; // 슬로프에 있다.

                PlayerSpeed = 0; // 플레이어 정지

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작

                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
                }


                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다


                animator1.SetBool("IsSlopeRight", true);
                animator2.SetBool("IsSlopeRight", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림
            }

            if (other.gameObject.tag == "sound_01")
            {
                TwincleTime = 2.0f; // 깜박임 시간 초기화
                AudioSource.PlayClipAtPoint(sndLeft, transform.position); // 안내음성 출력
                txtSound.text = "왼쪽 방향 허리 운동이 곧 시작됩니다.";
                IsCountDown = true; // 카운트 다운 중이다

            }
            else if (other.gameObject.tag == "sound_02")
            {
                TwincleTime = 2.0f; // 깜박임 시간 초기화
                AudioSource.PlayClipAtPoint(sndRight, transform.position);
                txtSound.text = "오른쪽 방향 허리 운동이 곧 시작됩니다.";
                IsCountDown = true;

            }

            // 회전 처리
            if(other.gameObject.tag == "rotation_01") // 첫 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, -89.8f, 0); // 플레이어 회전

                //GameObject.FindWithTag("MainCamera").transform.rotation = Quaternion.Euler(0, 0, 10); // 플레이어 캐릭터만 회전하고, 카메라는 회전하지 않음으로서 오큘러스 착용자가 직접 시선을 회전해야 함
                

            } else if(other.gameObject.tag == "rotation_02") // 두 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, -65.5f, 0); // 플레이어 회전
            } else if(other.gameObject.tag == "rotation_03") // 세 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, -94f, 0); // 플레이어 회전
            } else if(other.gameObject.tag == "rotation_04") // 네 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, 1.3f, 0); // 플레이어 회전
            }


            // 끝까지 도달하면 게임을 성공적으로 종료
            if (other.gameObject.tag == "finishLine")
            {
                SceneManager.LoadScene("Success");
            }
        }

        // 라이프 감소 처리에 관한 부분
        void OnTriggerExit(Collider other)
        {
            // 음성 안내 나올 때 자막 출력
            if ((other.gameObject.tag == "slope_01") || (other.gameObject.tag == "slope_02") || (other.gameObject.tag == "slope_03") || (other.gameObject.tag == "slope_04") || (other.gameObject.tag == "slope_05") || (other.gameObject.tag == "slope_06") || (other.gameObject.tag == "slope_07") || (other.gameObject.tag == "slope_08") || (other.gameObject.tag == "slope_09") || (other.gameObject.tag == "slope_10"))
            {
                if(isDeath == true)
                {
                    // 별 제거
                    
                    if (animator1.GetBool("slopeFail") == true)
                    {
                        Destroy(GameObject.FindWithTag("Life" + Life));
                        Life -= 1;

                        isFail = true; // 캐릭터 깜박이기 시작
                    }
                    
                }

                // 안내문 초기화
                txtLifeReason.text = " ";
                txtSound.text = " ";

                IsSlopeArrowLeft = false;
                IsSlopeArrowRight = false;

                // teacher 캐릭터의 화살표 모두 비활성화
                slopeArrowLeft.SetActive(false);
                slopeArrowRight.SetActive(false);

                IsCountDownDo = false;

                // 이전 slope에서 실패를 했더라도 다음 slope에 영향을 미치지 않게 exit할 때 초기화를 해준다
                animator1.SetBool("slopeFail", false); 
                animator1.SetBool("IsSlopeLeft", false);
                animator1.SetBool("IsSlopeRight", false);
                animator2.SetBool("IsSlopeLeft", false);
                animator2.SetBool("IsSlopeRight", false);
                animator1.SetBool("LeftBack", false);
                animator1.SetBool("RightBack", false);

                isDeath = false;

                isSlopeGoTime = false;
                SlopeGoTime = 4.0f;
                isLeftSlope = false;
                isRightSlope = false;
                slopeGoodtime = 0.0f;
                isNotSlope = true;
                AudioPlay = false;

            }

            /*
            // 기울이는 구간에서 사용자의 up 입력이 없었다면 life를 감소시킴
            if (other.gameObject.tag == "slope_01")
            {
                if (!(playerSlope >= playerSlopeGood))
                {
                    Destroy(GameObject.FindWithTag("Life" + Life));
                    Life -= 1;
                    playerSlopeGood = playerSlope;
                }
                else
                {
                    playerSlopeGood++;
                }
            }
            else if (other.gameObject.tag == "slope_02")
            {
                if (!(playerSlope >= playerSlopeGood + 1))
                {
                    Destroy(GameObject.FindWithTag("Life" + Life));
                    Life -= 1;
                    playerSlopeGood = playerSlope;
                }
                else
                {
                    playerSlopeGood++;
                }
            }
            */
        }

        void Update()
        {
            
            // 키보드로 테스트
            /*
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                leftGood = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                rightGood = true;
            }
            */
            
            // 마우스 커서로 시선 회전
            v3 = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            GameObject.FindWithTag("MainCamera").transform.Rotate(v3 * turnspeed);

            distinctionB BBB = GameObject.Find("movingPerson").GetComponent<distinctionB>(); // 1번

            // Arduinity
            // 다른 스크립트에서 아두이노 컨트롤러 값을 받아온다
            distinctionB Controller = GameObject.Find("movingPerson").GetComponent<distinctionB>();

            // Arduinity
            // bool 변수에 컨트롤러 값들을 저장(trigger 함수에서 사용하기 위함)
            leftGood = Controller.leftback;
            rightGood = Controller.rightback;



            // true/false 값 출력하기
            print(Controller.leftback);
            print(Controller.rightback);
            print("slopeGoodtime: " + slopeGoodtime);
            print("isSlope: " + isSlope);
            print("SlopeGoTime: " + SlopeGoTime);


            // 진행 시간 표시
            _time += Time.deltaTime;
            int minute = (int)_time / 60;
            _timerText.text = (minute.ToString());


            /*
            // 컨초_허리 영상 끝나면 캐릭터 출발시키고 이동하기
            if(BBB.count2 >= 300)
            {
                Destroy(GameObject.FindWithTag("Initialize")); // 컨초_허리 영상 제거하기

                PlayerMove = Time.deltaTime * PlayerSpeed;
                transform.Translate(Vector3.forward * PlayerMove); // 플레이어 이동

                // 상태바 이동
                float fMove2 = Time.deltaTime * speed;
                GameObject.FindGameObjectWithTag("barPlayer").transform.Translate(Vector3.right * fMove2);
            
            }
            */

            PlayerMove = Time.deltaTime * PlayerSpeed;
            transform.Translate(Vector3.forward * PlayerMove); // 플레이어 이동

            // 상태바 이동
            float fMove2 = Time.deltaTime * speed;
            GameObject.FindGameObjectWithTag("barPlayer").transform.Translate(Vector3.right * fMove2);



            if (isLeftSlope == true) //왼쪽 슬로프에 도달
            {
                // 올바른 왼쪽 동작 값이 들어오면 카운트를 시작하고, 올바르지 않은 값이 들어오면 안내문 출력 및 타이머를 초기화한다.
                if (leftGood == true) // 올바른 왼쪽 동작을 하면
                {
                    slopeGoodtime += Time.deltaTime; // 카운트 시작
                }
                else
                {
                    slopeGoodtime = 0.0f; // 올바른 왼쪽 동작이 나오지 않으면 바로 타이머 초기화

                    txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
                }
            }

            if(isRightSlope == true) // 오른쪽 슬로프에 도달
            {
                // 올바른 오른쪽 동작 값이 들어오면 카운트를 시작하고, 올바르지 않은 값이 들어오면 안내문 출력 및 타이머를 초기화한다.
                if (rightGood == true) // 올바른 왼쪽 동작을 하면
                {
                    slopeGoodtime += Time.deltaTime; // 카운트 시작
                }
                else
                {
                    slopeGoodtime = 0.0f; // 올바른 왼쪽 동작이 나오지 않으면 바로 타이머 초기화

                    txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
                }
            }


            if(isSlopeGoTime == true) // 기울여 내려가는 애니메이션 카운트 감소 시작
            {
                SlopeGoTime -= Time.deltaTime;
            }


            // 기울여 내려가는 애니메이션이 끝났을 때 성공/실패여부를 검사
            // ((int)SlopeGoTime == 0 && isNotSlope == false) 
             if((int)SlopeGoTime == 0)
            {
                if(slopeGoodtime >= 2.0f) // 올바른 동작 3초를 채웠다면
                {
                    animator1.SetBool("slopeFail", false); // 시간 구간 내에서 true가 되므로, 빠져나갈 때 바꿔줘야 함

                    if(isLeftSlope == true) // 왼쪽 기울이기 동작 구간이라면
                    {
                        animator1.SetBool("LeftBack", true); // 성공 동작
                    }
                    else if(isRightSlope == true) // 오른쪽 기울이기 동작 구간이라면
                    {
                        animator1.SetBool("RightBack", true); // 성공 동작
                    }
                    
                    isDeath = false;
                } else
                {
                    animator1.SetBool("slopeFail", true); // 실패 동작

                    isDeath = true;

                    txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력
                }
            } else if(SlopeGoTime <= -1.5f)
            {
                PlayerSpeed = 4;
            }


            // 깜박깜박
            // 플레이어 캐릭터 깜박임 구현
            if(isFail == true)
            {
                TwincleTime -= Time.deltaTime; // 총 깜박일 시간 2초를 카운트 다운 시작

                fail_color_Time += Time.deltaTime; // 시간으로 깜박임 구현

                if (((int)(fail_color_Time * 10)) % 10 == 6)
                {
                    fail_color.enabled = false;
                }
                else if (((int)(fail_color_Time * 10)) % 10 == 0)
                {
                    fail_color.enabled = true;
                }
            } else
            {
                fail_color.enabled = false;
            }

            if(TwincleTime <= 0) // 2초가 지나면 깜박임 중지
            {
                isFail = false;
            }




            // teacher 캐릭터에 화살표 출력
            if (IsSlopeArrowLeft == true) // 왼쪽 동작이라면
            {
                slopeArrowLeft.SetActive(true); // teacher 캐릭터에 화살표 출력

                slopeArrowTime += Time.deltaTime; // 시간으로 깜박임 구현

                // 화살표 깜박깜박임 처리
                if (((int)(slopeArrowTime * 10)) % 10 == 6)
                {
                    slopeArrowLeft.SetActive(false);
                }
                else if (((int)(slopeArrowTime * 10)) % 10 == 0)
                {
                    slopeArrowLeft.SetActive(true);
                }
            }
            else if (IsSlopeArrowRight == true) // 오른쪽 동작이라면
            {
                slopeArrowRight.SetActive(true); // teacher 캐릭터에 화살표 출력

                slopeArrowTime += Time.deltaTime;


                if (((int)(slopeArrowTime * 10)) % 10 == 6)
                {
                    slopeArrowRight.SetActive(false);
                }
                else if (((int)(slopeArrowTime * 10)) % 10 == 0)
                {
                    slopeArrowRight.SetActive(true);
                }
            }

            // 화면에 동작 전 몇 초 남았다고 카운트 다운 알려주는 글씨
            if (IsCountDown == true)
            {
                CountDown -= Time.deltaTime;
                countdowntime = (int)CountDown;
                txtSoundCountDown.text = (countdowntime.ToString() + "초 후 운동 시작");
            }
            else
            {
                txtSoundCountDown.text = " ";
                CountDown = 6;
            }

            // (운동 중) 카운트 다운 시간이 끝나면 초기화
            if(countdowntime == 0)
            {
                IsCountDownDo = false;
            }

            // 동작 중일 때 몇 초 남았다고 카운트 다운 알려주는 글씨
            if (IsCountDownDo == true)
            {
                CountDownDo -= Time.deltaTime;
                countdowntime = (int)CountDownDo;

                if(countdowntime <= 5)
                {
                    txtSoundCountDownDo.text = ((countdowntime).ToString() + "초 남음...");
                }
              
            } else {
                txtSoundCountDownDo.text = " ";
                CountDownDo = 6.3f;
            }


            /*
            // 테스트용
            // 기울이기 동작 구간에서 + up 키를 누르면 카운트 증가
            if (Input.GetKeyDown(KeyCode.UpArrow) && ((_time >= 26f && _time <= 32.9f) || (_time >= 47.4f && _time <= 53.9f) || (_time >= 65.15f && _time <= 72.2f) || (_time >= 83.7f && _time <= 90.5f) || (_time >= 108.2f && _time <= 115.1f) || (_time >= 138f && _time <= 145.4f) || (_time >= 156f && _time <= 162.5f) || (_time >= 169.4f && _time <= 176.9f) || (_time >= 182.5f && _time <= 188.7f) || (_time >= 194.1f && _time <= 200.5)))
            {
                playerSlope++;
                //upText.text = (playerSlope.ToString());
            }
            */

            
            // 남은 목숨이 없을 경우 게임 종료시키기
            if (Life == 0)
            {
                //SceneManager.LoadScene("Fail");
                GameObject.Find("movingPerson").transform.Find("EndMenu").gameObject.SetActive(true);
            }
          

            /*
            // 플레이어 실패 시 깜박임 종료 시간 (time을 int로 변환해서 equal 조건주면 멈추지 않음)
            if((int)_time == 36 || (int)_time == 57 || (int)_time == 75 || (int)_time == 93 || (int)_time == 118 || (int)_time == 148 || (int)_time == 165 || (int)_time == 180 || (int)_time == 193 || (int)_time == 204 )
            {
                isFail = false;
                slopeGoodtime = 0.0f;
                AudioPlay = false; // 오디오 출력을 한 번만 하게 하기 위한 변수
                isSlope = false;
            } 
            */

            
        //    // 시간 구간
        //    // trigger 대신 시간 구간으로 slope들을 나누고, 각 구간에서 true/false로 애니메이터 조작
        //    if (_time >= 26f && _time <= 31.9f) // slope_01
        //    {

        //        // 운동 동작 중 안내문 출력
        //        txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //        IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

        //        IsCountDownDo = true; // 운동 동작 중 카운트 다운


        //        // 여기에서 타이머를 쓰면 타이머가 동작은 하지만 애니메이터가 멈춤.. 그래서 성공/실패 동작이 재생되지 않는다.

        //        if (leftGood == true) // 올바른 왼쪽 동작을 하면
        //        {
        //            slopeGoodtime += Time.deltaTime; // 카운트 시작
        //        } else
        //        {
        //            slopeGoodtime = 0.0f; // 올바른 왼쪽 동작이 나오지 않으면 바로 타이머 초기화

        //            txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //        }


        //        // 0815 에러를 수정하고자 고친 코드
        //        if(_time >= 26f && _time <= 28.95f) // 기울여서 내려가는 동작이 실행되는 시간에 다음 동작의 파라미터를 결정(실제보다 좀 더 촉박하게 잡음)
        //        {
        //            if ((int)slopeGoodtime >= 3)
        //            {
        //                // 멈춤
        //                animator1.SetBool("slopeFail", false); // 시간 구간 내에서 true가 되므로, 빠져나갈 때 바꿔줘야 함
        //                animator1.SetBool("LeftBack", true); // 성공 동작

        //                isDeath = false;
        //            }
        //            else
        //            {
        //                animator1.SetBool("slopeFail", true); // 실패 동작

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력

        //            }
        //        }

              

        //        /*
        //        // 0815 에러가 발생한 코드
        //        if ((int)slopeGoodtime >= 3)
        //        {
        //            // 멈춤
        //            animator1.SetBool("slopeFail", false); // 시간 구간 내에서 true가 되므로, 빠져나갈 때 바꿔줘야 함
        //            animator1.SetBool("LeftBack", true); // 성공 동작

        //            isDeath = false;
        //        }
        //        else
        //        {
        //            animator1.SetBool("slopeFail", true); // 실패 동작

        //            isDeath = true;

        //            txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력

        //        }
        //        */

        //        /*
        //         * 컨트롤러 없을 때 키보드로 테스트하기 위한 코드
        //        // 기울이기 동작이 끝나기 전까지 조건(3초 이상)을 맞춰야 성공 동작이 실행됨... 애니메이터가 예전에 멈췄던 것도 이와 관련된 문제인 것 같음
        //        // 노트에 적어놓은 대로 생각하면 큰 문제가 되지 않는다고 생각함..
        //        if (playerSlope >= 3)
        //        {
        //            animator1.SetBool("slopeFail", false); // 시간 구간 내에서 true가 되므로, 빠져나갈 때 바꿔줘야 함
        //            animator1.SetBool("LeftBack", true); // 성공 동작
        //        }
        //        else
        //        {
        //            animator1.SetBool("slopeFail", true); // 실패 동작

        //            txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력
        //        }
        //        */
              
        //        /*
        //         * 가장 오래 전 코드, 기본(단순하게 시간 조건 없이 검사)
        //        // 사용자의 올바른 입력 값
        //        if (leftGood == true)
        //        {
        //            animator1.SetBool("LeftBack", true);
        //        }
        //        else // 올바르지 않은 입력 값
        //        {
        //            animator1.SetBool("slopeFail", true);
        //        }
        //        */

        //    } else if(_time >= 46.9f && _time <= 52.9f) // slope_02
        //    {
        //            /*
        //            // 시간 구간으로 네비메쉬 중지시키기?
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            animator1.SetBool("LeftBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운

        //            if (rightGood == true)
        //            {
        //                slopeGoodtime += Time.deltaTime;
        //            }
        //            else
        //            {
        //                slopeGoodtime = 0.0f;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
              
              


        //        if (_time >= 46.9f && _time <= 49.9f)
        //        {
        //            if ((int)slopeGoodtime >= 3)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("RightBack", true);

        //                isDeath = false;
        //            }
        //            else
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!";

        //            }
        //        }
              
              

        //    } else if(_time >= 64.8f && _time <= 71.4f) // slope_03
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            animator1.SetBool("RightBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운

        //            if (leftGood == true)
        //            {
        //                slopeGoodtime += Time.deltaTime;
        //            }
        //            else
        //            {
        //                slopeGoodtime = 0.0f;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              

        //        if(_time >= 64.8f && _time <= 68.1f)
        //        {
        //            if ((int)slopeGoodtime >= 3)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("LeftBack", true);

        //                isDeath = false;
        //            }
        //            else
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!";

        //            }
        //        }
              

        //    } else if(_time >= 83.4f && _time <= 89.6f) // slope_04
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */
                  

        //            animator1.SetBool("LeftBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운

        //            if (rightGood == true)
        //            {
        //                slopeGoodtime += Time.deltaTime;
        //            }
        //            else
        //            {
        //                slopeGoodtime = 0.0f;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              

        //        if(_time >= 83.4f && _time <= 86.5f)
        //        {
        //            if ((int)slopeGoodtime >= 3)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("RightBack", true);

        //                isDeath = false;
        //            }
        //            else
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!";

        //            }
        //        }
              
        //    } else if(_time >= 107.4f && _time <= 114.5f) // slope_05
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */
        //        }
                  

        //            animator1.SetBool("RightBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운

        //            if (leftGood == true)
        //            {
        //                slopeGoodtime += Time.deltaTime;
        //            }
        //            else
        //            {
        //                slopeGoodtime = 0.0f;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
              
              

        //        if(_time >= 107.4f && _time <= 110.95f)
        //        {
        //            if ((int)slopeGoodtime >= 3)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("LeftBack", true);

        //                isDeath = false;
        //            }
        //            else
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!";

        //            }
        //        }
              
        //    } else if(_time >= 138f && _time <= 144.5f) // slope_06
        //    {
        //        // slope_06 이후 시간 구간과 slope가 맞지 않아 생기는 문제를 위해 조건 검사 추가
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            if (AudioPlay == false)
        //            {
        //                AudioPlay = true;
        //                AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
        //            }


        //            IsSlopeArrowRight = true;

        //            IsCountDown = false;
        //            //IsCountDownDo = true;


        //            animator1.SetBool("IsSlopeRight", true);
        //            animator2.SetBool("IsSlopeRight", true);

        //            animator1.SetBool("LeftBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운
        //        }

        //        if (_time >= 138f && _time <= 141.25f)
        //        {
        //            // 사용자의 올바른 입력 값
        //            if (rightGood == true)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("RightBack", true);

        //                isDeath = false;
        //            }
        //            else // 올바르지 않은 입력 값
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
        //        if((int)_time >= 144)
        //        {
        //            txtSound.text = " ";
        //        }

        //    } else if(_time >= 155f && _time <= 161.5f) // slope_07
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            if (AudioPlay == false)
        //            {
        //                AudioPlay = true;
        //                AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position);
        //            }


        //            //txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true;

        //            IsCountDown = false;
        //            //IsCountDownDo = true;


        //            animator1.SetBool("IsSlopeLeft", true);
        //            animator2.SetBool("IsSlopeLeft", true);

        //            animator1.SetBool("RightBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운
        //        }

        //        if (_time >= 155f && _time <= 158.25f)
        //        {
        //            // 사용자의 올바른 입력 값
        //            if (leftGood == true)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("LeftBack", true);

        //                isDeath = false;
        //            }
        //            else // 올바르지 않은 입력 값
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              

        //    } else if(_time >= 168.7f && _time <= 176f) // slope_08
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            if (AudioPlay == false)
        //            {
        //                AudioPlay = true;
        //                AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
        //            }


        //            //txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true;

        //            IsCountDown = false;
        //            //IsCountDownDo = true;


        //            animator1.SetBool("IsSlopeRight", true);
        //            animator2.SetBool("IsSlopeRight", true);

        //            animator1.SetBool("LeftBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운
        //        }

        //        if (_time >= 168.7f && _time <= 172.35f)
        //        {
        //            // 사용자의 올바른 입력 값
        //            if (rightGood == true)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("RightBack", true);

        //                isDeath = false;
        //            }
        //            else // 올바르지 않은 입력 값
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              

        //    } else if(_time >= 182.9f && _time <= 189f) // slope_09
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            if (AudioPlay == false)
        //            {
        //                AudioPlay = true;
        //                AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position);
        //            }


        //            //txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true;

        //            IsCountDown = false;
        //            //IsCountDownDo = true;


        //            animator1.SetBool("IsSlopeLeft", true);
        //            animator2.SetBool("IsSlopeLeft", true);

        //            animator1.SetBool("RightBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운
        //        }

        //        if (_time >= 182.9f && _time <= 185.95f)
        //        {
        //            // 사용자의 올바른 입력 값
        //            if (leftGood == true)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("LeftBack", true);

        //                isDeath = false;
        //            }
        //            else // 올바르지 않은 입력 값
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              

        //    } else if(_time >= 194.1f && _time <= 200.5f) // slope_10
        //    {
        //        if (isSlope == true)
        //        {
        //            /*
        //            // 네비메쉬 중지
        //            nvAgent.velocity = Vector3.zero;
        //            nvAgent.Stop(true);
        //            */

        //            if (AudioPlay == false)
        //            {
        //                AudioPlay = true;
        //                AudioSource.PlayClipAtPoint(sndRightPrev, transform.position);
        //            }

        //            //txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true;

        //            IsCountDown = false;
        //            //IsCountDownDo = true;


        //            animator1.SetBool("IsSlopeRight", true);
        //            animator2.SetBool("IsSlopeRight", true);

        //            animator1.SetBool("LeftBack", false);

        //            // 운동 동작 중 안내문 출력
        //            txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 오른쪽 방향으로 5초 동안 해주세요.";

        //            IsSlopeArrowRight = true; // 화살표를 출력하기 위한 변수 설정

        //            IsCountDownDo = true; // 운동 동작 중 카운트 다운

        //        }

        //        if (_time >= 194.1f && _time <= 197.2f)
        //        {
        //            // 사용자의 올바른 입력 값
        //            if (rightGood == true)
        //            {
        //                animator1.SetBool("slopeFail", false);
        //                animator1.SetBool("RightBack", true);

        //                isDeath = false;
        //            }
        //            else // 올바르지 않은 입력 값
        //            {
        //                animator1.SetBool("slopeFail", true);

        //                isDeath = true;

        //                txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
        //            }
        //        }
              
        //    }


        //    // 시간으로 다시 네비메쉬 작동 시키기
        //    if (_time >= 31.9f && _time < 46.9f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeLeft", false);
        //        animator2.SetBool("IsSlopeLeft", false);
        //    }
        //    else if (_time >= 52.9f && _time <= 64.8f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeRight", false);
        //        animator2.SetBool("IsSlopeRight", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 71.4f && _time <= 83.4f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeLeft", false);
        //        animator2.SetBool("IsSlopeLeft", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 89.6f && _time <= 107.4f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeRight", false);
        //        animator2.SetBool("IsSlopeRight", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 114.5f && _time <= 138f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeLeft", false);
        //        animator2.SetBool("IsSlopeLeft", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 144.5f && _time <= 155f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeRight", false);
        //        animator2.SetBool("IsSlopeRight", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);                
        //    }
        //    else if (_time >= 161.5f && _time <= 168.7f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeLeft", false);
        //        animator2.SetBool("IsSlopeLeft", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);                
        //    }
        //    else if (_time >= 176f && _time <= 182.9f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeRight", false);
        //        animator2.SetBool("IsSlopeRight", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 189f && _time <= 194.1f)
        //    {
        //        //nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeLeft", false);
        //        animator2.SetBool("IsSlopeLeft", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }
        //    else if (_time >= 200.5f)
        //    {
        //        // nvAgent.isStopped = false; // 네비메쉬
        //        animator1.SetBool("IsSlopeRight", false);
        //        animator2.SetBool("IsSlopeRight", false);
        //        animator1.SetBool("IsWalk", true);
        //        animator2.SetBool("IsWalk", true);
        //    }

        }

    }
}