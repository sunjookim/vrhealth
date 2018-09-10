using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 캔버스 제어
using UnityEngine.SceneManagement; // 씬 전환

namespace Ardunity
{

    public class Game2 : MonoBehaviour
    {
        Animator animator1; // 플레이어 애니메이터
        Animator animator2; // teacher 애니메이터


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
        //public Text txtLifeReason; // 별 깎인 이유 출력

        
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
        bool isGood = false; // slope에서 한 번이라도 올바른 동작을 했는지?
        bool isPause = false; // 일시정지가 눌렸는지 검사
        bool isPauseOnce = false; // 일시정지 계속하기 클릭 시 캐릭터 속도 회복 1번만

        public GameObject slopeArrowLeft; // teacher 캐릭터에 화살표 출력
        public GameObject slopeArrowRight; // teacher 캐릭터에 화살표 출력
        
        
        int countdowntime; // 운동 동작 중 카운트 다운 숫자
        int Life = 4; // 목숨의 개수
        int PlayerSpeed = 4; // 플레이어 걸어가는 속도


        float speed = 0.00178f; // 플레이어 상태바가 움직이는 속도              
        float turnspeed = 2f; // 마우스 커서로 회전하는 속도


        Vector3 v3; // 마우스 커서 회전에 사용되는 변수


        public AudioClip sndLeft; // 왼쪽으로 기울기 안내 음성
        public AudioClip sndRight; // 오른쪽으로 기울기 안내 음성
        public AudioClip sndLeftPrev; // 왼쪽으로 기울기 안내 음성 (시작 전)
        public AudioClip sndRightPrev; // 오른쪽으로 기울기 안내 음성 (시작 후)


        public Projector fail_color; // 실패 시 플레이어를 깜박이게 하기 위한 프로젝터 컴포넌트

        
        void Start()
        {
            // 게임 방법 설명
            //GameObject.Find("Ardunity").transform.Find("movingPerson").transform.Find("Main Camera").transform.Find("HowtoCanvas").gameObject.SetActive(true);

            // 실패 시 깜박임 처리하기 위해 프로젝터를 사용하였고 이 컴포넌트를 변수에 저장
            fail_color = GameObject.FindWithTag("fail_color").GetComponent<Projector>();

            // 처음 시작할 땐 화살표들 안보이게 처리
            slopeArrowLeft.SetActive(false);
            slopeArrowRight.SetActive(false);

            Time.timeScale = 1; // 일시정지 버튼 - 재시작 버튼을 눌렀을 때 time이 멈추는 현상 방지
            

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
                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

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

                
            }
            else if (other.gameObject.tag == "slope_02")
            {
                

                isGood = false;

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작
                

                PlayerSpeed = 0; // 플레이어 정지
                

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

                
            }
            else if (other.gameObject.tag == "slope_03")
            {
                isGood = false;

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

                isSlopeGoTime = true; // 기울여 내려가는 애니메이션 카운트 감소 시작
                

                PlayerSpeed = 0; // 플레이어 정지
                
                isSlope = true; // slope에 해당하는지의 변수


                //// 오디오가 여러 번 출력이 되어서 수정함
                if (AudioPlay == false)
                {
                    AudioPlay = true;
                    AudioSource.PlayClipAtPoint(sndLeftPrev, transform.position); // 안내음성 출력
                }

                animator1.SetBool("slopeFail", false);

                IsCountDown = false; // (UI)카운트 다운 중이 아니다
                

                // 캐릭터 움직이기(1 : 플레이어, 2 : teacher)
                animator1.SetBool("IsSlopeLeft", true); // 애니메이션 transition에 사용되는 변수 조정(왼쪽으로 기울기)
                animator2.SetBool("IsSlopeLeft", true);


                // 운동 동작 중 안내문 출력
                txtSound.text = "두 팔을 뻗어 위로 올리고\n허리 운동을 왼쪽 방향으로 5초 동안 해주세요.";

                IsSlopeArrowLeft = true; // 화살표를 출력하기 위한 변수 설정

                IsCountDownDo = true; // 운동 동작 중 카운트 다운

                

            }
            else if (other.gameObject.tag == "slope_04")
            {
                isGood = false;

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                

            }
            else if (other.gameObject.tag == "slope_05")
            {
                isGood = false;

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                

            }
            else if (other.gameObject.tag == "slope_06")
            {
                isGood = false;

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                
            }
            else if (other.gameObject.tag == "slope_07")
            {
                isGood = false;

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                
            }
            else if (other.gameObject.tag == "slope_08")
            {
                isGood = false;

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                
            }
            else if (other.gameObject.tag == "slope_09")
            {
                isGood = false;

                isLeftSlope = true; // 왼쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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

                
            }
            else if (other.gameObject.tag == "slope_10")
            {
                isGood = false;

                isRightSlope = true; // 오른쪽 슬로프에 도달했다고 update에 알림

                SlopeGoTime = 4.0f;

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
                GameObject.Find("GameObject").transform.rotation = Quaternion.Euler(0, -89.8f, 0); // 버튼들 회전

            }
            else if(other.gameObject.tag == "rotation_02") // 두 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, -65.1f, 0); // 플레이어 회전
                GameObject.Find("GameObject").transform.rotation = Quaternion.Euler(0, -65.1f, 0); // 버튼들 회전
            }
            else if(other.gameObject.tag == "rotation_03") // 세 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, -94f, 0); // 플레이어 회전
                GameObject.Find("GameObject").transform.rotation = Quaternion.Euler(0, -94f, 0); // 버튼들 회전
            }
            else if(other.gameObject.tag == "rotation_04") // 네 번째 회전
            {
                transform.rotation = Quaternion.Euler(0, 1.7f, 0); // 플레이어 회전
                GameObject.Find("GameObject").transform.rotation = Quaternion.Euler(0, 1.7f, 0); // 버튼들 회전
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
                    /*
                    // 별 제거
                    Destroy(GameObject.FindWithTag("Life" + Life));
                    Life -= 1;

                    isFail = true; // 캐릭터 깜박이기 시작
                    */
                }

                // 안내문 초기화
                //txtLifeReason.text = " ";
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

                //isLeftSlope = false;
                //isRightSlope = false;

                slopeGoodtime = 0.0f;

                AudioPlay = false;

            }
            
        }

        void Update()
        {
            

                /*
                // 키보드로 테스트
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
                // bool 변수에 컨트롤러 값들을 저장(trigger 함수에서 사용하기 위함)
                leftGood = BBB.leftback;
                rightGood = BBB.rightback;


                // true/false 값 출력하기
                print(BBB.leftback);
                print(BBB.rightback);
                print("SlopeGoTime: " + SlopeGoTime);
                print("leftGood: " + leftGood);
                print("rightGood: " + rightGood);
                print("isleft: " + isLeftSlope);
                print("isright: " + isRightSlope);
                print("isGood: " + isGood);
                print("isDeath: " + isDeath);


                // 진행 시간 표시
                _time += Time.deltaTime;
                int minute = (int)_time / 60;
                _timerText.text = (minute.ToString());


                
                // 컨초_허리 영상 끝나면 캐릭터 출발시키고 이동하기
                if (BBB.count2 >= 250)
                {
                    // Destroy(GameObject.FindWithTag("Initialize")); // 컨초_허리 영상 제거하기

                    PlayerMove = Time.deltaTime * PlayerSpeed;
                    transform.Translate(Vector3.forward * PlayerMove); // 플레이어 이동
                    GameObject.Find("movingButton").transform.Translate(Vector3.forward * PlayerMove); // 버튼들 같이 이동

                    // 상태바 이동
                    float fMove2 = Time.deltaTime * speed;
                    GameObject.FindGameObjectWithTag("barPlayer").transform.Translate(Vector3.right * fMove2);

                }


                /*
                PlayerMove = Time.deltaTime * PlayerSpeed;
                transform.Translate(Vector3.forward * PlayerMove); // 플레이어 이동
                GameObject.Find("movingButton").transform.Translate(Vector3.forward * PlayerMove); // 버튼들 같이 이동

                // 상태바 이동
                float fMove2 = Time.deltaTime * speed;
                GameObject.FindGameObjectWithTag("barPlayer").transform.Translate(Vector3.right * fMove2);
                */


                if (isLeftSlope == true && (int)SlopeGoTime >= 0) //왼쪽 슬로프에 도달 && 기울이기 동작이 끝나기 전
                {


                    // 올바른 왼쪽 동작 값이 들어오면 카운트를 시작하고, 올바르지 않은 값이 들어오면 안내문 출력 및 타이머를 초기화한다.
                    if (leftGood == true) // 올바른 왼쪽 동작을 하면
                    {
                        //slopeGoodtime += Time.deltaTime; // 카운트 시작
                        isGood = true;
                    }
                    else
                    {
                        //slopeGoodtime = 0.0f; // 올바른 왼쪽 동작이 나오지 않으면 바로 타이머 초기화

                        //txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
                    }
                }

                if (isRightSlope == true && (int)SlopeGoTime >= 0) // 오른쪽 슬로프에 도달 && 기울이기 동작이 끝나기 전
                {


                    // 올바른 오른쪽 동작 값이 들어오면 카운트를 시작하고, 올바르지 않은 값이 들어오면 안내문 출력 및 타이머를 초기화한다.
                    if (rightGood == true) // 올바른 왼쪽 동작을 하면
                    {
                        //slopeGoodtime += Time.deltaTime; // 카운트 시작
                        isGood = true;
                    }
                    else
                    {
                        //slopeGoodtime = 0.0f; // 올바른 왼쪽 동작이 나오지 않으면 바로 타이머 초기화

                        //txtLifeReason.text = "※ 동작을 좀 더 정확하게 해주세요!";
                    }
                }


                if (isSlopeGoTime == true) // 기울여 내려가는 애니메이션 카운트 감소 시작
                {
                    SlopeGoTime -= Time.deltaTime;
                }


                // 기울여 내려가는 애니메이션이 끝났을 때 성공/실패여부를 검사
                if ((int)SlopeGoTime == 0)
                {
                    if (isGood == true)
                    {
                        //animator1.SetBool("slopeFail", false); // 시간 구간 내에서 true가 되므로, 빠져나갈 때 바꿔줘야 함

                        if (isLeftSlope == true) // 왼쪽 기울이기 동작 구간이라면
                        {
                            animator1.SetBool("LeftBack", true); // 성공 동작
                        }
                        else if (isRightSlope == true) // 오른쪽 기울이기 동작 구간이라면
                        {
                            animator1.SetBool("RightBack", true); // 성공 동작
                        }

                        isDeath = false;
                    }
                    else
                    {
                        animator1.SetBool("slopeFail", true); // 실패 동작

                        isDeath = true;

                        //txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력
                    }
                    /*
                    if(slopeGoodtime >= 0.8f) // 올바른 동작 3초를 채웠다면
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

                        //txtLifeReason.text = "※ 동작을 좀 더 오랫동안 해주세요!"; // 별이 깎인 이유 출력
                    }
                    */
                }
                else if (SlopeGoTime <= -1.5f)
                {
                    PlayerSpeed = 4;

                    isGood = false; // slope 중에 올바른 동작을 한 번이라도 했는지?를 false 값으로 초기화

                    animator1.SetBool("slopeFail", false);
                    animator1.SetBool("LeftBack", false);
                    animator1.SetBool("RightBack", false);

                    isLeftSlope = false;
                    isRightSlope = false;

                    /*
                    // 키보드 테스트 용
                    leftGood = false;
                    rightGood = false;
                    */
                }


                // 깜박깜박
                // 플레이어 캐릭터 깜박임 구현
                if (isFail == true)
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
                }
                else
                {
                    fail_color.enabled = false;
                }

                if (TwincleTime <= 0) // 2초가 지나면 깜박임 중지
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
                if (countdowntime == 0)
                {
                    IsCountDownDo = false;
                }

                // 동작 중일 때 몇 초 남았다고 카운트 다운 알려주는 글씨
                if (IsCountDownDo == true)
                {
                    CountDownDo -= Time.deltaTime;
                    countdowntime = (int)CountDownDo;

                    if (countdowntime <= 5)
                    {
                        txtSoundCountDownDo.text = ((countdowntime).ToString() + "초 남음...");
                    }

                }
                else
                {
                    txtSoundCountDownDo.text = " ";
                    CountDownDo = 6.3f;
                }


                // 남은 목숨이 없을 경우 버튼 뜨도록 하기
                if (Life == 0)
                {
                    GameObject.Find("movingPerson").transform.Find("EndMenu").gameObject.SetActive(true);
                }
           

        }

    }
}