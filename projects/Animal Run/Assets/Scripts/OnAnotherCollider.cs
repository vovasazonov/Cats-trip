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
			CoinSettins coinSettins = collision.GetComponent<CoinSettins>();

			if (CoinSettins.CollectCoin != null)
			{
				// Call event when collect coin.
				CoinSettins.CollectCoin.Invoke(coinSettins);
			}

			// Hide money
			collision.GetComponent<SpriteRenderer>().enabled = false;

			// Add money to player's wallet. :)
            RunGame.CountMoney += coinSettins.Cost;
        }
		if (collision.tag == "Bonus")
		{
			// Set bonus as child of player.
			collision.transform.SetParent(transform);
		}
		else
        {
            RunGame.IsGameOver = true;
        }
    }
}
