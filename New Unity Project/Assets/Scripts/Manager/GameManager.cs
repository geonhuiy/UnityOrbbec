using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    private List<string> objectStringList;
    private List<GameObject> objectList = new List<GameObject>();
    void Start()
    {
        //add object to list
        Debug.Log(object1);
        Debug.Log(object2);
        Debug.Log(object3);
        //
        objectList.Add(object1);
        objectList.Add(object2);
        objectList.Add(object3);
        //initiate the starting sprite
        objectStringList = ObjectManager.GetRandomObjectWithoutDuplicate(3);

        for(int i = 0; i<3; i++) {
            Debug.Log(objectStringList[i]);
            objectList[i].GetComponent<Object>().setType(objectStringList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
