using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// end button is the button that appears after the game round is finished
// when player hovers over it it should start the game over
public class EndButtonScript : MonoBehaviour
{
    private float hoverTime = 0;
    private bool isHovering = false;
    private float maxHoverTime = 3.0f;
    public Canvas endGameCanvas, cardCanvas;

    // newScale is the maximum scaling of the button when player hovers over the button
    private Vector2 newScale = new Vector2(20f, 20f);
    // normalScale is the scale of the button when left untouched by the player
    private Vector2 normalScale = new Vector2(15f, 15f);
    private float speed = 3.0f;

    // triggered when another 2D trigger collider enters the object collider
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // when the collided object's tag is handObject
        // start the colliding event by setting 'isHovering' to 'true'
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
        // the hovering event
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
