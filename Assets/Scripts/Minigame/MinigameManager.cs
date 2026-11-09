using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class MinigameManager : MonoBehaviour
{
    // handles MINIGAME UI and YSS

    [SerializeField] private GameObject myMinigameUI;
    [SerializeField] private Minigame myMinigame;
    [SerializeField] private DialogueRunner dialogueRunner;

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