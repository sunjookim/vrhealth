using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject Target;
    public float dist = 10f;
    public float height= 5f;
    public float smoothRotate = 5.0f;

    Vector3 cameraPo;
    private void LateUpdate()
    {
        float currYAngle = Mathf.LerpAngle(transform.eulerAngles.y, Target.transform.eulerAngles.y, smoothRotate * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

        transform.position = Target.transform.position - (rot * Vector3.forward * dist) + (Vector3.up * height);

        transform.LookAt(Target.transform);
        //float x = transform.rotation.x;
        transform.Rotate(-20, 0, 0);
    }
}
