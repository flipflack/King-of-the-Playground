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

        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            Button button = shuffledButtons[i];

            if (!playedGame1)
            {
                //First game goes up by 1
                button.GetComponentInChildren<TextMeshProUGUI>().text =
                    (i + 1).ToString();
            }
            else
            {
                //Second game goes up by 2
                button.GetComponentInChildren<TextMeshProUGUI>().text =
                    ((i + 1) * 2).ToString();
            }

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

        int expectedNumber;

        //second game checks for 2,4,6,8,10 etc.
        if(playedGame1)
        {
            expectedNumber = (counter + 1) * 2;
        }
        //first game checks for 1-10
        else
        {
            expectedNumber = counter + 1;
        }
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
