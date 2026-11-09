using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using System.Collections;

//singleton script, persistent
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public NPC_Data CurrentNPC { get; private set; }
    public string ReturnScene { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void BeginConversation(NPC_Data npc, string sceneToLoad)
    {
        CurrentNPC = npc;
        ReturnScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void EndConversation()
    {
        CurrentNPC = null;
        if (!string.IsNullOrEmpty(ReturnScene))
            SceneManager.LoadScene(ReturnScene);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CurrentNPC == null) return;   // not in a conversation (e.g. returning to the overworld)

        DialogueRunner runner = FindAnyObjectByType<DialogueRunner>();
        if (runner == null)
        {
            Debug.LogError("GameManager: no DialogueRunner found in scene '" + scene.name + "'.");
            return;
        }

        StartCoroutine(StartDialogueRoutine(runner));
    }
    private IEnumerator StartDialogueRoutine(DialogueRunner runner)
    {
        yield return null;   // wait one frame so the DialogueRunner finishes setting itself up
        if (CurrentNPC == null || runner == null) yield break;

        runner.onDialogueComplete.AddListener(OnDialogueComplete);
        runner.StartDialogue(CurrentNPC.GetNodeForPhase());
    }
    private void OnDialogueComplete()
    {
        EndConversation();
    }

    // Yarn: <<set_phase "game">>  /  <<set_phase "end">>
    [YarnCommand("set_phase")]
    public static void SetPhase(string phase)
    {
        if (Instance == null || Instance.CurrentNPC == null) return;

        if (System.Enum.TryParse(phase, true, out NPC_Data.dialogPhase parsed))
            Instance.CurrentNPC.currentPhase = parsed;
        else
            Debug.LogWarning("Unknown phase '" + phase + "'. Use start, game or end.");
    }
}