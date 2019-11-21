using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabLoader : MonoBehaviour
{
    public List<GameObject> cardPositions;
    public Text puzzleText;
    private List<GameObject> objectList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        createRandomPrefabs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createRandomPrefabs()
    {
        GameObject[] prefebs = Resources.LoadAll<GameObject>("Prefabs");
        List<GameObject> prefabList = new List<GameObject>();
        List<GameObject> activeCards = new List<GameObject>();
        foreach (GameObject a in prefebs)
        {
            prefabList.Add(a);
        }
        //int prefebLength = prefebs.Length;
        Debug.Log(prefabList.Count);
        for (int i = 0; i < cardPositions.Count; i++)
        {
            int rand = Random.Range(0, prefabList.Count);
            GameObject temp = prefabList[rand];

            Instantiate(temp, cardPositions[i].transform.position, transform.rotation);
            Debug.Log(temp.name);
            prefabList.Remove(temp);
            //objectList.Add(temp);
            activeCards.Add(temp);
        }

        //puzzleText.text = objectList[Random.Range(0, objectList.Count + 1)].name;
        Debug.Log(activeCards.Count);
        puzzleText.text = activeCards[Random.Range(0, activeCards.Count)].name;
    }

}
