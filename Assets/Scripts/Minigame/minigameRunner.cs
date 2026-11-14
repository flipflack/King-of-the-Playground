using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum iType { marbles, counting, word, arm, sewing}

public class minigameRunner : MonoBehaviour
{
    [Tooltip("Set the type of interaction to load the correct variables")]
    [SerializeField] public iType type;

    //All canvases
    public GameObject minigameCanvas;
    public GameObject marblesMinigameCanvas;
    public GameObject countingMinigameCanvas;
    public GameObject wordMinigameCanvas;
    public GameObject armWrestleMinigameCanvas;
    public GameObject sewingMinigameCanvas;

    //timer stuff
    public float gameTime = 5f;
    private float currentTime;
    public Slider timerBar;

    public bool gameActive = false;
    public bool gameEnded = false;

    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI countdownText;
    public string instructPrompt = "";
    public string countdownPrompt = "";

    public void Start()
    {
        StartCoroutine(StartCountdown());
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
        countdownText.text = countdownPrompt;
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        countingMinigameCanvas.SetActive(true);

    }
}
