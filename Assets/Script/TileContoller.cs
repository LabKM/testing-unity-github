using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileContoller : MonoBehaviour
{
    private Renderer renderUnit;
    Color colorDefault;

    // Start is called before the first frame update
    void Awake()
    {
        renderUnit = GetComponent<Renderer>();
        colorDefault = renderUnit.material.color;
    }

    float flick_per_sec = 6f;
    public IEnumerator Flickering(Enemy enemy)
    {
        float t = 0;
        while(t <= 1f) {
            t += Time.deltaTime;
            renderUnit.material.color = Color.Lerp(colorDefault, Color.red, System.Math.Abs((float)System.Math.Sin(System.Math.PI * t * flick_per_sec)));
            yield return null;
        }
        renderUnit.material.color = colorDefault;
        enemy.Spawn(transform.position);
    }

    public void ChangeColor()
    {
        if (renderUnit.material.color == colorDefault)
        {
            renderUnit.material.color = Color.red;
        }
        else
        {
            renderUnit.material.color = colorDefault;
        }
    }
}
