/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;

/// <summary>
/// Correct vizualization between elements and player.
/// This class make effect like we see object in real life:
/// in dependence object on game set behide another object.
/// </summary>
public class CorrectOrderByY : MonoBehaviour {

	// Component SpriteRendere of object
    private SpriteRenderer _spriteRenderer;
	// Position of object.
    private Vector3 _position;

	void Start () {
		// Get component of object
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        // Get a position of current object
        _position = transform.position;

        // Set order:
        // the order more higher if position y more lower
        // the player every time in position y = -2, so
        // it has const order and another object in this position will
        // not have such order (it's help for do not have problems with same
        // order with player and another objects(do effect like in real time))
        for (int i = 7, j = 1; i!=-6; i--, j++)
        {
            if (_position.y < i)
            {
                _spriteRenderer.sortingOrder = j;

                if (tag != "Player")
                {
                    if (_position.y >= -2)
                        _spriteRenderer.sortingOrder = j - 1;

                    if (_position.y < -2)
                        _spriteRenderer.sortingOrder = j + 1;
                }
            }
        }   
    }   
}
