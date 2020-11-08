using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Enemy : LivingEntity
{
    Player target;
    NavMeshAgent m_Agent;
    Behavior action;

    float last_configured_to_action = 0;
    public float TracePlayerPerMin = 20;
    public float WaitTimeToAction = 1f;
    public float AttackAnimationTime = 0.1f;
    float attackDistanceThreshold = 1.5f;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        action();
    }

    public void Spawn(Vector3 pos)
    {
        transform.position = pos;
        transform.rotation = Quaternion.identity;
        transform.gameObject.SetActive(true);

        m_Agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").GetComponent<Player>();
        action = new Behavior(trace);
        m_Agent.destination = target.transform.position;
    }

    Renderer myMaterial;
    Color defaultColor;
    void trace()
    {
        if ((transform.position - target.transform.position).magnitude < attackDistanceThreshold)
        {
            action = new Behavior(attack);
            last_configured_to_action = Time.time;
            m_Agent.isStopped = true;
            myMaterial = transform.Find("Body").GetComponent<Renderer>();
            defaultColor = myMaterial.material.color;
        }
        else if (last_configured_to_action + (60 / TracePlayerPerMin) < Time.time)
        {
            last_configured_to_action = Time.time;
            m_Agent.destination = target.transform.position;
        }
    }

    static string[] attack_comment = "I will break you\n your friend is not here\n I hate you\n go away\n assshole!\n i will kill all of you".Split('\n');

    void attack()
    {
        float s = (last_configured_to_action + WaitTimeToAction - Time.time) / WaitTimeToAction;
        myMaterial.material.color = Color.Lerp(defaultColor, Color.red, s);
        if (last_configured_to_action + WaitTimeToAction < Time.time)
        {
            transform.position += transform.forward * attackDistanceThreshold / AttackAnimationTime * Time.deltaTime;
            if (last_configured_to_action + WaitTimeToAction + AttackAnimationTime < Time.time)
            {
                GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position + Vector3.up, attack_comment[UnityEngine.Random.Range(0, attack_comment.Length)], 10, Color.yellow);
                last_configured_to_action = Time.time;
                if ((transform.position - target.transform.position).magnitude < 1.0f)
                {
                    target.GetComponent<IDamageable>().TakeHit(1);
                }
                action = new Behavior(trace);
                m_Agent.isStopped = false;
                transform.Find("Body").GetComponent<Renderer>().material.color = defaultColor;
            }
        }
        else
        {
            transform.position += transform.forward * -attackDistanceThreshold / 2 / WaitTimeToAction * Time.deltaTime;
        }
    }
    protected override void Die()
    {
        dead = true;
        Destroy(gameObject);
    }
}
