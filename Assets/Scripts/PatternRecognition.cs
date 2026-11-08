using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatternRecognition : MonoBehaviour
{
    public List<Button> buttons;
    public List<Button> shuffledButtons;
    int counter = 0;

    public float gameTime = 5f;
    private float currentTime;
    public Slider timerBar;

    public bool gameActive = false;
    public bool gameEnded = false;

    public void Start()
    {
        currentTime = gameTime;
        if (timerBar != null)
        {
            timerBar.maxValue = gameTime;
            timerBar.value = gameTime;
        }
        RestartTheGame();
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
            LoseCountGame();
        }
    }
    public void RestartTheGame()
    {
        counter = 0;
        currentTime = gameTime;
        gameEnded = false;
        gameActive = true;

        if (timerBar != null)
        {
            timerBar.maxValue = gameTime;
            timerBar.value = gameTime;
        }

        shuffledButtons = buttons.OrderBy(a => Random.Range(0, 100)).ToList();
        for (int i = 1; i < 11; i++)
        {
           // shuffledButtons[i - 1].GetComponentInChildren<Text>().text = i.ToString();
            shuffledButtons[i-1].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            shuffledButtons[i - 1].interactable = true;
            shuffledButtons[i - 1].image.color = new Color32(177, 220, 233, 255);
        }
        gameActive = true;
        
    }
    public void pressButton(Button button)
    {
        //cant press button after game ends
        if (!gameActive || gameEnded)
            return;

        if (int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text) - 1 == counter)
        {
            counter++;
            button.interactable = false;
            button.image.color = Color.green;

            if (counter == 10)
            {
                StartCoroutine(presentResult(true));
            }
           
        }
        else
        {
            StartCoroutine(presentResult(false));
        }
    }

    public IEnumerator presentResult(bool win)
    {
        gameEnded = true;
        gameActive = false;

        if (!win)
        {
            Debug.Log("lost");
            foreach (var button in shuffledButtons)
            {
                button.image.color = Color.red;
                button.interactable = false;
            }
        }
        else
        {
            Debug.Log("won");
            foreach (var button in shuffledButtons)
            {
                button.image.color = Color.green;
                button.interactable = false;
            }
        }
            yield return new WaitForSeconds(2f);
       // RestartTheGame();
       //it will say a you win
       //then dialog then next minigame
    }
    private void LoseCountGame()
    {
        if (gameEnded || !gameActive)
            return;
        StartCoroutine(presentResult(false));
    }
}
