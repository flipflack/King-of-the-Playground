using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
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
            group.alpha = 1f;
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
        ////so it doesnt check untill every slot has answer
        //foreach(ItemSlot slot in answerSlots)
        //{
        //    if (slot.occupiedCard == null)
        //        return;
        //}

        ////create the players answer into a word
        //string playerAnswer = "";
        //Debug.Log(playerAnswer);
        //foreach(ItemSlot slot in answerSlots)
        //{
        //    playerAnswer += slot.occupiedCard.letter;
        //}

        ////check player answer
        //if(playerAnswer == correctWord)
        //{
        //    Debug.Log("won");
        //}
        //else
        //{
        //    Debug.Log("lost");
        //}
        // Don't check until every slot has a letter
        foreach (ItemSlot slot in answerSlots)
        {
            if (slot.occupiedCard == null)
                return;
        }

        // Build the player's answer
        string playerAnswer = "";

        foreach (ItemSlot slot in answerSlots)
        {
            playerAnswer += slot.occupiedCard.letter;
        }

        Debug.Log("Player Answer: " + playerAnswer);

        // Check the answer
        if (playerAnswer == correctWord)
        {
            Debug.Log("YOU WIN!");
            // Add win dialogue or next minigame here
        }
        else
        {
            Debug.Log("Incorrect! Try again.");
            // Player can rearrange the letters
        }
    }
}
