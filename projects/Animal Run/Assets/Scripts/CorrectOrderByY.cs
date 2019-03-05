using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectOrderByY : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Vector3 pos;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        //get a position of current object
        pos = transform.position;

        //set order
        //the order more higher if position y more lower
        //the player every time in position y = -2, so
        //it has const order and another object in this position will
        //not have such order (it's help for do not have problems with same
        //order with player and another objects
        for (int i = 7, j = 1; i!=-6; i--, j++)
        {
            if (pos.y < i)
            {

                spriteRenderer.sortingOrder = j;

                if (tag != "Player")
                {
                    if (pos.y >= -2)
                        spriteRenderer.sortingOrder = j - 1;

                    if (pos.y < -2)
                        spriteRenderer.sortingOrder = j + 1;
                }

            }

        }
        
    }

    
}
