using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float hoverTime = 0;
    private bool isHovering = false;
    private float maxHoverTime = 3.0f;
    public Canvas endGameCanvas, cardCanvas;
    // Start is called before the first frame update

    private Vector2 newScale = new Vector2(20f, 20f);
    private Vector2 normalScale = new Vector2(15f, 15f);
    private float speed = 3.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "handObject")
        {
            isHovering = true;
        }
        hoverTime = 0;
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        isHovering = false;
    }

    void Update()
    {

        if (isHovering)
        {
            hoverTime += Time.deltaTime;
            transform.localScale = Vector2.Lerp(transform.localScale, newScale, speed * Time.deltaTime);
            //Debug.Log(hoverTime);

            // when hover time goes over max hover time, stop checking for hover and start the check 
            // answer progress, the stop is to stop the check answer function getting called repeatedly
            if (hoverTime >= maxHoverTime)
            {
                isHovering = false;
                hoverTime = 0;
                endGameCanvas.gameObject.SetActive(false);
                cardCanvas.gameObject.SetActive(true);
                CardManager.instance.ResetGame();
                //TutorialManager.instance.ResetTutorial();

            }
        }
        else
        {
            transform.localScale = Vector2.Lerp(transform.localScale, normalScale, speed * Time.deltaTime);
        }
    }
}
