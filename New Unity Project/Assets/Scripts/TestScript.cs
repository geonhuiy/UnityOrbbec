using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("up")) {
            print("up held down");
            count++;
            print(count);
        }

        if(Input.GetKey("down")) {
            print("down held down");
            count++;
            print(count);
        }
    }
}
