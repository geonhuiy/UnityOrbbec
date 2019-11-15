using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject yes;
    public GameObject no;
    private int counter_happy;
    private int counter_sad;
    public float intX;
    public float intY;
    public float intZ;
    float discrepancy;
    int interval_happy = 120;
    int interval_sad = 120;
    System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        counter_happy = 0;
        counter_sad = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counter_happy++;
        counter_sad++;
        if(counter_happy == interval_happy) {
            counter_happy = 0;
            discrepancy = rnd.Next(-11, 11);
            GameObject newYes = Instantiate(yes, new Vector3 (intX+discrepancy, intY, intZ), transform.rotation);
            interval_happy = rnd.Next(60, 120);
            Debug.Log(interval_happy);
        }

        if(counter_sad == interval_sad) {
            counter_sad = 0;
            discrepancy = rnd.Next(-11, 11);
            GameObject newNo = Instantiate(no, new Vector3 (intX-discrepancy, intY, intZ), transform.rotation);
            interval_sad = rnd.Next(60, 120);
            Debug.Log(interval_sad);
        }
    }
}
