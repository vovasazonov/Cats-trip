/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;

/// <summary>
/// Attach script to player. Do interection
/// when collider player interaction with
/// another collider.
/// </summary>
public class OnAnotherCollider : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
		// Player touched money in game.
        if (collision.tag == "Coin")
        {
			// Hide money
            collision.GetComponent<SpriteRenderer>().enabled = false;

			// Add money to player's wallet. :)
            if (collision.name == "CoinGold(Clone)")
                RunGame.CountMoney += 3;
            else if (collision.name == "CoinSliver(Clone)")
                RunGame.CountMoney++;
        }
        else
        {
            RunGame.IsGameOver = true;
        }
    }
}
