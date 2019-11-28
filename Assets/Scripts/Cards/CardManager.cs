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
    private int tutorialCount = 0;
    //Placeholder empty GameObjects in the scene
    [SerializeField]
    private GameObject[] placeholders;
    [SerializeField]
    private Card[] cardNames = null;
    [SerializeField]
    private Text randomText;
    private List<string> currentCardName = new List<string>();
    public int selectedCard;
    private int correctCard;
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

    public void CheckAnswer() {
        //check if answer is correct, if correct, add 50 to score, if incorrect minus 50 from score
        if (selectedCard == correctCard+1) {
            Debug.Log("correct");
            scoreDisplay.text = (Int32.Parse(scoreDisplay.text) + 50).ToString(); 
            
        } else {
            Debug.Log("incorrect");
            scoreDisplay.text = (Int32.Parse(scoreDisplay.text) - 50).ToString(); 
        }
        AssignSprites();

        //after checking for score, move to end round
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
        int rand = UnityEngine.Random.Range(0, currentCardName.Count);
        randomText.text = currentCardName[rand];
        correctCard = rand;
        Debug.Log(rand);
    }
}
