using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 체력 계속 카메라를 바라보게 하는 스크립트
public class BillBoard : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }


    void Update()
    {
        //몬스터가 회전하면 체력슬라이더도 같이 회전하는데
        //슬라이더를 카메라를 바라보게하면 수정가능
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }
}
