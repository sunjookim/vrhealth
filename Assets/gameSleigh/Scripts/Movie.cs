using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gameUI오브젝트에서 실행함
public class Movie : MonoBehaviour { 

    // 처음 시작 시 초기화 영상 재생
    void Start()
    {
        GameObject obj = Instantiate(Resources.Load("Initializing")) as GameObject; // 초기화 영상 재생
    }
}
