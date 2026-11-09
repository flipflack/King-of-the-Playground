using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordScramble : MonoBehaviour
{
    //pick a word from list of potential words 
    //shuffle letter order and position as cards on screen
    //click and drag cards into letter slots 
    //check if cards are in the right spots aka spelling the right word

    public List<string> potentialWords;
    public List<DragDrop> letterCards;
    public List<ItemSlot> answerSlots;

    public string correctWord;

    public float gameTime = 5f;
    private float currentTime;
    public Slider timerBar;

    public bool gameActive = false;
    public bool gameEnded = false;

    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI countdownText;
    public GameObject wordMinigameCanvas;

    public void Start()
    {
        StartCoroutine(StartCountdown());
        //StartGame();
    }
    IEnumerator StartCountdown()
    {
        Debug.Log("runningCountdown");
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        countdownText.text = "Spell!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        wordMinigameCanvas.SetActive(true);
        StartGame();
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
            LoseWordGame();
        }
    }
    public void StartGame()
    {
        //eventually change this so it isn't hardcoded
        instructionsText.text = ("Unscrambled the letters to spell a word!");
        currentTime = gameTime;
        if (timerBar != null)
        {
            timerBar.maxValue = gameTime;
            timerBar.value = gameTime;
        }

        currentTime = gameTime;
        gameEnded = false;
        gameActive = true;

        if (timerBar != null)
        {
            timerBar.maxValue = gameTime;
            timerBar.value = gameTime;
        }

        //pick random word
        correctWord = potentialWords[Random.Range(0, potentialWords.Count)].ToUpper();
        Debug.Log(correctWord);

        //make sure number of cards and slots is samesies 
        if (letterCards.Count != correctWord.Length || answerSlots.Count != correctWord.Length)
        {
            Debug.Log("you done messed up the cards and slots bro");
            return;
        }
        //reset all slots
        foreach (ItemSlot slot in answerSlots)
        {
            slot.ClearSlot();
            slot.wordScramble = this;
        }
        //seperate word into letters
        List<char> scrambledLetters = correctWord.ToList();
        //shuffle letters
        scrambledLetters = scrambledLetters.OrderBy(x => Random.value).ToList();

        //assign letters to cards
        for (int i = 0; i < letterCards.Count; i++)
        {
            letterCards[i].currentSlot = null;
            letterCards[i].LetterSetter(scrambledLetters[i]);
            CanvasGroup group = letterCards[i].GetComponent<CanvasGroup>();
            group.interactable = true;
            group.blocksRaycasts = true;
            letterCards[i].gameObject.SetActive(true);
            letterCards[i].ReturnHome();
        }
        //randomize card pos
        List<DragDrop> shuffledCards = letterCards.OrderBy(x => Random.value).ToList(); 
        for (int i = 0; i <shuffledCards.Count; i++)
        {
            shuffledCards[i].transform.SetSiblingIndex(i);
            shuffledCards[i].SetOriginalPosition();
        }
    }
    public void CheckAnswer()
    {
        if (gameEnded || !gameActive)
            return;
        //so it doesnt check untill every slot has answer
        foreach (ItemSlot slot in answerSlots)
        {
            if (slot.occupiedCard == null)
                return;
        }

        //create the players answer into a word
        string playerAnswer = "";
        foreach (ItemSlot slot in answerSlots)
        {
            playerAnswer += slot.occupiedCard.letter;
        }
        Debug.Log(playerAnswer);

        //check player answer
        if (playerAnswer == correctWord)
        {
            EndWordGame(true);
        }
        else
        {
            Debug.Log("not try again");
        }
        
    }
    public void EndWordGame(bool won)
    {
        if(gameEnded) return;
        gameEnded = true;
        gameActive = false;
        foreach(DragDrop card in letterCards)
        {
            CanvasGroup group = card.GetComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        if(won)
        {
            Debug.Log("won");
            instructionsText.text = "You got the word!";
        }
        else
        {
            Debug.Log("lose");
            instructionsText.text = "You can't spell dummy.";
        }
    }
    private void LoseWordGame()
    {
        if (gameEnded || !gameActive)
            return;
       EndWordGame(false);
    }
}
