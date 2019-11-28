using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [SerializeField]
    private GameObject leftHand, rightHand;

    void Awake()
    {
        //Ensures that only one instance of HandManager exists
        if (instance != null)
        {
            //Destroy instance if HandManager exists
            Destroy(gameObject);
        }
        //Set current to HandManager
        instance = this;
        DontDestroyOnLoad(this);
    }


}
