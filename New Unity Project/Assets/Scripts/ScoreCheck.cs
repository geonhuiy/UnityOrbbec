using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System;

public class ScoreCheck : MonoBehaviour
{
    public Text scoreDisplay;
    void Start() {
        scoreDisplay.text = "0";
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
       
       if(collision.gameObject.name == "Yes" || collision.gameObject.name == "Yes(Clone)") {
           int score = Int32.Parse(scoreDisplay.text);
           score++;
           scoreDisplay.text = score.ToString();
           Destroy(collision.gameObject);
       } else if (collision.gameObject.name == "No" || collision.gameObject.name == "No(Clone)")
       {
           int score = Int32.Parse(scoreDisplay.text);
           score--;
           scoreDisplay.text = score.ToString();
           Destroy(collision.gameObject);
       }
    }
}
