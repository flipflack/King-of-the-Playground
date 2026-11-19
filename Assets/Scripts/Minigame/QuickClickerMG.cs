using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickClickerMG : MonoBehaviour
{
    //FIN: Pop up of white and red sheep in random loc
    //add all red sheep to list
    //Click on all the red sheep 
    //Remove red sheep from list
    //If list empty win minigame
    //timer bar at bottom

    public GameObject whiteSheep;
    public GameObject redSheep;

    public RectTransform spawnArea;

    public int wspawnCount;
    public int rspawnCount;
    private float spawnInterval = 0.02f;
    
    private bool gameActive = false;
    private int currentCount = 0;

    public bool playedGame1 = false;

    public List<GameObject> redSheepList = new List<GameObject>();
    public List<GameObject> spawnedSheep = new List<GameObject>();

    private Minigame minigame;

    private void Awake()
    {
        minigame = GetComponent<Minigame>();
    }
    
    //called by Minigame.ResetState()
    public void ResetSheepGame()
    {
        StopAllCoroutines();
        gameActive = false;
        currentCount = 0;

        //clearing previous sheep 
        foreach(GameObject sheep in spawnedSheep)
        {
            if(sheep != null)
                Destroy(sheep);
        }
        foreach(GameObject sheep in redSheepList)
        {
            if (sheep != null)
                Destroy(sheep);
        }
        spawnedSheep.Clear();
        redSheepList.Clear();
    }

    //called after the countdown
    public void StartSheepGame()
    {
        StartCoroutine(SpawnProg());
    }
   
    private IEnumerator SpawnProg()
    {
        //instructionsText.text = ("Click all of the red sheep!");
        //Debug.Log("working");
        //Debug.Log(redSheepList);
        wspawnCount = Random.Range(8, 10);
        rspawnCount = Random.Range(2, 4);
        if(playedGame1)
        {
            wspawnCount = Random.Range(10,14);
            rspawnCount = Random.Range(4,6);
        }

        while (currentCount < wspawnCount + rspawnCount)
        {
            sheepSpawner();
            currentCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        gameActive = true;
        playedGame1 = true;
        Debug.Log("playedGame1" + playedGame1);
    }

    public void sheepSpawner()
    {
        float width = spawnArea.rect.width;
        float height = spawnArea.rect.height;
        float edgeOffset = 50f;



        float randomX = Random.Range((-width / 2f) + edgeOffset, (width / 2f) - edgeOffset);
        float randomY = Random.Range((-height / 2f) + edgeOffset, (height / 2f) - edgeOffset);
        Vector3 randomLocalPos = new Vector3(randomX, randomY, 0f);

        GameObject sheepPrefab;

        if (currentCount < wspawnCount)
        {
            sheepPrefab = whiteSheep;
        }
        else
        {
            sheepPrefab = redSheep;
        }

        GameObject newSheep = Instantiate(sheepPrefab, spawnArea);
        //redSheepList.Add(newSheep);
        RectTransform rect = newSheep.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localPosition = randomLocalPos;
        }

        if (sheepPrefab == redSheep)
        {
            redSheepList.Add(newSheep);

            Button sheepButton = newSheep.GetComponent<Button>();

            if (sheepButton != null)
            {
                sheepButton.onClick.AddListener(() => SheepClicked(newSheep));
            }
        }
        else
        {
            spawnedSheep.Add(newSheep);
        }

    }
    public void SheepClicked(GameObject sheep)
    {
        if (!gameActive || minigame.IsFinished)
            return;
        
        if (redSheepList.Contains(sheep))
        {
            Destroy(sheep);
            redSheepList.Remove(sheep);
            Debug.Log("got red sheep, remaining: " + redSheepList.Count);

            if (redSheepList.Count == 0)
            {
                WinSheepGame();
            }
        }
        Debug.Log("played marble 1: " + playedGame1);
    }

    private void WinSheepGame()
    {
        //gameEnded = true;
        gameActive = false;
        Debug.Log("won");
        minigame.Complete(true);
        //this.gameObject.SetActive(false);
    }

    //private void LoseSheepGame()
    //{
    //    gameEnded = true;
    //    gameActive=false;
    //    Debug.Log("lost");
    //    GetComponent<Minigame>().Complete(false);
    //    //this.gameObject.SetActive(false);
    //}
}

