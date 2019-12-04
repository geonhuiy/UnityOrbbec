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
    public Canvas cardCanvas;
    public GameObject tutorialScreen;
    public GameObject correctImage, failImage;
    public GameObject leftHand, RightHand;
    private float feedbackDisplayTime = 2.0f;
    private float feedbackCounter = 0;
    private bool showWrongAnswerFeedback;
    private bool showRightAnswerFeedback;

    private int tutorialCount = 0;
    //Placeholder empty GameObjects in the scene
    [SerializeField]
    private GameObject[] placeholders;
    [SerializeField]
    private Card[] cardNames;
    [SerializeField]
    private Text randomText;
    public List<string> currentCardName = new List<string>();
    public int selectedCard;
    private int correctCard;
    private int guessCount = 0;

    public Canvas endGameCanvas;
    public Text resultText;
    private int score = 0;

    void Awake()
    {
        correctImage.SetActive(false);
        failImage.SetActive(false);
        //Ensures that only one instance of CardManager exists
        if (instance != null)
        {
            //Destroy instance if CardManager exists
            Destroy(gameObject);
        }
        //Set current to CardManager
        instance = this;
        DontDestroyOnLoad(this);
        //AssignSprites();
    }
    private void Update()
    {
        if (showWrongAnswerFeedback)
        {

            if (feedbackCounter < feedbackDisplayTime)
            {
                failImage.SetActive(true);
                print(feedbackCounter);
                RightHand.SetActive(false);
                leftHand.SetActive(false);


            }
            else
            {
                failImage.SetActive(false);
                showWrongAnswerFeedback = false;
                feedbackCounter = 0;
                RightHand.SetActive(true);
                leftHand.SetActive(true);

            }
            feedbackCounter += Time.deltaTime;
        }
        if (showRightAnswerFeedback)
        {
            if (feedbackCounter < feedbackDisplayTime)
            {
                correctImage.SetActive(true);
                RightHand.SetActive(false);
                leftHand.SetActive(false);
            }
            else
            {
                correctImage.SetActive(false);
                showRightAnswerFeedback = false;
                feedbackCounter = 0;
                RightHand.SetActive(true);
                leftHand.SetActive(true);

            }
            feedbackCounter += Time.deltaTime;

        }
    }

    public void ResetGame()
    {
        guessCount = 0;
        scoreDisplay.text = "0";
        score = 0;
        endGameCanvas.gameObject.SetActive(false);
        AssignSprites();
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
            showRightAnswerFeedback = true;
        }
        else
        {
            Debug.Log("incorrect");
            FindObjectOfType<SoundManager>().Play("incorrectAnswer");
            score -= 20;
            scoreDisplay.text = score.ToString();
            showWrongAnswerFeedback = true;
        }

        //after checking for score, move to end round
        EndRound();
    }

    private void EndRound()
    {
        // end round is display answer result and feedback to player
        if (guessCount < 5  )
        {
            // start the next round
            AssignSprites();
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        cardCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);

        if (score <= 0)
        {
            resultText.text = "Hyvä!" + System.Environment.NewLine + " Kiitos pelaamisesta";
        }
        else
        {
            resultText.text = "Hyvä! " + score.ToString() + " pistettä!";
        }


        //public void CheckAnswer() {
        //    //check if answer is correct, if correct, add 50 to score, if incorrect minus 50 from score
        //    if (selectedCard == correctCard + 1) {
        //        Debug.Log("correct");
        //        showRightAnswerFeedback = true;
        //        //correctImage.SetActive(true);
        //        scoreDisplay.text = (Int32.Parse(scoreDisplay.text) + 50).ToString();

        //    } else {
        //        //failImage.SetActive(true);
        //        showWrongAnswerFeedback = true;
        //        Debug.Log("incorrect");
        //        scoreDisplay.text = (Int32.Parse(scoreDisplay.text) - 50).ToString();
        //    }
        //    AssignSprites();

        //    //after checking for score, move to end round
        //
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
