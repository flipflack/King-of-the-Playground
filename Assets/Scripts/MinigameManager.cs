using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class MinigameManager : MonoBehaviour
{
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

        myMinigameUI.SetActive(true);

        while (!myMinigame.IsFinished)
        {
            yield return null;
        }

        VariableStorageBehaviour variableStorage = FindAnyObjectByType<VariableStorageBehaviour>();

        if (variableStorage != null)
        {
            variableStorage.SetValue("$minigame_won", myMinigame.DidPlayerWin);
        }

        myMinigameUI.SetActive(false);
    }
}