using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonClick : MonoBehaviour {

    private float gazeTimer;
    private Vector3 ScreenCenter;

    // Use this for initialization
    void Start()
    {
        gazeTimer = 0.0f;
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            if (hit.transform.tag == "button")
            {
                gazeTimer += 1.0f / 3.0f * Time.deltaTime;
                Debug.Log(gazeTimer);
                if (gazeTimer >= 1)
                {
                    GetComponent<move_scene>().SceneToGame2();
                    gazeTimer = 0;
                }
            }
        }
        else
        {
            gazeTimer = 0;
        }
    }
}
