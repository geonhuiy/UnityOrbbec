using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(SpriteRenderer))]
public class CardCollision : MonoBehaviour
{
    [SerializeField]
    private int cardNum;
    void Awake()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CardManager.instance.selectedCard == 0)
        {
            CardManager.instance.selectedCard = cardNum;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (cardNum == CardManager.instance.selectedCard) {
            CardManager.instance.selectedCard = 0;
        }
    }
}
