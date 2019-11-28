using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorPanel : MonoBehaviour
{
    private int shapeCounter = 0;
    private bool isPhasing = true;
    private System.Random rnd = new System.Random();
    private int currentCheck = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentCheck = rnd.Next(10, 30);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPhasing == true) {
            shapeCounter++;
        }

        if(shapeCounter == currentCheck) {
            
        }
    }

    void Reset() {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void StartShrink() {
        
    }
}
