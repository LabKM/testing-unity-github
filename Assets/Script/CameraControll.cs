using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private GameObject target;
    private UnityEngine.Vector3 subTarget;
    private delegate void movement();
    private movement camera_walk;
    // Start is called before the first frame update

    private void Awake()
    {
        camera_walk = track;
    }
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        subTarget = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        camera_walk();
    }

    private void track()
    {
        UnityEngine.Vector3 target_pos = target.transform.position + subTarget;
        float dist = (target_pos - transform.position).magnitude;
        transform.position = UnityEngine.Vector3.Lerp(transform.position, target_pos, dist * Time.deltaTime);
        if (dist <= 0)
        {
            camera_walk = stand;
        }
    }

    private void stand()
    {
        UnityEngine.Vector3 vector_target = target.transform.position - transform.position;
        if (vector_target.magnitude > 5)
        {
            camera_walk = track;
        }
    }
}
