using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    LineRenderer tail;
    Vector3 tail_end = new Vector3(0, 0, 0);
    float len_tail = 5.0f;
    float SpeedPerSecond = 60.0f;
    float SpeedPerFrame;

    RaycastHit raycastHit;
    void Start()
    {
        Destroy(this.gameObject, 3.0f);
        tail = transform.GetChild(1).GetComponent<LineRenderer>();
        tail.SetPosition(1, tail_end);
        StartCoroutine(ManageTail());
    }

    private void FixedUpdate()
    {
        SpeedPerFrame = SpeedPerSecond * Time.fixedDeltaTime;
        RaycastToEnemy();
        transform.Translate(transform.forward * SpeedPerFrame, Space.World);
    }

    IEnumerator ManageTail()
    {
        while (tail_end.z > -len_tail)
        {
            tail_end.z += SpeedPerFrame * -0.5f;
            tail.SetPosition(1, tail_end);
            yield return null;
        }
    }

    void RaycastToEnemy()
    {
        if(Physics.SphereCast(transform.position, 0.2f, transform.forward, out raycastHit, SpeedPerFrame))
        {
            IDamageable damagealbe_object = raycastHit.transform.GetComponent<IDamageable>();
            if (damagealbe_object != null)
            {
                damagealbe_object.TakeHit(1, raycastHit);
                GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position + Vector3.up, "hit!", 5, Color.black);
            }
            Destroy(this.gameObject);
        }
    }
}
