using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeManager : MonoBehaviour
{

    private GridManager gridManager;

    private GameObject spike;
    private GameObject warning;
    private bool doSpawning;

    private int rounds;
    private int currentRound;

    IEnumerator WarnSpike(int rand)
    {
        GameObject newWarning = Instantiate(warning, gridManager.gridPoints[rand].transform, false);
        newWarning.GetComponent<Animator>().Play("blinkWarning");
        yield return new WaitForSeconds(1.3f);
        Destroy(newWarning);
    }

    IEnumerator SpawnSpike()
    {
        while (doSpawning)
        {
            //Get the GridPoint that will be used
            int rand = Random.Range(0, gridManager.gridPoints.Length);
            while (gridManager.IsGridPointInUse(rand))
            {
                // Debug.Log(gridPoints[rand] + " in use! Finding a new one...");
                rand = Random.Range(0, gridManager.gridPoints.Length);
            }
            gridManager.UseGridPoint(rand);
            yield return new WaitForSeconds(3f);
            
            //Begin the warning
            IEnumerator coroutine = WarnSpike(rand);
            StartCoroutine(coroutine);
            yield return new WaitForSeconds(1.3f);
            
            //Spawn the spike
            GameObject newSpike = Instantiate(spike, gridManager.gridPoints[rand].transform, false);
            newSpike.GetComponent<Animator>().Play("spikeUp");
            CameraShake.Shake(0.5f, 0.5f);
            yield return new WaitForSeconds(1f);
            
            //Despawn the spike
            gridManager.StopUsingGridPoint(rand);
            newSpike.GetComponent<Animator>().Play("spikeDown");
            Destroy(newSpike, 1f);
        }
    }

    IEnumerator CountRounds()
    {
        while (doSpawning)
        {
            yield return new WaitForSeconds(5.5f);
            currentRound++;
            if (currentRound == rounds)
            {
                doSpawning = false;
            }
        }
    }

    public void SetupSpikes(GameObject s, GameObject w, GridManager g)
    {
        spike = s;
        warning = w;
        gridManager = g;
    }

    public void SpawnSpikes(int amt, int amtRounds)
    {
        doSpawning = true;
        currentRound = 0;
        rounds = amtRounds;
        StartCoroutine("CountRounds");
        for (int i = 0; i < amt; i++)
        {
            StartCoroutine("SpawnSpike");
        }
    }

    public bool AreSpikesActive()
    {
        return doSpawning;
    }
}
