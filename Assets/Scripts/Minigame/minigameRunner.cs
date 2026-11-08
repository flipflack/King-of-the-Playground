using TMPro;
using UnityEngine;

public class minigameRunner : MonoBehaviour
{
    //public GameObject Runner;
    public GameObject minigameCanvas;
    public GameObject dialogueCanvas;
    public void startSheepGame()
    {
        //Runner.SetActive(true);
        minigameCanvas.SetActive(true);
        dialogueCanvas.SetActive(false);
    }
}
