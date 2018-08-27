using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace Ardunity
{

    public class Player : MonoBehaviour
    {
        private GameUI UIObj;      // ui 오브젝트
        private GameObject[] img;
        private StepCtrl stepCtrl;

        public float x;     // 마우스에 따른 x회전 위치
        public int cornerCount = 1;

        public GameObject snowing; // 눈내리는 이벤트
        private GameObject cornertr;
        public GameObject BoostEf; // 부스터 이펙트
        private GameObject newEf;

        NavMeshAgent nvAgent;

        public float nvSpeed, pSpeed; // 플레이어 속도, 네비 속도
        public bool isColl, isBoost, isCollEnemy;
        public bool isSt1, isSt2, isSt3;
        private int Life;
        private float countTime;

        public Renderer lamp;
        private Color originColor; // 원래 색깔

        // Use this for initialization
        void Start()
        {
            originColor = lamp.material.color;
            // 네비게이션
            nvAgent = gameObject.GetComponent<NavMeshAgent>();
            nvAgent.speed = nvSpeed; // 코너를 따라가는 속도

            UIObj = GameObject.Find("Game_UI").GetComponent<GameUI>(); // GameUI 오브젝트 찾음
            UIObj.ChangeCorner(5 - cornerCount);
            img = GameObject.FindGameObjectsWithTag("img"); // 이미지 UI 오브젝트들
            Life = img.Length;
            Debug.Log("목숨 : " + Life);
            isSt1 = false; isSt2 = false; isSt3 = false; // 스텝 1,2,3은 다 비활성 상태
            isBoost = false; isColl = false;

            stepCtrl = GetComponent<StepCtrl>();

            nvSpeed = 4f;
            pSpeed = 0.17f;
        }

        // Update is called once per frame
        void Update()
        {
            if (Life <= 0)
            {
                // 게임 오버 메시지 뜨게 함- 목숨을 다 소모했다고 함
                UIObj.ChangeGameOver(0);
            }
            else
            {
                //RotationReactor2 snowsnow = GameObject.Find("snow").GetComponent<RotationReactor2>(); // 나중에 형이 바꿔줌

                if (cornerCount != 5) // 코너를 다 돌지 않았으면 계속 코너를 따라가도록 함
                {
                    cornertr = GameObject.FindWithTag("corner" + cornerCount);
                    nvAgent.SetDestination(cornertr.transform.position);
                    nvAgent.speed = nvSpeed; // 코너를 따라가는 속도
                }
                // 눈 이벤트 위치 조정
                Vector3 vector3 = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);
                snowing.transform.position = vector3;

                if(isCollEnemy)
                {
                    float flicker = Mathf.Abs(Mathf.Sin(Time.time * 10));
                    lamp.material.color = originColor * flicker;
                }

                if (!isColl) // 충돌하지 않으면
                {
                    //플레이어 회전 구현
                    // 코너 1 : -90~90, 코너 2 : 0~180 ....
                    x = (Input.mousePosition.x - Screen.width / 2) / Screen.width; // x의 범위는 -0.5~ 0.5

                    // 회전반경 제한
                    if (x < -0.5f) x = -0.5f; if (x > 0.5f) x = 0.5f;

                    Quaternion turn = Quaternion.Euler(0, x * 180 + 90 * (cornerCount - 1), 0);
                    transform.rotation = turn;
                    //transform.rotation = Quaternion.Slerp(transform.rotation, turn, 7 * Time.deltaTime); // 부드럽게 턴
                    transform.Translate(Vector3.forward * pSpeed); // 가속

                    if (!isBoost) // 부스터가 아님
                    {
                        pSpeed *= 0.996f;
                        nvSpeed *= 0.996f;
                    }
                    else // 부스터 상태
                    {
                        if (newEf) // 오브젝트가 할당 된 상태야 함, 부스터 이펙트 생성
                        {
                            newEf.transform.position = transform.position;
                            newEf.transform.Translate(new Vector3(0, 2, 0));
                        }

                        pSpeed *= 1.2f;
                        nvSpeed *= 1.2f;

                        if (pSpeed >= 0.3f) pSpeed = 0.3f;
                        if (nvSpeed >= 4f) nvSpeed = 4f;
                    }
                }

                if(Time.time <= countTime) // 제한 시간안에 올바른 동작을 함
                {

                }
                else // 동작을 수행하지 못함 : 목숨 깎이고 실패하는 애니메이션 - 미끄러지는 애니메이션 말고, 속도는 그대로
                {

                }

                if (Input.GetKeyDown(KeyCode.Q) && !isSt1) // Time.time <= countTime 나중에 이것도 추가 - 제한 시간안에 올바른 동작을 함
                {
                    stepCtrl.StepOne();
                    isSt1 = true;
                    
                }
                else if (Input.GetKeyDown(KeyCode.W) && !isSt2)
                {
                    stepCtrl.StepTwo();
                    isSt2 = true;
                    
                }
                else if (Input.GetKeyDown(KeyCode.E) && !isSt3)
                {
                    stepCtrl.StepThree();
                    isSt3 = true;
                    
                }

                /**** 밑에는 아두니티 쓰는거 *****/
                //if (snowsnow.snow0)
                //{
                //    stepCtrl.StepOne();
                //}
                //else if (snowsnow.snow1)
                //{
                //    stepCtrl.StepTwo();
                //}
                //else if (snowsnow.snow2)
                //{
                //    stepCtrl.StepThree();
                //}
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    stepCtrl.StepSuccess();
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    stepCtrl.StepFail();
                }
            }
        }

        void OnCollisionEnter(Collision coll) // 부딪혔을 때
        {
            if (coll.collider.tag == ("corner" + cornerCount))
            {
                isColl = true;            // 부딪힌 상태
                Destroy(coll.gameObject); // 코너 삭제
                
                cornerCount++;
                UIObj.GetComponent<GameUI>().ChangeCorner(5 - cornerCount);

                if(cornerCount < 5)
                {
                    Quaternion turn = Quaternion.Euler(0, x * 180 + 30 * (cornerCount - 1), 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, turn, 0.01f * Time.deltaTime); // 부드럽게 턴
                    Invoke("isCollFalse", 2f); // 3초 후에 정상
                }
                else
                {
                    // 맵을 다 돌았으니까 최종 결과가 나오고, 만약 최고기록보다 짧게 들어오면 갱신함
                    UIObj.ChangeGameOver(1);
                    UIObj.ChangeBest();
                }
            }

            if (coll.collider.tag == ("enemy"))
            {
                isColl = true;            // 부딪힌 상태
                isCollEnemy = true;
                // 목숨 깎이고, 뱅글돌아가는 효과 호출
                nvSpeed = 0f;
                pSpeed = 0f;

                Debug.Log("적이랑 부딪힘 ㅜㅜ : life" + Life + "번 깎임");
                Destroy(GameObject.Find("life" + Life));
                Life--;

                stepCtrl.StepFail();
                Invoke("isCollFalse", 3f); // 3초 후에 정상
                Clear();
            }
        }

        void isCollFalse() // 코너 부딪힘 끝
        {
            Debug.Log("isColl false!");
            isColl = false;
            isCollEnemy = false;
            lamp.material.color = originColor;
        }

        public void Booster() // 부스터 온
        {
            isBoost = true;
            Debug.Log("부스터 온!");
            newEf = Instantiate(BoostEf, this.transform.position, this.transform.rotation) as GameObject;

            nvSpeed = 1f;
            pSpeed = 0.10f;
            Destroy(newEf, 1.5f); // 1.5초뒤에 이펙트 삭제
            Invoke("BoostOff", 1.5f); // 1.5초 후에 정상
        }

        void BoostOff() // 부스터 끄기
        {
            Debug.Log("부스터 오프!");
            isBoost = false;
        }

        void CountDown() // 동작을 실행하기 위해 시간을 초기화
        {
            countTime = Time.time + 5f; // 5초 추가
        }

        public void Clear() // 불값 초기화
        {
            isSt1 = false;
            isSt2 = false;
            isSt3 = false;
        }
    }
}