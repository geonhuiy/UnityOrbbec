using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    // Update is called once per frame
    private Vector3 mousePosition;
    public float moveSpeed = 1.0f; 
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 1;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector3.Lerp(transform.position, mousePosition, moveSpeed);
    }
}
