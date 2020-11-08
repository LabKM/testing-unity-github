using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class CommentManager : MonoBehaviour
{
    public GameObject bubble;
    
    public void saySomething(Vector3 start, string comment, float strength, Color color)
    {
        GameObject speechBubble = Instantiate(bubble, start + Vector3.up * 2, Quaternion.LookRotation(Camera.main.transform.position - start, Vector3.up));
        speechBubble.transform.rotation *= Quaternion.Euler(0, 180, 0);
        speechBubble.GetComponent<TextMeshPro>().text = comment;
        speechBubble.GetComponent<TextMeshPro>().fontSize = strength;
        speechBubble.GetComponent<TextMeshPro>().color = color;
    }

    static string[] helpComment = ("Help Me!|" +
    "I'm Here|" + "Keep Closer").Split('|');
    public void helpSign(Vector3 start)
    {
        Transform listener = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 point = Vector3.Lerp(listener.position, start, 0.5f); 
        GameObject speechBubble = Instantiate(bubble, point, Quaternion.LookRotation(Camera.main.transform.position - start, Vector3.up));
        speechBubble.transform.rotation *= Quaternion.Euler(0, 180, 0);
        speechBubble.GetComponent<TextMeshPro>().text = helpComment[UnityEngine.Random.Range(0, helpComment.Length)];
        speechBubble.GetComponent<TextMeshPro>().fontSize = 12;
        speechBubble.GetComponent<TextMeshPro>().color = Color.black;
    }
}
