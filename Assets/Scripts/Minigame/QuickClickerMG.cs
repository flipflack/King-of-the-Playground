using System.Collections;
using System.Collections.Generic;
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

    public int wspawnCount = 7;
    public int rspawnCount = 3;

    public float spawnInterval = 0.05f;
    public float gameTime = 3f;
    private float currentTime;
    private bool gameActive = false;
    private bool gameEnded = false;

    private int currentCount = 0;
    [SerializeField]
    private List<GameObject> redSheepList = new List<GameObject>();

    public Slider timerBar;

    private void Awake()
    {
        //sheepSpawner(); 
        currentTime = gameTime;
        if (timerBar != null)
        {
            timerBar.maxValue = gameTime;
            timerBar.value = gameTime;
        }
        StartCoroutine(SpawnProg());
    }
    private void Update()
    {
        if (!gameActive || gameEnded)
            return;
        currentTime -= Time.deltaTime;
        if (timerBar != null)
        {
            timerBar.value = currentTime;
        }
        if (currentTime <= 0)
        {
            LoseSheepGame();
        }
    }
    private IEnumerator SpawnProg()
    {
        //Debug.Log("working");
        //Debug.Log(redSheepList);
        while (currentCount < wspawnCount + rspawnCount)
        {
            sheepSpawner();
            currentCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        gameActive = true;
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

    }
    public void SheepClicked(GameObject sheep)
    {
        if (!gameActive || gameEnded)
            return;
        if (redSheepList.Contains(sheep))
        {
            redSheepList.Remove(sheep);
            Destroy(sheep);
            Debug.Log("got red sheep, remaining: " + redSheepList.Count);

            if (redSheepList.Count == 0)
            {
                WinSheepGame();
            }
        }
    }

    private void WinSheepGame()
    {
        gameEnded = true;
        gameActive = false;
        Debug.Log("won");
        GetComponent<Minigame>().Complete(true);
        //this.gameObject.SetActive(false);
    }

    private void LoseSheepGame()
    {
        gameEnded = true;
        gameActive=false;
        Debug.Log("lost");
        GetComponent<Minigame>().Complete(false);
        //this.gameObject.SetActive(false);
    }
}

