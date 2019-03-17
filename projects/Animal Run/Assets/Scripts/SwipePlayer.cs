/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;

/// <summary>
/// Do interection with player in depedence swiped side.
/// </summary>
public class SwipePlayer : MonoBehaviour {

	// First position touch
	private Vector3 fp;
	// Last position touch
	private Vector3 lp;   
	// True if object is moving to swiped side.
    private bool isMoved = false;

    void Update()
    {
        if (!RunGame.IsGameOver && Time.timeScale != 0)
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
                if (!isMoved && (touch.phase == TouchPhase.Ended || 
					(fp.x - lp.x) > 60 || (fp.x - lp.x) < -60))
                {
					// Left swipe
                    if ((fp.x - lp.x) > 60) 
                    {
                        if (transform.position.x != -2)
                        {
                            transform.Translate(Vector3.left);
                        }

                        isMoved = true;
                    }
					// Right swipe
                    else if ((fp.x - lp.x) < -60) 
                    {
                        if (transform.position.x != 2)
                        {
                            transform.Translate(Vector3.right);
                        }

                        isMoved = true;
                    }
					// Up swipe
					else if ((fp.y - lp.y) < -60)
                    {
                        // Add your jumping code or other here
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    isMoved = false;
                }
            }
        }
		else
		{
			// Solve the problem after click button pauseNo player move
			fp = lp;
		}
    }


}
