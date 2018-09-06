﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour {

    public float x;
    private float y;
    public float mouse_x;
    private float mouse_y;
    public Material Mat;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;
    private bool random_check;
    private int random_number;
    private List<int> random_array = new List<int>();
    private int point; //점수

    // Use this for initialization
    void Start () {
        random_check = false;
        point = 0;
    }
	
	// Update is called once per frame
	void Update () {
        mouse_x = Input.mousePosition.x;
        mouse_y = Input.mousePosition.y;

        //플레이어 회전 구현
        // 코너 1 : -90~90, 코너 2 : 0~180 ....
        x = (Input.mousePosition.x - Screen.width / 2) / Screen.width; // x의 범위는 -0.5~ 0.5
        y = -(Input.mousePosition.y - Screen.height / 2) / Screen.height;

        // 회전반경 제한
        if (x < -0.5f) x = -0.5f;
        if (x > 0.5f) x = 0.5f;
        if (y < -0.5f) y = -0.5f;
        if (y > 0.5f) y = 0.5f;

        Quaternion turn = Quaternion.Euler(y * 180, x * 180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, turn, 7 * Time.deltaTime); // 부드럽게 턴


        //랜덤으로 과녁을 선택
        if (random_check == false)
        {
            random_number = randFunc();
            random_check = true;
        }

        if(random_number == 1)
        {
            GameObject child_tar = GameObject.FindWithTag("target").transform.GetChild(1).gameObject;
            child_tar.GetComponent<Renderer>().material = Mat;
        }
        else if(random_number == 2)
        {
            GameObject child_tar = GameObject.FindWithTag("target (1)").transform.GetChild(1).gameObject;
            child_tar.GetComponent<Renderer>().material = Mat;
        }
        else if(random_number == 3)
        {
            GameObject child_tar = GameObject.FindWithTag("target (2)").transform.GetChild(1).gameObject;
            child_tar.GetComponent<Renderer>().material = Mat;
        }
        else if(random_number == 4)
        {
            GameObject child_tar = GameObject.FindWithTag("target (3)").transform.GetChild(1).gameObject;
            child_tar.GetComponent<Renderer>().material = Mat;
        }
        else if(random_number == 0)
        {
            GameObject child_tar = target1.transform.GetChild(1).gameObject;
        }
    }

    public float getX()
    {
        return x;
    }

    public void setX(float other)
    {
        x = other;
    }

    public int randFunc()
    {
        int rand_num = UnityEngine.Random.Range(1, 5);

        if (random_array.Contains(rand_num))
        {
            if (random_array.Count >= 4)
                rand_num = 0;
            else
                rand_num = randFunc();
        }
        else
            random_array.Add(rand_num);

        return rand_num;
    }

    public void setRandom_check(bool other)
    {
        random_check = other;
    }

    public int getRandom_number()
    {
        return random_number;
    }

    //추가한 부분
    public int getPoint()
    {
        return point;
    }

    public void setPoint(int po)
    {
        point = po;
    }

    public void setPlusPoint(int po)
    {
        point = point + po;
    }
}
