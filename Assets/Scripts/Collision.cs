using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Collision : MonoBehaviour
{
    private float hoverTime = 0;
    private float maxHoverTime = 2.0f;
    public Text answer;
    public Text display;
    public Text score;

    // the camera in the scene
    public GameObject camera;

    // tutorial display object
    public GameObject tutorialScreen;
    private int tutorialCount = 0;
    private bool isHovering = false;

    void Start() {
        answer.text = "";
        score.text = "";
        display.text = "";
        tutorialCount = 1;
        ShowTutorial();
    }

    // display tutorial at the start of the game
    private void ShowTutorial() {
        if(tutorialCount < 4) {
            tutorialScreen.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TutorialScreen/Tutorial Sample "+ tutorialCount);
        } else {
            InitGame();
        }
        
    }

    private void InitGame() {
        score.text = "0";
        Destroy(tutorialScreen);
        StartRound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        hoverTime = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hoverTime += Time.deltaTime;

        if (hoverTime > maxHoverTime)
        {
            switch(collision.gameObject.tag) {
                // when user hover over card object
                case "object":
                    hoverTime = 0;
                    if (collision.gameObject.name == answer.text || collision.gameObject.name == answer.text + "(Clone)")
                    {
                        OnCorrectAnswer();
                    }
                    else
                    {
                        OnWrongAnswer();
                    }
                    break;
                
                // when user hover over tutorial object
                case "tutorialButton":
                    tutorialCount++;
                    hoverTime = 0;
                    ShowTutorial();
                    break;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isHovering = false;
        hoverTime = 0;
    }

    private void OnCorrectAnswer()
    {
        display.text = "correct answer";
        score.text = (Int32.Parse(score.text) + 50).ToString();
        StartRound();
    }

    private void OnWrongAnswer()
    {
        display.text = "incorrect answer";
        score.text = (Int32.Parse(score.text) - 50).ToString();
        StartRound();
    }

    private void StartRound() {
        PrefabLoader prefabLoader = camera.GetComponent<PrefabLoader>();
        for (int i = 0; i < prefabLoader.activeCards.Count; i++)
        {
            GameObject cloneObjects = prefabLoader.activeCards[i];
            Debug.Log(cloneObjects.name);
            Destroy(cloneObjects);
            
        }
        prefabLoader.activeCards.Clear();
        prefabLoader.createRandomPrefabs();
    }
}
