using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

//singleton script, persistent
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public NPC_Data CurrentNPC { get; private set; }
    public string ReturnScene { get; private set; }

    // where Lola was standing when she started talking, so she comes back to the same spot
    private bool hasReturnPosition;
    private Vector3 returnPosition;
    private bool returnFlipX;
    private InMemoryVariableStorage variableStorage;

    // Each NPC's dialogue phase, kept here (not on the ScriptableObject) so it
    // survives scene changes. Unity can unload and reload a ScriptableObject
    // between scenes, and NPC_Data.OnEnable resets it back to "start" when that happens.
    private readonly Dictionary<string, NPC_Data.dialogPhase> npcPhases =
        new Dictionary<string, NPC_Data.dialogPhase>(System.StringComparer.OrdinalIgnoreCase);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        variableStorage = GetComponent<InMemoryVariableStorage>();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void BeginConversation(NPC_Data npc, string sceneToLoad)
    {
        CurrentNPC = npc;
        ReturnScene = SceneManager.GetActiveScene().name;

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        hasReturnPosition = player != null;
        if (player != null)
        {
            returnPosition = player.transform.position;
            returnFlipX = player.spriteRenderer != null && player.spriteRenderer.flipX;
        }
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
        if (Instance != this) return;   // ignore events meant for an old/duplicate GameManager

        if (hasReturnPosition && scene.name == ReturnScene && CurrentNPC == null)
        {
            RestorePlayerPosition();
            return;
        }
        if (CurrentNPC == null) return;   // not in a conversation (e.g. returning to the overworld)

        DialogueRunner runner = FindAnyObjectByType<DialogueRunner>();
        if (runner == null)
        {
            Debug.LogError("GameManager: no DialogueRunner found in scene '" + scene.name + "'.");
            return;
        }

        runner.VariableStorage = variableStorage;   // usepersistent storage 

        NPC npcInScene = FindAnyObjectByType<NPC>();
        if (npcInScene != null && CurrentNPC.npcSprite != null)
        {
            SpriteRenderer sr = npcInScene.GetComponent<SpriteRenderer>(); // getting the NPC sprite
            if (sr != null) sr.sprite = CurrentNPC.npcSprite;
            npcInScene.transform.position += (Vector3)CurrentNPC.confrontationOffset;
        }

        // per-character blurred background
        DialogueBackground bg = FindAnyObjectByType<DialogueBackground>();
        if (bg == null && CurrentNPC.backgroundSprite != null)
        {
            GameObject bgObject = new GameObject("DialogueBackground");
            bgObject.AddComponent<SpriteRenderer>();
            bg = bgObject.AddComponent<DialogueBackground>();
        }
        if (bg != null) bg.SetBackground(CurrentNPC.backgroundSprite);

        StartCoroutine(StartDialogueRoutine(runner));
    }
    private IEnumerator StartDialogueRoutine(DialogueRunner runner)
    {
        yield return null;   // wait one frame so the DialogueRunner finishes setting itself up
        if (CurrentNPC == null || runner == null) yield break;

        runner.onDialogueComplete.AddListener(OnDialogueComplete);
        runner.StartDialogue(GetNodeFor(CurrentNPC));
    }
    private void OnDialogueComplete()
    {
        EndConversation();
    }

    private void RestorePlayerPosition()
    {
        hasReturnPosition = false;

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player == null) return;

        player.transform.position = returnPosition;
        if (player.rb != null)
        {
            player.rb.position = returnPosition;
            player.rb.linearVelocity = Vector2.zero;
        }
        if (player.spriteRenderer != null) player.spriteRenderer.flipX = returnFlipX;

        // snap the camera straight to her instead of sliding over from the spawn point
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 camPos = cam.transform.position;
            cam.transform.position = new Vector3(returnPosition.x, camPos.y, camPos.z);
        }
    }

    public NPC_Data.dialogPhase GetPhase(NPC_Data npc)
    {
        if (npc == null) return NPC_Data.dialogPhase.start;
        return npcPhases.TryGetValue(npc.npcName, out var phase) ? phase : NPC_Data.dialogPhase.start;
    }

    private string GetNodeFor(NPC_Data npc)
    {
        NPC_Data.dialogPhase phase = GetPhase(npc);
        npc.currentPhase = phase;   // keep the Inspector in sync for debugging
        return phase == NPC_Data.dialogPhase.end && !string.IsNullOrEmpty(npc.endingNode)
            ? npc.endingNode
            : npc.startingNode;
    }

    private void StorePhase(string npcName, string phase)
    {
        if (string.IsNullOrEmpty(npcName)) return;

        if (System.Enum.TryParse(phase, true, out NPC_Data.dialogPhase parsed))
        {
            npcPhases[npcName] = parsed;
            if (CurrentNPC != null && string.Equals(CurrentNPC.npcName, npcName, System.StringComparison.OrdinalIgnoreCase))
                CurrentNPC.currentPhase = parsed;
            Debug.Log($"Phase for {npcName} set to {parsed}");
        }
        else
        {
            Debug.LogWarning("Unknown phase '" + phase + "'. Use start, game or end.");
        }
    }

    // Yarn: <<set_phase "end">>  (the NPC you're talking to)
    [YarnCommand("set_phase")]
    public static void SetPhase(string phase)
    {
        if (Instance == null || Instance.CurrentNPC == null) return;
        Instance.StorePhase(Instance.CurrentNPC.npcName, phase);
    }

    // Yarn: <<set_phase_for "Grace" "end">>  (any NPC, by npcName)
    [YarnCommand("set_phase_for")]
    public static void SetPhaseFor(string npcName, string phase)
    {
        if (Instance == null) return;
        Instance.StorePhase(npcName, phase);
    }
}