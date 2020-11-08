using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPrinter : MonoBehaviour
{
    // Start is called before the first frame update
    Sprite[] hpBars;
    Player player;
    float maxHp;
    float hp;

    const int stateStair = 5;
    const int barNum = 4;
    int[] hpBarState;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxHp = player.MaxHp;
        hp = player.hp;
        hpBarState = new int[4];
        hpBars = new Sprite[stateStair];
        for (int i = 0; i < stateStair; ++i)
        {
            string barName = "HP_" + (i + 1).ToString();
            hpBars[i] = transform.GetChild(barNum + i).GetComponent<SpriteRenderer>().sprite;
        }
        configHpBarState();
        configBarImage();
    }

    // Update is called once per frame
    void Update()
    {
        hp = Math.Max(0, player.hp);
        configHpBarState();
        configBarImage();
    }

    void configHpBarState()
    {
        float hpRate = (hp / maxHp) * stateStair * barNum;
        int fullBarCount = (int)hpRate / stateStair;
        int remBar = (int)hpRate - fullBarCount * stateStair;
        for(int i = fullBarCount; i > 0; --i)
        {
            hpBarState[barNum - i] = 0;
        } // full bar
        if (barNum - fullBarCount - 1 >= 0)
        {
            hpBarState[barNum - fullBarCount - 1] = stateStair - remBar;
        } // remainder bar 
        for(int i = 0; i < barNum - fullBarCount - 1; ++i)
        {
            hpBarState[i] = stateStair;
        } // empty bar
    }

    void configBarImage()
    {
        for(int i = 0; i < barNum; ++i)
        {
            if (hpBarState[i] < stateStair) {
                Image bar = transform.GetChild(i).GetComponent<Image>();
                bar.color = Color.white;
                bar.sprite = hpBars[hpBarState[i]];
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.clear;
            }
        }
    }
}
