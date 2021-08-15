using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//KnightHealth 스크립트와 거의동일
public class RockHealth : MonoBehaviour
{
    //시작위치
    Vector3 startPos;
    //시작체력
    public int startingHealth = 100;
    //현재체력
    public int currentHealth;

    public bool Dead = false;

    Rigidbody rigid;
    Animator ani;
    NavMeshAgent nav;

    Rock rock;
    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject hpShake;
    [SerializeField]
    GameObject mpShake;
    int randomItem;
    int randomCoin;
    void Start()
    {
        currentHealth = startingHealth;
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        rock = GetComponent<Rock>();
        nav = GetComponent<NavMeshAgent>();
        hpSlider.maxValue = startingHealth;
        startPos = transform.position;

    }


    void Update()
    {
        hpSlider.value = currentHealth;
    }

    public void TakeDamage(int amout, Vector3 EnemyPosition, float pushBack)
    {
        if (!hpSlider.gameObject.activeSelf)
            hpSlider.gameObject.SetActive(true);
        //체력깎음
        currentHealth -= amout;
        //player.hitTime = Time.time;
        if (Time.time - rock.attackTime > 2f && !Dead)
        {
            ani.Rebind();

            ani.SetTrigger("Hit");

        }



        if (currentHealth <= 0 && !Dead)
        {

            Die();
        }
        else
        {
            Vector3 diff = transform.position - EnemyPosition;

            diff = diff / diff.sqrMagnitude;
            rigid.AddForce((new Vector3(diff.x, diff.y, diff.z)) * pushBack, ForceMode.Impulse);

        }
    }

    void Die()
    {
        PlayerTarget.targetOn = false;

        StatManager.instance.getEXP(40);
        ani.Rebind();
        ani.SetTrigger("Death");
        rock.CancelInvoke("NavOn");
        rock.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("enemyActive", 2);
        Dead = true;
        DropItem();
    }
    public void Deactive()
    {
        PlayerTarget.targetOn = false;
        Destroy(rock.throwDumbble);//던지려고햇던 or 날아가던 덤벨도 삭제
        rock.CancelInvoke("NavOn");
        rock.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        enemyActive();
        Dead = true;
        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
    }
    public void enemyRevive()
    {
        ani.Rebind();
        rock.enabled = true;
        Dead = false;
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
        GetComponent<BoxCollider>().enabled = true;
        gameObject.layer = 12;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        hpSlider.gameObject.SetActive(false);

        currentHealth = startingHealth;
        transform.position = startPos;

    }
    void enemyActive()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }
    void DropItem()
    {
        randomItem = Random.Range(0, 4);
        randomCoin = Random.Range(0, 3);
 

        GameObject coinPrefeb1;
        GameObject coinPrefeb2;
        GameObject coinPrefeb3;

        GameObject hpPrefeb;
        GameObject mpPrefeb;
        switch (randomItem)
        {
            case 0:
                break;
            case 1:
                hpPrefeb = Instantiate(hpShake, transform.position + new Vector3(0.3f, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
                hpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                break;
            case 2:
                mpPrefeb = Instantiate(mpShake, transform.position + new Vector3(0.3f, 0.2f, 0f), Quaternion.Euler(-90, 0, 0));
                mpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            case 3:
                hpPrefeb = Instantiate(hpShake, transform.position + new Vector3(0.3f, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
                hpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                mpPrefeb = Instantiate(mpShake, transform.position - new Vector3(0.4f, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
                mpPrefeb.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                break;
            default:
                break;


        }
        switch (randomCoin)
        {
            case 0:
                coinPrefeb1 = Instantiate(coin, transform.position, Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            case 1:
                coinPrefeb1 = Instantiate(coin, transform.position + new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb2 = Instantiate(coin, transform.position - new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb2.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            case 2:
                coinPrefeb1 = Instantiate(coin, transform.position + new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb1.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb2 = Instantiate(coin, transform.position, Quaternion.identity);
                coinPrefeb2.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                coinPrefeb3 = Instantiate(coin, transform.position - new Vector3(0, 0.2f, 0.3f), Quaternion.identity);
                coinPrefeb3.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

                break;
            default:
                break;

        }
    }
}
