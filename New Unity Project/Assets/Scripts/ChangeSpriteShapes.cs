using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteShapes : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer rend;
    private Sprite circleSprite, diamondSprite;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        circleSprite = Resources.Load<Sprite>("Circle");
        diamondSprite = Resources.Load<Sprite>("Diamond");
        rend.sprite = circleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (rend.sprite == circleSprite)
                rend.sprite = diamondSprite;
            else if (rend.sprite == diamondSprite)
            {
                rend.sprite = circleSprite;
            }
        }
    }
}
