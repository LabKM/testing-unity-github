using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Prison : MonoBehaviour, IDamageable
{
    public int MaxDurability;
    float durability;

    public bool isBroken { private set; get; }
    // Start is called before the first frame update
    GameObject prisoner_prefab;

    void Awake()
    {
        prisoner_prefab = Resources.Load<GameObject>("Prefab/Friend");
    }

    void Start()
    {
        durability = MaxDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBroken == true)
        {
            freePrisoner();
        }
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeHit(damage);
    }
    public void TakeHit(float damage)
    {
        durability -= damage;

        if (durability <= 0)
        {
            isBroken = true;
        }
    }

    public void Heal(float healPoint)
    {
    }

    void freePrisoner()
    {
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject prisoner = Instantiate(prisoner_prefab, player.transform.position, Quaternion.identity);
        prisoner.GetComponent<Friend>().Free();
    }
}
