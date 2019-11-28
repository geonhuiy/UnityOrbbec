using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public Canvas cardCanvas;
    public GameObject tutorialScreen;
    private int tutorialCount = 1;
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

    public void ShowTutorial()
    {
        tutorialCount++;
        if (tutorialCount > 3)
        {
            InitGame();
        }
        else
        {
            tutorialScreen.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial/Tutorial Sample " + tutorialCount);
        }
    }

    private void InitGame()
    {
        cardCanvas.gameObject.SetActive(true);
        CardManager.instance.AssignSprites();
        tutorialScreen.gameObject.SetActive(false);
    }


}
