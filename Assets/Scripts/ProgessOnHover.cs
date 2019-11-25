using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgessOnHover : MonoBehaviour
{
    private bool isHovering = false;
    private float hoverTime;
    private float maxHoverTime = 2.0f;
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
        isHovering = true;
        Debug.Log("Enter trigger");
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
                //TextIndicatorPercent.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
                //TextLoading.gameObject.SetActive(true);

            }
            // When the loading bar is full
            else
            {
                //TextLoading.gameObject.SetActive(false);
                //TextIndicatorPercent.GetComponent<Text>().text = "DONE!";
                Radial.gameObject.SetActive(false);

            }

            LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
            Debug.Log("The current amount " + currentAmount);
            //buttonObjCollider = buttonObj.GetComponent<Collider2D>();
            //buttonObjCollider.enabled = false;
            //buttonObj.transform.position = handObj.transform.position;
        }

    }





    // Reset the values for the progress bar
    private void resetProgressBarValues()
    {
        isHovering = false;
        //Radial.gameObject.SetActive(false);
        currentAmount = 0;
        hoverTime = 0;
    }
}
