using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public Canvas cardCanvas;
    public GameObject tutorialScreen;
    public Canvas tutorialCanvas;
    private int tutorialCount = 0;
    void Awake()
    {
        //Ensures that only one instance of TutorialManager exists
        if (instance != null)
        {
            //Destroy instance if TutorialManager exists
            Destroy(gameObject);
        }
        //Set current to TutorialManager
        instance = this;
        DontDestroyOnLoad(this);
        cardCanvas.gameObject.SetActive(false);
        ShowTutorial();
    }
    public void ResetTutorial() {
        tutorialCount = 0;
        tutorialCanvas.gameObject.SetActive(true);
        ShowTutorial();
    }

    public void ShowTutorial()
    {
        //Increments the tutorial sprite each time next button is pressed/hovered on
        tutorialCount++;
        if (tutorialCount > 3)
        {
            //Starts the game after the tutorial count
            InitGame();
        }
        else
        {
            tutorialScreen.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial/Tutorial Sample " + tutorialCount);
        }
    }

    private void InitGame()
    {
        //Loads the game canvas 
        cardCanvas.gameObject.SetActive(true);
        //Random card generation on the game canvas
        CardManager.instance.AssignSprites();
        //Disables the tutorial canvas
        tutorialCanvas.gameObject.SetActive(false);
    }


}
