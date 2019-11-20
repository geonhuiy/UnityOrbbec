using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Object: MonoBehaviour
{
    string type = "";

    void Start() {

    }

    void Update() {
        
    }
    
    public void setType(string input) {
        Debug.Log("Sprite called");
        type = input;
        var rend = GetComponent<SpriteRenderer>();
        rend.sprite = Resources.Load<Sprite>(input);
    }
}
