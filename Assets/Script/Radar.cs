using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public float RadarAnimatePerSecond = 60;
    public float breadthRadarAnimatePerSecond = 0.5f;
    Text text;

    Player player;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = GameObject.Find("Prison").transform;
    }

    // Update is called once per frame
    void Update()
    {
        animateRadar();
    }

    float distanceLevel = 1;
    float animate_param = 0;
    float breathCount = 0;
    delegate void Animate();
    Queue<Animate> aniQueue = new Queue<Animate>();
    void animateRadar()
    {
        animate_param += Time.deltaTime;
        breathCount += Time.deltaTime;
        if (target != null)
        {
            distanceLevel = (player.transform.position - target.transform.position).magnitude / 10;
        }
        text.color = Color.Lerp(Color.white, Color.black, Utility.linearSin(breathCount * breadthRadarAnimatePerSecond));
        if (animate_param > (1 / RadarAnimatePerSecond * distanceLevel))
        {
            if (aniQueue.Count == 0)
            {
                //전부 소진시 새 애니메이션 큐 삽입
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(minusRadarSignal));
                aniQueue.Enqueue(new Animate(plusRadarSignal));
            }
            //애니메이션 큐 소모
            aniQueue.Dequeue()();
            animate_param = 0;
        }
    }

    void plusRadarSignal()
    {
        text.text += "||";
    }

    void minusRadarSignal()
    {
        if (text.text.Length > 0)
        {
            text.text = text.text.Substring(0, text.text.Length - 2);
        }
    }

    void configureDistance()
    {
    }
}