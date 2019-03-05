using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnotherCollider : MonoBehaviour {

    //improve it to another script
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
            if (collision.name == "CoinGold(Clone)")
                RunGame.CountMoney += 3;
            else if (collision.name == "CoinSliver(Clone)")
                RunGame.CountMoney++;
        }
        else
        {
            RunGame.isGameOver = true;
        }
    }
}
