using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    private float hoverTime = 0;
    private float maxHoverTime = 2;
    public Text answer;
    public Text display;

    public GameObject camera;

    private GameObject handObj, targetObj;
    private Collider2D targetObjCollider;
    private bool isHovering = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        hoverTime = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hoverTime += Time.deltaTime;

        if (hoverTime > maxHoverTime)
        {
            if (collision.gameObject.tag == "object")
            {
                if (collision.gameObject.name == answer.text || collision.gameObject.name == answer.text + "(Clone)")
                {
                    display.text = "correct answer";
                    OnCorrectAnswer();
                }
                else
                {
                    display.text = "incorrect answer";
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isHovering = false;
        hoverTime = 0;
        display.text = "try again";
    }

    // private void Update()
    // {
    //     // if (isHovering)
    //     // {
    //     //     targetObjCollider = targetObj.GetComponent<Collider2D>();
    //     //     targetObjCollider.enabled = false;
    //     //     targetObj.transform.position = handObj.transform.position;
    //     // }
    // }

    private void OnCorrectAnswer()
    {
        PrefabLoader name = camera.GetComponent<PrefabLoader>();
        for (int i = 0; i < name.activeCards.Count; i++)
        {
            GameObject cloneObjects = name.activeCards[i];
            Debug.Log(cloneObjects.name);
            Destroy(cloneObjects);
        }
    }

    private void OnWrongAnswer()
    {

    }
}
