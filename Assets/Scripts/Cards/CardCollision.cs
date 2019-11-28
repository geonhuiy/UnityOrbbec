using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class CardCollision : MonoBehaviour
{
    [SerializeField]
    private int cardNum;
    private bool isHovering = false;
    private Vector3 newScale = new Vector3(0.65f, 0.65f, 0.65f);
    private Vector3 normalScale = new Vector3(0.5f, 0.5f, 1f);
    private float speed = 2.0f;
    private float hoverTime = 0;
    private float maxHoverTime = 2.0f;

    void Awake()
    {
        /* Ideally, a size of box collider in the card could be change to card size dynamically?*/
    }

    /* When a Collider 2D (mouse for the test, hands from the camera) hits Box Collider 2D in cards
    isHovering becomes true*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        hoverTime = 0;
        isHovering = true;
        Debug.Log("Trigger entered");
        if (CardManager.instance.selectedCard == 0)
        {
            CardManager.instance.selectedCard = cardNum;
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        hoverTime += Time.deltaTime;

    }*/

    // When the collision ends between a mouse(for test) or hands, isHovering becomes false 
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exit");
        isHovering = false;
        hoverTime = 0;
        if (cardNum == CardManager.instance.selectedCard) {
            CardManager.instance.selectedCard = 0;
        }

    }

    // Keep updating the scale of a card objects according to the isHovering status in each frame
    private void Update()
    {
        /*if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, speed * Time.deltaTime);

            if (hoverTime > maxHoverTime)
            {
                Vector3 theScale = transform.localScale;

                theScale.x *= -1;

                transform.localScale = theScale;
            }

        }
        else
        {
            //transform.localScale = Vector3.Lerp(transform.localScale, normalScale, speed * Time.deltaTime);
        }*/
        if(isHovering) {
            hoverTime += Time.deltaTime;
            Debug.Log(hoverTime);

            // when hover time goes over max hover time, stop checking for hover and start the check 
            // answer progress, the stop is to stop the check answer function getting called repeatedly
            if(hoverTime >= maxHoverTime) {
                isHovering = false;
                hoverTime = 0;
                CardManager.instance.CheckAnswer();
            }
        }

        
    }
}
