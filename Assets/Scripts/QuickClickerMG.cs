using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickClickerMG : MonoBehaviour
{
    //Pop up of white and red sheep in random loc
    //add all red sheep to list
    //Click on all the red sheep 
    //Remove red sheep from list
    //If list empty win minigame
    //timer bar at bottom
    //MAYBE: if clicked on white sheep shake screen, lower time

    public GameObject whiteSheep;
    public GameObject redSheep;
    public RectTransform spawnArea;
    public int wspawnCount = 7;
    public int rspawnCount = 3;
    public int totalCount = 10;
    private int currentCount = 0;
    public float spawnInterval = 0.05f;


    
    private void Start()
    {
        //sheepSpawner();
        StartCoroutine(SpawnProg());
    }
    private IEnumerator SpawnProg()
    {
        Debug.Log("working");
        while(currentCount < totalCount)
        {
            sheepSpawner();
            currentCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
        
    }




    public void sheepSpawner()
    {
        float width = spawnArea.rect.width;
        float height = spawnArea.rect.height;

        if (currentCount < wspawnCount)
        {
            float edgeOffset = 50f;
            float randomX = Random.Range((-width / 2f) + edgeOffset, (width / 2f) - edgeOffset);
            float randomY = Random.Range((-height / 2f) + edgeOffset, (height / 2f) - edgeOffset);
            Vector3 randomLocalPos = new Vector3(randomX, randomY, 0f);

            GameObject newSheep = Instantiate(whiteSheep, spawnArea);

            RectTransform rect = newSheep.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localPosition = randomLocalPos;
            }
          
        }
        else 
        {
            float edgeOffset = 50f;
            float randomX = Random.Range((-width / 2f) + edgeOffset, (width / 2f) - edgeOffset);
            float randomY = Random.Range((-height / 2f) + edgeOffset, (height / 2f) - edgeOffset);
            Vector3 randomLocalPos = new Vector3(randomX, randomY, 0f);

            GameObject newSheep = Instantiate(redSheep, spawnArea);

            RectTransform rect = newSheep.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localPosition = randomLocalPos;
            }

        }

    }

}
