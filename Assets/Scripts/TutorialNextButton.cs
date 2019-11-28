using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNextButton : MonoBehaviour
{
    private float hoverTime = 0;
    private bool isHovering = false;
    private float maxHoverTime = 2;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "handObject") {
            isHovering = true;
        }
        hoverTime = 0;
    }

    void Update() {
        if(isHovering) {
            hoverTime += Time.deltaTime;
            Debug.Log(hoverTime);

            // when hover time goes over max hover time, stop checking for hover and start the check 
            // answer progress, the stop is to stop the check answer function getting called repeatedly
            if(hoverTime >= maxHoverTime) {
                isHovering = false;
                hoverTime = 0;
                CardManager.instance.ShowTutorial();
            }
        }
    }
}
