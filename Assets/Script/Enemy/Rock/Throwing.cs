using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//덤벨 던지는 스크립트
public class Throwing : MonoBehaviour
{
     Transform m_Target;
    public float m_InitialAngle = 30f; // 처음 날라가는 각도
    private Rigidbody m_Rigidbody;

    float throwTime;
    
    Transform ThrowTransform; //생성된 덤벨 고정위치
    bool throwing;

    [SerializeField]
    GameObject Effect;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        throwTime = Time.time;//시간 초기화
        Destroy(gameObject, 3f);
    }
    //위치값 함수로 받아옴
    public void setTransform(Transform _ThrowTransform)
    {
        ThrowTransform = _ThrowTransform;
    }
    void Update()
    {
        //0.5초전까진 고정된위치 따라가게함
        if(Time.time - throwTime < 0.5f)
        {
            transform.position = ThrowTransform.position;
            
        }
        //0.5초후에 플레이어 위치값을 받아와 플레이어에게 던짐
        else if(Time.time - throwTime >=0.5f && !throwing)
        {
            m_Target = GameObject.FindGameObjectWithTag("Player").transform;
            throwing = true;
            Vector3 velocity = GetVelocity(transform.position, m_Target.position, m_InitialAngle);
            m_Rigidbody.velocity = velocity;

        }
        

    }
    private void FixedUpdate()
    {
        //던질때 덤벨 회전
        if (Time.time - throwTime > 0.5f && gameObject.layer == 14)
        {
            transform.Rotate(new Vector3(5, 5, 5));

        }
    }

    public Vector3 GetVelocity(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && gameObject.layer == 14)
        {
            collision.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(14, 18), transform.position, 15);
            Instantiate(Effect, collision.transform.transform.position, Quaternion.identity);
            SoundManager.instance.Play("Hit");

            gameObject.layer = 15;
        }
        if ((collision.transform.tag == "Wall" || collision.gameObject.layer == 17) && gameObject.layer == 14&& Time.time - throwTime >= 0.5f)
        {
            SoundManager.instance.Play("Skill");

            gameObject.layer = 15;
            Instantiate(Effect, transform.position, Quaternion.identity);
        }
    }


}
