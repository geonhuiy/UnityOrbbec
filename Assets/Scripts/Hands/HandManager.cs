using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [SerializeField]
    private GameObject leftHand, rightHand;

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
        Collider2D leftCollider = leftHand.GetComponent<Collider2D>();
        Collider2D rightCollider = rightHand.GetComponent<Collider2D>();
        List<GameObject> cardObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("card"));
        foreach (var i in cardObjects)
        {
            if(leftCollider.IsTouching(i.GetComponent<Collider2D>())) {
                rightCollider.enabled = false;
            }
            //rightCollider.enabled = true;
            else if(rightCollider.IsTouching(i.GetComponent<Collider2D>())) {
                leftCollider.enabled = false;
            }
            else {
                leftCollider.enabled = true;
                rightCollider.enabled = true;
            }
            //leftCollider.enabled = true;
        }
    }
    void Update()
    {
        checkCollidingHand();
    }


}
