using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//KnightHealth와 동일
public class SpearHealth : MonoBehaviour
{
    Vector3 startPos;

    public int startingHealth = 100;
    public int currentHealth;

    public  bool Dead = false;

    Rigidbody rigid;
    Animator ani;
    NavMeshAgent nav;

    Spear spear;

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
        spear = GetComponent<Spear>();
        nav = GetComponent<NavMeshAgent>();
        hpSlider.maxValue = startingHealth;
        startPos = transform.position;


    }

    // Update is called once per frame
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

        if (currentHealth <= 0 && !Dead)
        {
            Die();
        }
        else
        {
            if (Time.time - spear.attackTime > 0.2f && !Dead)
            {
                ani.Rebind();

                ani.SetTrigger("Hit");
                Vector3 diff = transform.position - EnemyPosition;

                diff = diff / diff.sqrMagnitude;
                rigid.AddForce((new Vector3(diff.x, diff.y, diff.z)) * pushBack, ForceMode.Impulse);
            }
 
          
        }
    }
    void PlayerDie()
    {
        PlayerTarget.targetOn = false;

        StatManager.instance.getEXP(20);
        ani.Rebind();
        spear.enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        ani.SetTrigger("Death");
        Destroy(gameObject, 2f);
        spear.CancelInvoke("NavOn");

        Dead = true;
        DropItem();
    }
    void Die()
    {
        PlayerTarget.targetOn = false;

        StatManager.instance.getEXP(20);
        ani.Rebind();
        ani.SetTrigger("Death");
        spear.CancelInvoke("NavOn");
        spear.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("enemyActive", 2);



        Dead = true;
        DropItem();
    }
    public void Deactive()
    {
        PlayerTarget.targetOn = false;
        spear.CancelInvoke("NavOn");
        spear.enabled = false;
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
        spear.enabled = true;
        Dead = false;
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
        GetComponent<BoxCollider>().enabled = true;
        gameObject.layer = 12;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
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
