using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private GameObject coin;
    private GridManager gridManager;

    private bool doSpawning;
    
    IEnumerator SpawnCoin()
    {
        while (doSpawning)
        {
            int rand = Random.Range(0, gridManager.gridPoints.Length);
            yield return new WaitForSeconds(10f);

            GameObject newCoin = Instantiate(coin, gridManager.gridPoints[rand].transform, false);
            newCoin.GetComponent<Animator>().Play("coinSpin");
        }
    }
    
    public void SetupCoins(GameObject c, GridManager g)
    {
        coin = c;
        gridManager = g;
    }

    public void SpawnCoins()
    {
        doSpawning = true;
        StartCoroutine("SpawnCoin");
    }
}
