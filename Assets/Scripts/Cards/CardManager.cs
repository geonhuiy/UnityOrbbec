using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //Card manager singleton
    public static CardManager instance;
    //Placeholder empty GameObjects in the scene
    [SerializeField]
    private GameObject[] placeholders;
    [SerializeField]
    private Card[] cardNames;
    public int selectedCard;
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


    private void AssignSprites()
    {
        //Creates a list from Card array for later removal
        List<Card> activeCards = new List<Card>(cardNames);
        //Loads a sprite for each placeholder
        for (int i = 0; i < placeholders.Length; ++i)
        {
            //Chooses a random number below the Card count
            int rand = Random.Range(0,activeCards.Count);
            //Instantiates a Card at position rand
            Card randCard = activeCards[rand];
            //Sets the sprite of placeholder GameObject to the sprite component of randCard
            placeholders[i].GetComponent<SpriteRenderer>().sprite = randCard.cardImage;
            //Removes the randCard to prevent duplicate sprites from being selected
            activeCards.Remove(randCard);
        }
    }
}
