using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Knight스크립트와 동일
public class Spear : MonoBehaviour
{

    public bool destinyOn;
    public bool targetOn;
    bool Attack;
    public float attackTime;
    NavMeshAgent nav;
    Animator ani;
    
    GameObject player;
    [SerializeField]
    Transform destiny1;
    [SerializeField]
    Transform destiny2;
    [SerializeField]
    BoxCollider attackCollider;

    SpearHealth spearhealth;
    
    Vector3 dir;
    void Awake()
    {
        attackTime = Time.time - 1f;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        spearhealth = GetComponent<SpearHealth>();
        
    }


    void Update()
    {
        if (!spearhealth.Dead)
        {

            OnTarget();
            if (!targetOn)
            {
                NoTarget();

            }
        }



    }
    void OnTarget()
    {
     
            if (Vector3.Distance(player.transform.position, transform.position) < 8 && Vector3.Distance(player.transform.position, transform.position) > 2.2f && Time.time - attackTime > 1.3f)
            {
                targetOn = true;
                nav.speed = 2;
                ani.SetBool("Walk", false);
                ani.SetBool("Run", true);
                Attack = false;

            }
            else if (Vector3.Distance(player.transform.position, transform.position) <= 2.2f && Time.time - attackTime > 1.3f)
            {
                ani.SetBool("Run", false);
                ani.SetBool("Walk", true);

                nav.speed = 0;


            dir = player.transform.position - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 4f * Time.deltaTime);


            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2.2f, LayerMask.GetMask("Player")))
                {
                    ani.SetBool("Walk", false);

                    if (Time.time - attackTime > 2.1f)
                    {
                        if (hit.transform.tag == "Player")
                        {
                            if (!Attack)
                            {

                                Attack = !Attack;
                                attackTime = Time.time;
                                ani.SetTrigger("Attack1");
                            }
                            else if (Attack)
                            {
                                Attack = !Attack;
                                attackTime = Time.time;
                                ani.SetTrigger("Attack2");
                            }
                        }
                    }

                }
             


            }
            else if (Vector3.Distance(player.transform.position, transform.position) > 8)
            {
                targetOn = false;
                ani.SetBool("Run", false);



            }

            if (targetOn)
            {
                nav.SetDestination(player.transform.position);
            }
        
        
    }
    void NoTarget()
    {
        if (destinyOn)
        {
            nav.speed = 1;
            nav.SetDestination(destiny1.transform.position);
            ani.SetBool("Walk", true);
        }
        else
         {
            nav.speed = 1;
            nav.SetDestination(destiny2.transform.position);
            ani.SetBool("Walk", true);


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destiny")
        {
            destinyOn = !destinyOn;
        }
        if(other.gameObject.layer == 11)
        {
            spearhealth.TakeDamage(Random.Range(StatManager.instance.p_attackMin, StatManager.instance.p_attackMax), player.transform.position,20);
            other.GetComponentInChildren<HitEffect>().HitEffectOn(this.transform);
            SoundManager.instance.Play("Hit");

        }

        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(9, 15), transform.position, 1);
            SoundManager.instance.Play("Hit");

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerDumbbel")
        {
            spearhealth.TakeDamage(Random.Range(StatManager.instance.Skill2Min, StatManager.instance.Skill2Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
        if (collision.gameObject.tag == "PlayerPlate")
        {
            spearhealth.TakeDamage(Random.Range(StatManager.instance.Skill1Min, StatManager.instance.Skill1Max), player.transform.position, 0);
            SoundManager.instance.Play("Hit");


        }
    }
    public void AttackON()
    {
        attackCollider.enabled = true;
        
    }
    public void AttackOFF()
    {
        attackCollider.enabled = false;

    }
    public void NoDamageOn()
    {
        nav.speed = 0;
        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.velocity = Vector3.zero;
        gameObject.layer = 8;
        AttackOFF();
        CancelInvoke("NavOn");
        Invoke("NavOn", 1f);
    }
    public void NoDamageOff()
    {
       
        gameObject.layer = 12;

    }

    public  void NavOn ()
    {
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
    }
    public void Atk1Sound()
    {
        SoundManager.instance.Play("EnemyAttack1");
    }
    public void Atk2Sound()
    {
        SoundManager.instance.Play("EnemyAttack2");
    }
}
