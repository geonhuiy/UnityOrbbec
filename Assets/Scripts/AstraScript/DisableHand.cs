using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHand : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    private Collider2D leftCollider, rightCollider;
    private bool leftHovering, rightHovering = false;
    // Update is called once per frame
    void Update()
    {
        HoverCheck();
        if (leftHovering) {
            rightCollider.enabled = false;
        }
        else if (rightHovering) {
            leftCollider.enabled = false;
        }
    }

    void HoverCheck() {
        leftCollider = leftHand.GetComponent<Collider2D>();
        rightCollider = rightCollider.GetComponent<Collider2D>();

        if (leftCollider.enabled == true) {
            leftHovering = true;
            rightHovering = false;
        }
        else if (rightCollider.enabled == true) {
            rightHovering = true;
            leftHovering = false;
        }
    }
}
