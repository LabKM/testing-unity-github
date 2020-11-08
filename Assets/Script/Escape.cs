using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>() != null && GameObject.Find("Friend") != null)
        {
            GameObject.FindGameObjectWithTag("Comment").GetComponent<CommentManager>().saySomething(transform.position, "You successly escape", 20, Color.Lerp(Color.red, Color.blue, 0.3f));
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                Destroy(enemy.gameObject);
                Debug.Log("TESTSTDSFSDFSDGSDG");
            }
            GameObject.Find("Option").GetComponent<Option>().GameClear();
        }
    }
}
