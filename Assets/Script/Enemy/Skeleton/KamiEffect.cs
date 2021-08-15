using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamiEffect : MonoBehaviour
{
    SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        StartCoroutine(colliderOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator colliderOff()
    {
        yield return new WaitForSeconds(0.1f);
        sphereCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(Random.Range(20, 28), transform.position,5);
        }
    }
}
