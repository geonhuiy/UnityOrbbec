using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class CardCollision : MonoBehaviour
{
    [SerializeField]
    private int cardNum;
    private bool isHovering = false;
    private Vector2 newScale = new Vector2(25.00f, 25.00f);
    private Vector2 normalScale = new Vector2(20.363f, 20.363f);
    private float speed = 3.0f;
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
        if (cardNum == CardManager.instance.selectedCard)
        {
            CardManager.instance.selectedCard = 0;
        }

    }

    // Keep updating the scale of a card objects according to the isHovering status in each frame
    private void Update()
    {
        //Vector2 normalScale = new Vector2(transform.position.x, transform.position.y);

        if (isHovering)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, newScale, speed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector2.Lerp(transform.localScale, normalScale, speed * Time.deltaTime);
        }
        if (isHovering)
        {
            hoverTime += Time.deltaTime;
            //Debug.Log(hoverTime);

            // when hover time goes over max hover time, stop checking for hover and start the check 
            // answer progress, the stop is to stop the check answer function getting called repeatedly
            if (hoverTime >= maxHoverTime)
            {
                isHovering = false;
                hoverTime = 0;
                CardManager.instance.CheckAnswer();
            }
        }


    }
}
