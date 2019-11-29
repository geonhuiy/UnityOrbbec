using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CardManager : MonoBehaviour
{
    //Card manager singleton
    public static CardManager instance;
    public Text scoreDisplay;
    private int tutorialCount = 0;
    //Placeholder empty GameObjects in the scene
    [SerializeField]
    private GameObject[] placeholders;
    [SerializeField]
    private Card[] cardNames;
    [SerializeField]
    private Text randomText;
    private List<string> currentCardName = new List<string>();
    public int selectedCard;
    private int correctCard;
    private int guessCount = 0;
    public Canvas cardCanvas;
    public Canvas endGameCanvas;
    public Text resultText;
    private int score = 0;

    void Awake()
    {
        //Ensures that only one instance of CardManager exists
        if (instance != null)
        {
            //Destroy instance if CardManager exists
            Destroy(gameObject);
        }
        //Set current to CardManager
        instance = this;
        DontDestroyOnLoad(this);
        AssignSprites();
    }

    public void ResetGame() {
        guessCount = 0;
        scoreDisplay.text = "0";
        endGameCanvas.gameObject.SetActive(false);
    }

    public void CheckAnswer()
    {
        guessCount++;
        //check if answer is correct, if correct, add 50 to score, if incorrect minus 50 from score
        if (selectedCard == correctCard + 1)
        {
            Debug.Log("correct");
            FindObjectOfType<SoundManager>().Play("correctAnswer");
            score += 50;
            scoreDisplay.text = score.ToString();
        }
        else
        {
            Debug.Log("incorrect");
            FindObjectOfType<SoundManager>().Play("incorrectAnswer");
            score -= 20;
            scoreDisplay.text = score.ToString();
        }
        
        //after checking for score, move to end round
        EndRound();
    }

    private void EndRound()
    {
        // end round is display answer result and feedback to player
        if(guessCount < 3) {
            // start the next round
            AssignSprites();
        } else {
            EndGame();
        }
    }

    private void EndGame() {
        cardCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);
        resultText.text = "Hyvaa!!!! Sinulla on "+ score.ToString()+" pisteet!!";
    }

    public void AssignSprites()
    {
        //Creates a list from Card array for later removal
        List<Card> activeCards = new List<Card>(cardNames);
        //clear list for old card name before assign new ones
        currentCardName.Clear();
        //Loads a sprite for each placeholder
        for (int i = 0; i < placeholders.Length; ++i)
        {
            //Chooses a random number below the Card count
            int rand = UnityEngine.Random.Range(0, activeCards.Count);
            //Instantiates a Card at position rand
            Card randCard = activeCards[rand];
            //Sets the sprite of placeholder GameObject to the sprite component of randCard
            placeholders[i].GetComponent<SpriteRenderer>().sprite = randCard.cardImage;
            //Removes the randCard to prevent duplicate sprites from being selected
            activeCards.Remove(randCard);
            //add name of randcard to card name list for display
            currentCardName.Add(randCard.cardType.ToString());
        }
        RandomText();
    }

    private void RandomText()
    {
        //Selects a random card name from the 3 cards shown
        int rand = UnityEngine.Random.Range(0, currentCardName.Count);
        randomText.text = currentCardName[rand];
        correctCard = rand;
    }
}
