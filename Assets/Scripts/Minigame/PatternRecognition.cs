using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatternRecognition : MonoBehaviour
{
    //Count down 3,2,1 appears in text b4 game starts
    //NOTE: feels a little slow when in "realtime"
    // quick pop up of what to do "Count!"
    //NOTE: needs to be changable per minigame
    //one sentence description of what to do ("Click the buttons from 1-10")
    //NOTE: needs to be changable per minigame 

    
    public List<Button> buttons;
    public List<Button> shuffledButtons;
    private int counter = 0;
    private int incNum;

    public bool gameActive = false;
    public bool playedGame1 = false;

    private Minigame minigame;

    private void Awake()
    {
        minigame = GetComponent<Minigame>();            
    }

    //called in minigame.resetstate()
    public void ResetCountingGame()
    {
        counter = 0;
        gameActive = false;

        shuffledButtons = buttons.OrderBy(x => Random.value).ToList();

        if(!playedGame1)
        {
         incNum = Random.Range(1, 3);
            Debug.Log("incNum1:"+ incNum);
        }
        else
        {
          incNum = Random.Range(3, 5);
            Debug.Log("incNum2:" + incNum);
        }
            for (int i = 0; i < shuffledButtons.Count; i++)
            {
                Button button = shuffledButtons[i];
                button.GetComponentInChildren<TextMeshProUGUI>().text = ((i + 1) * incNum).ToString();
                button.interactable = true;
                button.image.color = new Color32(177, 220, 233, 255);
            }
    }

    //called after countdown
    public void StartCountingGame()
    {
        gameActive = true;
    }
    
    public void pressButton(Button button)
    {
        //cant press button after game ends
        if (!gameActive || minigame.IsFinished)
            return;

        int buttonNumber = int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text);

        int expectedNumber = (counter + 1) * incNum;
        
        if (buttonNumber == expectedNumber)
        {
            //correct button pressed
            counter++;
            button.interactable = false;
            button.image.color = Color.green;
            if (counter == buttons.Count)
            {
                WinCountGame();
            }
        }
        else
        {
            //if hit wrong number button
            LoseCountGame();
        }
    }

    private void WinCountGame()
    {
        gameActive = false;
        Debug.Log("won count mg");
        
        foreach(Button button in shuffledButtons)
        {
            button.image.color = Color.green;
            button.interactable = false;
        }
        playedGame1 = true;
        minigame.Complete(true);
    }
    private void LoseCountGame()
    {
        gameActive = false;
        Debug.Log("lose count mg");

        foreach(Button button in shuffledButtons)
        {
            button.image.color = Color.red;
            button.interactable = false;    
        }
        playedGame1 = true;
        minigame.Complete(false);
    }
   
}
