using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Friend : LivingEntity
{
    // Start is called before the first frame update
    NavMeshAgent m_Agent;
    Behavior action;
    GameObject companion;
    public override void Start()
    {
        base.Start();
        action = new Behavior(idle);
    }

    // Update is called once per frame
    void Update()
    {
        action();
    }

    public void Free()
    {
        name = "Friend";
        m_Agent = GetComponent<NavMeshAgent>();
        companion = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position + Vector3.up, "Oh, Thanks My Friend\nlet's escape, follow me", 10, Color.green);
        last_configured_to_action = Time.time + 1f;
    }
    float last_configured_to_action = 0;
    void follow()
    {
        if (last_configured_to_action + 0.1f < Time.time )
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount < 5) {
                last_configured_to_action = Time.time + 1f;

            }
            else
            {
                action = new Behavior(idle);
                last_configured_to_action = Time.time + 0.5f;
            }
        }
    }

    new void idle()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (last_configured_to_action < Time.time)
        {
            if (enemyCount < 5)
            {
                GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position + Vector3.up, "Let's run away", 10, Color.green);
                action = new Behavior(follow);
                m_Agent.isStopped = false;
                m_Agent.destination = GameObject.Find("Escape").transform.position;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position + Vector3.up,
                    "i'm so scare!\nToo Many Enemies", 10, Color.green);
                last_configured_to_action = Time.time + 1f;
                m_Agent.isStopped = true;
            }
        }
    }
}
