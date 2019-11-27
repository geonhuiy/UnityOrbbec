using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgessOnHover : MonoBehaviour
{
    private bool isHovering = false;
    private float hoverTime;
    public float maxHoverTime;
    public float speed;
    [SerializeField] private float currentAmount;

    public Transform LoadingBar;
    public Transform Radial;
    public Transform TextIndicatorPercent;
    public Transform TextLoading;

    public GameObject handObj, buttonObj;
    public Collider2D buttonObjCollider;


    // When you enter the collider 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "object" || collision.gameObject.tag == "user_interactable" || collision.gameObject.tag == "tutorialButton") {
            isHovering = true;
            Debug.Log("Enter trigger");
        }
    }

    //When you leave the colldier
    private void OnTriggerExit2D(Collider2D collision)
    {
        resetProgressBarValues();   
        Debug.Log("Left the collider");
    }

    private void Start()
    {
        Radial.gameObject.SetActive(false);
    }

    private void Update()
    {

        // If you are hovering over a hoverable object, do the following
        if (isHovering)
        {
            hoverTime += Time.deltaTime;

            // If the loading bar amount is less than full
            if (currentAmount < 100)
            {
                Radial.gameObject.SetActive(true);
                currentAmount = speed*hoverTime;
            }
            // When the loading bar is full
            else
            {
                Radial.gameObject.SetActive(false);
            }
            Radial.GetComponent<Image>().fillAmount = currentAmount / 100;
            Debug.Log("The current amount " + currentAmount);
        }

    }

    // Reset the values for the progress bar
    private void resetProgressBarValues()
    {
        isHovering = false;
        Radial.GetComponent<Image>().fillAmount = 0;
        Radial.gameObject.SetActive(false);
        currentAmount = 0;
        hoverTime = 0;
    }
}
