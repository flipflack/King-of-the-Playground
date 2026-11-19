
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


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

    private bool gameActive = false;

    private Minigame minigame;

    private void Awake()
    {
        minigame = GetComponent<Minigame>();    
    }

    //called by minigame.resetstate
    public void ResetWordGame()
    {
        gameActive = false;

        //pick rando word
        correctWord = potentialWords[Random.Range(0,potentialWords.Count)].ToUpper();
        
        //make sure number of cards and slots is samesies 
        if (letterCards.Count != correctWord.Length || answerSlots.Count != correctWord.Length)
        {
            Debug.Log("you done messed up the cards and slots bro");
            return;
        }
        //clear all answer slots in case
        foreach(ItemSlot slot in answerSlots)
        {
            if(slot.occupiedCard != null)
            {
                slot.occupiedCard.currentSlot = null;
            }
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
            DragDrop card = letterCards[i];
            card.currentSlot = null;
            card.gameObject.SetActive(true);
            card.wordScramble = this;

            card.LetterSetter(scrambledLetters[i]);
            CanvasGroup group = card.GetComponent<CanvasGroup>();
            
            if(group!=null)
            {
                group.alpha = 1f;
                group.interactable = true;
                group.blocksRaycasts = true;
            }
          
        }
        //randomize card pos
        List<DragDrop> shuffledCards = letterCards.OrderBy(x => Random.value).ToList(); 
        for (int i = 0; i <shuffledCards.Count; i++)
        {
            shuffledCards[i].transform.SetSiblingIndex(i);
        }
        //saving card location after!!! shuffling 
        //foreach(DragDrop card in letterCards)
        //{
        //    if (card == null)
        //    {
        //        Debug.LogError("One of the Letter Cards in the Inspector list is EMPTY!");
        //        continue;
        //    }

        //    Debug.Log("Checking card: " + card.gameObject.name);

        //    //card.SetOriginalPosition();
        //    //card.ReturnHome();
        //}

    }
    
    //called after countdown
    public void StartWordGame()
    {
        gameActive = true;
    }
    public bool CanInteract
    {
        get { return gameActive && !minigame.IsFinished; }
    }

    public void CheckAnswer()
    {
        if (minigame.IsFinished || !gameActive)
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
            WinWordGame();
        }
        else
        {
            Debug.Log("nope try again");
        }
        
    }
    public void WinWordGame()
    {
        gameActive = false;
        Debug.Log("won word game");

        //disable all cards
        foreach(DragDrop card in letterCards)
        {
            CanvasGroup group = card.GetComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;
        }
       minigame.Complete(true);
    }
   
}
