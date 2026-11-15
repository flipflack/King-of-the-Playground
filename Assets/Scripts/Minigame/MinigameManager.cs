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

    private void Awake()
    {
        //hide all minigame UI when the scene switches
        minigameBGCanvas.SetActive(false);

        foreach (MinigameSettings settings in minigames)
        {
            settings.minigameCanvas.SetActive(false);
        }
        countdownText.gameObject.SetActive(false);
    }

    [YarnCommand("start_minigame")]
    public IEnumerator StartMinigameCommand(string gameName)
    {
        //converting ys string to enum
        if(!System.Enum.TryParse(gameName, true, out MinigameType selectedType))
        {
            Debug.Log("Not a minigame type:" + gameName);
            yield break;
        }
        
        //finds matching minigame settings
        bool found = false; 
        
        foreach (MinigameSettings settings in minigames)
        {
            if(settings.type == selectedType)
            {
                currentSettings = settings;
                found = true;
                break;
            }
        }

        //if this be broke
        if(!found)
        {
            Debug.Log("no settings found fix yo shit for: " + gameName);
            yield break;
        }
        if(currentSettings.minigameCanvas == null || currentSettings.minigame == null)
        {
            Debug.Log("missing references for: " + gameName);
            yield break;
        }

        isRunning = true;
        //turning on everything for the selected mg
        currentMinigame = currentSettings.minigame;
        minigameBGCanvas.SetActive(true);
        instructionsText.text = currentSettings.instructions;
        currentSettings.minigameCanvas.SetActive(true);
        currentTime = currentSettings.gameTime;

        if(timerBar != null)
        {
            timerBar.maxValue = currentTime;
            timerBar.value = currentTime;
        }

        currentMinigame.ResetState();

        //start the game 
        yield return StartCoroutine(StartCountdown());


        while(!currentMinigame.IsFinished && currentTime >0)
        {
            currentTime -= Time.deltaTime;
            timerBar.value = Mathf.Max(0, currentTime);

            yield return null;
        }

        //if time runs out lose
        if(!currentMinigame.IsFinished)
        {
            currentMinigame.Complete(false);
        }

        //tell ys the results
        if (dialogueRunner != null && dialogueRunner.VariableStorage != null)
        {
            dialogueRunner.VariableStorage.SetValue("$minigame_won", currentMinigame.DidPlayerWin);
        }
        Debug.Log(currentMinigame.DidPlayerWin ? "won minigame" : "lost minigame");

        yield return new WaitForSeconds(1f);

        //turn everything off after mg done
        currentSettings.minigameCanvas.SetActive(false);
        minigameBGCanvas.SetActive(false);
        countdownText.gameObject.SetActive(false);
        isRunning = false;
    }

    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        int count = 3;

        while(count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = currentSettings.countdownPrompt;
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        currentMinigame.StartGame();
    }
}