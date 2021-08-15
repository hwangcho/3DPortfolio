using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 몬스터 활성화 or 비활성화
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;
    [SerializeField]
    SkeletonHealth[] skeletonHealth;
    [SerializeField]
    RockHealth[] rockHealth;
    [SerializeField]
    KnightHealth[] knightHealth;
    [SerializeField]
    SpearHealth[] spearHealth;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        
    }


    //모든 몬스터 비활성화
    public void AllOFF()
    {
        for (int i = 0; i < skeletonHealth.Length; i++)
        {
            skeletonHealth[i].Deactive();
        }
        for (int i = 0; i < rockHealth.Length; i++)
        {
            rockHealth[i].Deactive();
        }
        for (int i = 0; i < knightHealth.Length; i++)
        {
            knightHealth[i].Deactive();
        }
        for (int i = 0; i < spearHealth.Length; i++)
        {
            spearHealth[i].Deactive();
        }
    }
    //모든 몬스터 활성화
    public void AllRespawn()
    {
        for (int i = 0; i < skeletonHealth.Length; i++)
        {
            skeletonHealth[i].enemyRevive();
        }
        for (int i = 0; i < rockHealth.Length; i++)
        {
            rockHealth[i].enemyRevive();
        }
        for (int i = 0; i < knightHealth.Length; i++)
        {
            knightHealth[i].enemyRevive();
        }
        for (int i = 0; i < spearHealth.Length; i++)
        {
            spearHealth[i].enemyRevive();
        }
    }
}
