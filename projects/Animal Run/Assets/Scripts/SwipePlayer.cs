using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePlayer : MonoBehaviour {

    private Vector3 fp;   //first position touch
    private Vector3 lp;   //last position touch

    // Use this for initialization
    void Start () {
        
    }

    bool isMoved = false;

    void Update()
    {
        if (!RunGame.isGameOver && Time.timeScale != 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    lp = touch.position;
                }
                if (!isMoved && (touch.phase == TouchPhase.Ended || (fp.x - lp.x) > 60 || (fp.x - lp.x) < -60))
                {
                    if ((fp.x - lp.x) > 60) // left swipe
                    {
                        if (transform.position.x != -2)
                        {
                            transform.Translate(Vector3.left);
                        }

                        isMoved = true;
                    }
                    else if ((fp.x - lp.x) < -60) // right swipe
                    {
                        if (transform.position.x != 2)
                        {
                            transform.Translate(Vector3.right);
                        }

                        isMoved = true;
                    }
                    else if ((fp.y - lp.y) < -60) // up swipe
                    {
                        // add your jumping code here
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    isMoved = false;
                }
            }
        }
        else
            fp = lp; // solve the problem after click button pauseNo player move
    }


}
