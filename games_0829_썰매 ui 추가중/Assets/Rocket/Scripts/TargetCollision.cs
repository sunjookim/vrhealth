using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour {

    public GameObject explosion;
    public GameObject rocket;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;
    public GameObject mainCamera;
    private int target_find;


	// Use this for initialization
	void Start () {
        //mainCamera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        if (this.tag == "target")
            target_find = 1;
        if (this.tag == "target (1)")
            target_find = 2;
        if (this.tag == "target (2)")
            target_find = 3;
        if (this.tag == "target (3)")
            target_find = 4;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "rocket" )
        {
            rocket.GetComponent<RocketMove>().mainCameraOn();
            rocket.GetComponent<RocketMove>().setRocketLaunchReady(false);

            GameObject newExplosion = 
                Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), this.transform.rotation) as GameObject;

            int rand_che = mainCamera.GetComponent<GameDirector>().getRandom_number();

            if(rand_che != target_find)
            {
                if (rand_che == 1)
                    Destroy(target1);
                if (rand_che == 2)
                    Destroy(target2);
                if (rand_che == 3)
                    Destroy(target3);
                if (rand_che == 4)
                    Destroy(target4);

                //GameObject newtarget = Instantiate(target, this.transform.position, this.transform.rotation) as GameObject;
            }
            else
            {
                Destroy(this.gameObject);
            }

            mainCamera.GetComponent<GameDirector>().setRandom_check(false);
        }
    }
}
