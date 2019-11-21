using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private float hoverTime = 0.0f;
    private float maxHoverTime = 3.0f;
    private GameObject handObj, targetObj;
    private Collider2D targetObjCollider;
    private bool isHovering = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        hoverTime = 0.0f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hoverTime += Time.deltaTime;
        if (hoverTime > maxHoverTime)
        {
            handObj = collision.gameObject;
            targetObj = gameObject;
            isHovering = true;
            Debug.Log(collision.gameObject.name + "attached to " + gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isHovering = false;
        hoverTime = 0.0f;
    }

    private void Update()
    {
        if (isHovering)
        {
            targetObjCollider = targetObj.GetComponent<Collider2D>();
            targetObjCollider.enabled = false;
            targetObj.transform.position = handObj.transform.position;
        }
    }
}
