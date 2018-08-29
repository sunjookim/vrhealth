﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ardunity
{
    [RequireComponent(typeof(AudioSource))]
    public class StepCtrl : MonoBehaviour
    {
        private AudioSource audiosource = null;
        private GameObject[] Enemy;
        private GameObject playerPr;
        private Player player;
        private float[] spownPo1, spownPo2, spownPo3, spownPo4;
        
        public GameObject EnemyPrefabs;
        public GameObject EnemyPrefabs2;
        public GameObject UIObj;
        public Animator animator;       // 애니메이션 객체

        private int count;
        private float dis;      // 장애물이 떨어져있는 거리

        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>(); // 플레이어 스크립트 객체
            playerPr = GameObject.FindWithTag("Player"); // 플레이어 위치
            animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
            Enemy = new GameObject[2];

            audiosource = GetComponent<AudioSource>(); // 오디오
            UIObj = GameObject.Find("Game_UI");

            spownPo1 = new float[] { -315, -(315 + 330) / 2, -330 };
            spownPo2 = new float[] { 265, (265 + 247) / 2, 247 };
            spownPo3 = new float[] { -48, -(48 + 76) / 2, -76 };
            spownPo4 = new float[] { -103, -(103 + 125) / 2, -125 };
            
            count = player.cornerCount;
            dis = 30;
        }

        void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("lose"))
            {
                clear(); // 애니메이션 초기화
                player.Clear();
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("win"))
            {
                player.Clear();
                clear();
            }
            count = player.cornerCount;
        }

        // 동작 1, 팔 모으기
        public void StepOne()
        {
            clear();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("wait"))
            {
                Debug.Log("1번째 동작!");
                for (int i = 0; i < 2; i++)
                {
                    int num = Random.Range(0, 2);
                    if (num == 0) Enemy[i] = (GameObject)Instantiate(EnemyPrefabs); // 생성
                    else Enemy[i] = (GameObject)Instantiate(EnemyPrefabs2); // 생성

                    Enemy[i].name = "Enemy_" + i.ToString();
                    Enemy[i].SetActive(false); // 비활성화 상태로 생성
                    Debug.Log("생성된 에너미 : " + num);
                }
                animator.SetBool("step1", true);
            }
        }

        // 동작 2, 팔 펴기
        public void StepTwo()
        {
            clear();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("step1"))
            {
                Debug.Log("2번째 동작!");
                Vector3 v = playerPr.transform.position;
                int idx = Random.Range(0, spownPo1.Length); // 스폰 지역 중 랜덤으로 선택(좌, 중, 우)
                if (count == 1)
                {
                    int a = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == idx)
                            continue;
                        else
                        {
                            Enemy[a].transform.position = new Vector3(spownPo1[i], v.y, v.z + dis);
                            a++;
                        }
                    }
                }
                else if (count == 2)
                {
                    int a = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == idx)
                            continue;
                        else
                        {
                            Enemy[a].transform.position = new Vector3(v.x + dis, v.y, spownPo2[i]);
                            a++;
                        }
                    }
                }
                else if (count == 3)
                {
                    int a = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == idx)
                            continue;
                        else
                        {
                            Enemy[a].transform.position = new Vector3(spownPo3[i], v.y, v.z - dis);
                            a++;
                        }
                    }
                }
                else if (count == 4)
                {
                    int a = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == idx)
                            continue;
                        else
                        {
                            Enemy[a].transform.position = new Vector3(v.x - dis, v.y, spownPo4[i]);
                            a++;
                        }
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    Enemy[i].transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0));
                    Debug.Log("장애물 위치 : " + idx);
                    Enemy[i].SetActive(true);
                }
                animator.SetBool("step2", true);
            }
        }

        // 동작 3, 팔 내리면서 허리 굽히기
        public void StepThree()
        {
            clear();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("step2"))
            {
                Debug.Log("3번째 동작!");
                player.Booster(); // 속도 일시적으로 상승
                animator.SetBool("step3", true);
            }
        }

        // 동작 4, 동작 실패, 장애물 충돌 등 
        public void StepFail()
        {
            Debug.Log("실패! ㅜㅜ");
            animator.SetBool("lose", true);
        }

        // 동작 5, 동작 성공, 제한시간안에 들어옴 등
        public void StepSuccess()
        {
            Debug.Log("성공 ^^");
            animator.SetBool("win", true);
        }

        void clear()
        {
            animator.SetBool("step1", false);
            animator.SetBool("step2", false);
            animator.SetBool("step3", false);
            animator.SetBool("lose", false);
            animator.SetBool("win", false);
        }
    }
}