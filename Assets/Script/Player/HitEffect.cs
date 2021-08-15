using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격에 맞추면 타격이펙트 나오게
public class HitEffect : MonoBehaviour
{
    [SerializeField]
    GameObject hitEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //타격이펙트 생성함수
    public void HitEffectOn(Transform enemy)
    {
        Instantiate(hitEffect, enemy.transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
    }
}
