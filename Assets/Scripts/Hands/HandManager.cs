using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [SerializeField]
    private GameObject leftHand, rightHand;
    private List<GameObject> cardObjects;


    void Awake()
    {
        //Ensures that only one instance of HandManager exists
        if (instance != null)
        {
            //Destroy instance if HandManager exists
            Destroy(gameObject);
        }
        //Set current to HandManager
        instance = this;
        DontDestroyOnLoad(this);

    }

    private void checkCollidingHand()
    {
        //Gets collider components for each hand object
        Collider2D leftCollider = leftHand.GetComponent<Collider2D>();
        Collider2D rightCollider = rightHand.GetComponent<Collider2D>();

        //Finds objects with tag 'card'
        cardObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("card"));
        foreach (var i in cardObjects)
        {
            //Disable right hand collider if left is touching a card
            if (leftCollider.IsTouching(i.GetComponent<Collider2D>()))
            {
                rightCollider.enabled = false;
            }
            //Disable left hand collider if right is touching a card
            else if (rightCollider.IsTouching(i.GetComponent<Collider2D>()))
            {
                leftCollider.enabled = false;
            }
            else
            {
                leftCollider.enabled = true;
                rightCollider.enabled = true;
            }
        }
    }
    void Update()
    {
        checkCollidingHand();
    }


}
