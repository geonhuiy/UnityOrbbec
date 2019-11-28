using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="newCard")]
//Allows creation of a GameObject based on a model with the data below
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public CardType cardType;
}
public enum CardType {
    Brocolli,Coffee,Banana,Wine,Rose,Fork,BBQ,Blueberry,Chocolate,Fish,Fridge,Kiwi,MincedMeat,Orange,RyeBread,Strawberry,Tomato,WoodenSpoon
}
