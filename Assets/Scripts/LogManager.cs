using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{

    [Header("SFX")] 
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip warningSound;

    private GridManager gridManager;
    public bool doSpawning;

    private GameObject log;
    private GameObject warning;
    
    IEnumerator WarnLog(int row, int index)
    {
        GameObject newWarning = Instantiate(warning, gridManager.gridRows[row][index].transform, false);
        newWarning.GetComponent<Animator>().Play("blinkWarning");
        yield return new WaitForSeconds(1.3f);
        Destroy(newWarning);
    }
    
    IEnumerator SpawnLog()
    {
        bool canSpawn = false;
        int rand = Random.Range(0, gridManager.gridRows.Length);
        while (!canSpawn)
        {
            for (int i = 0; i < gridManager.gridRows[rand].Length; i++)
            {
                if (gridManager.IsGridPointInUse(gridManager.FindGridIndex(gridManager.gridRows[rand][i])))
                {
                    // Debug.Log("Can't spawn! " + gridManager.gridRows[rand][i] + " is filled.");
                    canSpawn = false;
                    rand = Random.Range(0, gridManager.gridRows.Length);
                }

                if (i == gridManager.gridRows[rand].Length - 1)
                {
                    canSpawn = true;
                }
            }
        }

        for (int i = 0; i < gridManager.gridRows[rand].Length; i++)
        {
            gridManager.UseGridPoint(gridManager.FindGridIndex(gridManager.gridRows[rand][i]));
        }

        for (int i = 0; i < gridManager.gridRows[rand].Length; i++)
        {
            IEnumerator coroutine = WarnLog(rand, i);
            StartCoroutine(coroutine);
        }
        yield return new WaitForSeconds(1.3f);

        GameObject newLog = Instantiate(log, gridManager.gridRows[rand][0].transform, false);
        SFXController.instance.PlaySFX(attack, transform, 0.05f);
        int randomAnim = Random.Range(0, 2);
        if (randomAnim == 0)
        {
            newLog.GetComponent<Animator>().Play("logSwipeLeft");
        }
        else
        {
            newLog.GetComponent<Animator>().Play("logSwipeRight");
        }
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < gridManager.gridRows[rand].Length; i++)
        {
            gridManager.StopUsingGridPoint(gridManager.FindGridIndex(gridManager.gridRows[rand][i]));
        }
        Destroy(newLog);
    }

    IEnumerator WarningSFX()
    {
        SFXController.instance.PlaySFX(warningSound, transform, 0.02f);
        yield return new WaitForSeconds(0.5f);
        SFXController.instance.PlaySFX(warningSound, transform, 0.02f);
        yield return new WaitForSeconds(0.5f);
        SFXController.instance.PlaySFX(warningSound, transform, 0.02f);
    }

    IEnumerator SpawnLogAmt(int amt, float length)
    {
        for (int i = 0; i < amt; i++)
        {
            StartCoroutine("SpawnLog");
            StartCoroutine("WarningSFX");
            yield return new WaitForSeconds(length);
        }
        doSpawning = false;
    }
    
    public void SetupLogs(GameObject l, GameObject w, GridManager g)
    {
        log = l;
        warning = w;
        gridManager = g;
    }

    public void SpawnLogs(int amt, float length)
    {
        doSpawning = true;
        IEnumerator coroutine = SpawnLogAmt(amt, length);
        StartCoroutine(coroutine);
    }

    public bool AreLogsActive()
    {
        return doSpawning;
    }
}
