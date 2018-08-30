﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour {

    public GameObject target; //타겟
    public GameObject explosion;
    private float angle;  //물로켓의 각도
    private float speed; //로켓 속도
    private bool rocketLaunchReady; //물로켓 발사 준비
    private bool exerciseRight; //운동을 하고 있는 지 확인
    public float timer; //시간초
    private float timer_check; //시간초 체크를 위한 저장
    public GameObject mainCamera; //메인 카메라
    public GameObject subCamera;  //로켓 카메라
    public GameObject activeCamera; //액션 카메라
    private Vector3 rocket_init_pos;
    private Quaternion rocket_init_rot;
    private Vector3 player_init_pos;
    public GameObject player;
    private Animator animator;

    // Use this for initialization
    void Start () {
        angle = 30f;
        speed = 30f;
        rocketLaunchReady = false;
        exerciseRight = false;
        timer = 0;
        mainCameraOn();
        rocket_init_pos = transform.position;
        rocket_init_rot = transform.rotation;
        player_init_pos = player.transform.position;
        animator = player.GetComponent<Animator>();
        animator.SetBool("kickstate", true);
    }
	
	// Update is called once per frame
	void Update () {
        //운동을 수행한다면(발을 올린다면) true, 발을 거의 내린다면 false;
        if (Input.GetKey(KeyCode.Space))
        {
            exerciseRight = true;
            timer_check = timer;

            if (mainCamera.GetComponent<GameDirector>().getX() > -0.131 && mainCamera.GetComponent<GameDirector>().getX() < -0.123)
                target = GameObject.FindWithTag("target");
            else if (mainCamera.GetComponent<GameDirector>().getX() > -0.106 && mainCamera.GetComponent<GameDirector>().getX() < -0.086)
                target = GameObject.FindWithTag("target (1)");
            else if (mainCamera.GetComponent<GameDirector>().getX() > -0.022 && mainCamera.GetComponent<GameDirector>().getX() < -0.001)
                target = GameObject.FindWithTag("target (2)");
            else if (mainCamera.GetComponent<GameDirector>().getX() > 0.063 && mainCamera.GetComponent<GameDirector>().getX() < 0.083)
                target = GameObject.FindWithTag("target (3)");
            else
                target = GameObject.FindWithTag("target");
            //다른 효과를 넣어야 함

            animator.SetBool("gamestate", true);
        }
        else
        {
            exerciseRight = false;
        }

        //운동이 정상적으로 시행된다면, 로켓의 준비 및 발사
        if(exerciseRight)
        {
            rocketLaunchReady = true;
        }

        if(rocketLaunchReady)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);

            if (animator.GetBool("kickstate") && animator.GetBool("gamestate"))
            {
                activeCameraOn();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.kick") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.83f)
            {
                animator.SetBool("kickstate", false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            if(animator.GetBool("kickstate") == false && animator.GetBool("gamestate") == true)
            {
                subCameraOn();

                Launch(new Vector3(target.transform.position.x, target.transform.position.y + 8, target.transform.position.z), speed); //target + 알파 로 보냄

                if (exerciseRight == false && timer_check < 4)
                {
                    //로켓을 폭발시키자
                    GameObject newExplosion = Instantiate(explosion, this.transform.position, this.transform.rotation) as GameObject;
                    rocketLaunchReady = false;
                }
                else if (exerciseRight == false && timer_check >= 4 && timer_check <= 6)
                {
                    Launch(target.transform.position, speed);
                }
            }
        }
        else
        {
            mainCameraOn();
            transform.position = rocket_init_pos;
            transform.rotation = rocket_init_rot;
            player.transform.position = player_init_pos;
            timer = 0;
            target = GameObject.FindWithTag("target");
            exerciseRight = false;
            animator.SetBool("kickstate", true);
            animator.SetBool("gamestate", false);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if(timer >= 6)
        {
            mainCameraOn();
        }

        if(timer >= 10)
        {
            rocketLaunchReady = false;
        }
    }

    private void Launch(Vector3 posit, float speed)
    {
        Vector3 pos = transform.position;
        Vector3 target_pos = posit;

        float dist = Vector3.Distance(pos, target_pos);

        transform.LookAt(target_pos);

        float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / Mathf.Sin(Mathf.Deg2Rad * angle * 2));
        float Vy, Vz;
        Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * angle);
        Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * angle);

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);

        Vector3 globalVelocity = transform.TransformVector(localVelocity);

        if(timer < 6)
            GetComponent<Rigidbody>().velocity = globalVelocity * Time.deltaTime * speed;
        else if(timer >= 6)
            GetComponent<Rigidbody>().velocity = globalVelocity * Time.deltaTime * speed * 5;
    }

    public void mainCameraOn()
    {
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
        activeCamera.SetActive(false);
    }

    public void subCameraOn()
    {
        mainCamera.SetActive(false);
        subCamera.SetActive(true);
        activeCamera.SetActive(false);
    }

    public void activeCameraOn()
    {
        mainCamera.SetActive(false);
        subCamera.SetActive(false);
        activeCamera.SetActive(true);
    }

    public bool getRocketLaunchReady()
    {
        return rocketLaunchReady;
    }

    public void setRocketLaunchReady(bool ready)
    {
        rocketLaunchReady = ready;
    }
}
