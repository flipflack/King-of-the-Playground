using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class MinigameManager : MonoBehaviour
{
    //handles MINIGAME UI and YSS

    [SerializeField] private MinigameSettings[] minigames;

    //shared UI
    [SerializeField] private GameObject minigameBGCanvas;
    [SerializeField] private Slider timerBar;
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private TextMeshProUGUI countdownText;

    //yarnspinner stuff
    [SerializeField] private DialogueRunner dialogueRunner;

    private MinigameSettings currentSettings;
    private Minigame currentMinigame;

    private float currentTime;
    private bool isRunning = false;


    [SerializeField] private GameObject myMinigameUI;
    [SerializeField] private Minigame myMinigame;

    [YarnCommand("start_minigame")]
    public IEnumerator StartMinigameCommand(string gameName)
    {
        //checks if UI and Mnigame exist, or is referenced 
        if (myMinigameUI == null || myMinigame == null)
        {
            Debug.LogError("MinigameManager is missing its UI or Minigame reference.");
            yield break;
        }

        myMinigame.ResetState();
        myMinigameUI.SetActive(true);

        while (!myMinigame.IsFinished)
        {
            yield return null;
        }

        if (dialogueRunner != null && dialogueRunner.VariableStorage != null)
        {
            dialogueRunner.VariableStorage.SetValue("$minigame_won", myMinigame.DidPlayerWin);
        }

        myMinigameUI.SetActive(false);
    }
}