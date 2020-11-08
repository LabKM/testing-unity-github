using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpeechBubble : MonoBehaviour
{
    public float LifeTime = 1f;
    float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        if (birthTime + LifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
