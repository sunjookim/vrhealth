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

        public GameObject CornerPrefabs;
        public GameObject snowing; // 눈내리는 이벤트
        private GameObject cornertr;
        public GameObject BoostEf; // 부스터 이펙트
        private GameObject newEf;

        NavMeshAgent nvAgent;

        public float nvSpeed, pSpeed; // 플레이어 속도, 네비 속도
        private bool isColl = false;
        private bool isBoost = false;
        int Life;

        // Use this for initialization
        void Start()
        {
            // 네비게이션
            nvAgent = gameObject.GetComponent<NavMeshAgent>();
            nvAgent.speed = nvSpeed; // 코너를 따라가는 속도

            UIObj = GameObject.Find("Game_UI").GetComponent<GameUI>(); // GameUI 오브젝트 찾음
            UIObj.ChangeCorner(5 - cornerCount);
            img = GameObject.FindGameObjectsWithTag("img"); // 이미지 UI 오브젝트들
            Life = img.Length;

            stepCtrl = GetComponent<StepCtrl>();

            nvSpeed = 4f;
            pSpeed = 0.17f;
        }

        // Update is called once per frame
        void Update()
        {

            RotationReactor2 snowsnow = GameObject.Find("snow").GetComponent<RotationReactor2>(); // 

            if (cornerCount != 5) // 코너를 다 돌지 않았으면 계속 코너를 따라가도록 함
            {
                cornertr = GameObject.FindWithTag("corner" + cornerCount);
                nvAgent.SetDestination(cornertr.transform.position);
                nvAgent.speed = nvSpeed; // 코너를 따라가는 속도
            }
            // 눈 이벤트 위치 조정
            Vector3 vector3 = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);
            snowing.transform.position = vector3;

            //플레이어 회전 구현
            // 코너 1 : -90~90, 코너 2 : 0~180 ....
            x = (Input.mousePosition.x - Screen.width / 2) / Screen.width; // x의 범위는 -0.5~ 0.5
                                                                           // 회전반경 제한
            if (x < -0.5f) x = -0.5f;
            if (x > 0.5f) x = 0.5f;

            if (!isColl) // 충돌하지 않으면
            {
                Quaternion turn = Quaternion.Euler(0, x * 180 + 90 * (cornerCount - 1), 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, turn, 7 * Time.deltaTime); // 부드럽게 턴
                                                                                                     //transform.rotation = turn;

                //if (x <= -0.25f || x >= 0.25f) // 좌, 우로 피하기 위해 가속
                transform.Translate(Vector3.forward * pSpeed);
                if (!isBoost) // 부스터가 아님
                {
                    pSpeed *= 0.995f;
                    nvSpeed *= 0.995f;
                }
                else // 부스터 상태
                {
                    if (newEf) // 오브젝트가 할당 된 상태야 함
                    {
                        newEf.transform.position = transform.position;
                        newEf.transform.Translate(new Vector3(0, 2, 0));
                    }

                    pSpeed *= 1.03f;
                    nvSpeed *= 1.03f;
                }
            }

            if (snowsnow.snow0)
            {
                stepCtrl.StepOne();
            }
            else if (snowsnow.snow1)
            {
                stepCtrl.StepTwo();
            }
            else if (snowsnow.snow2)
            {
                stepCtrl.StepThree();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                stepCtrl.StepSuccess();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                stepCtrl.StepFail();
            }
        }

        void OnCollisionEnter(Collision coll) // 코너와 부딪혔을 때
        {
            if (coll.collider.tag == ("corner" + cornerCount))
            {
                Destroy(coll.gameObject); // 코너 삭제
                isColl = true;            // 부딪힌 상태

                cornerCount++;
                UIObj.GetComponent<GameUI>().ChangeCorner(5 - cornerCount);

                Quaternion turn = Quaternion.Euler(0, x * 180 + 45 * (cornerCount - 1), 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, turn, 7 * Time.deltaTime); // 부드럽게 턴
                Invoke("isCollFalse", 2f); // 3초 후에 정상
            }

            if (coll.collider.tag == ("enemy"))
            {
                // 목숨 깎이고, 뱅글돌아가는 효과 호출
                Debug.Log("적이랑 부딪힘 ㅜㅜ : life" + Life + "번 깎임");
                Destroy(GameObject.Find("life" + Life));
                Life--;
                stepCtrl.StepFail();
            }
        }

        void isCollFalse()
        {
            Debug.Log("isColl false!");
            isColl = false;
        }

        public void Booster()
        {
            isBoost = true;
            Debug.Log("부스터 온!");
            newEf = Instantiate(BoostEf, this.transform.position, this.transform.rotation) as GameObject;
            nvSpeed = 1f;
            pSpeed = 0.10f;
            Destroy(newEf, 1.5f); // 3초뒤에 이펙트 삭제
            Invoke("BoostOff", 1.5f); // 3초 후에 정상
        }

        void BoostOff()
        {
            Debug.Log("부스터 오프!");
            isBoost = false;
        }
    }
}