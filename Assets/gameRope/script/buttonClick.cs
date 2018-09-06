﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour {

    private float gazeTimer;
    private Vector3 ScreenCenter;
    public GameObject hpgaze;

    // Use this for initialization
    void Start()
    {
        gazeTimer = 0.0f;
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        hpgaze.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            //SceneToGame1()
            if (hit.transform.tag == "button_replaygame1")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToGame1();
                    gazeTimer = 0;
                }
            }

            //SceneToGame2()
            if (hit.transform.tag == "button_replay")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToGame2();
                    gazeTimer = 0;
                }
            }

            //SceneToGame3()
            if (hit.transform.tag == "button_replaygame3")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToGame3();
                    gazeTimer = 0;
                }
            }

            //SceneChange2()
            if (hit.transform.tag == "button_back")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneChange2();
                    gazeTimer = 0;
                }
            }

            //Pause
            if (hit.transform.tag == "button_menu")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().Pause();
                    gazeTimer = 0;
                }
            }

            //SceneToGame2_Play()
            if (hit.transform.tag == "button_start")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToGame2_Play();
                    gazeTimer = 0;
                }
            }

            //SceneToManual()
            if (hit.transform.tag == "button_manuel")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToManual();
                    gazeTimer = 0;
                }
            }

            //Exit()
            if (hit.transform.tag == "button_exit")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().Exit();
                    gazeTimer = 0;
                }
            }

            //Exit()
            if (hit.transform.tag == "button_exit")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().Exit();
                    gazeTimer = 0;
                }
            }

            //SceneChange1()
            if (hit.transform.tag == "button_backgame")
            {
                hpgaze.SetActive(true);
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                hpgaze.GetComponent<Image>().fillAmount -= 0.33f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneChange1();
                    gazeTimer = 0;
                }
            }
        }
        else
        {
            gazeTimer = 0;
            hpgaze.SetActive(false);
            hpgaze.GetComponent<Image>().fillAmount = 1;
        }
    }
}
