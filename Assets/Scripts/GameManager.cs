using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    public void BeginConversation(NPC_Data npc, string dialogueScene)
    {
        CurrentNPC = npc;
        ReturnScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(dialogueScene);
    }

    public void EndConversation()
    {
        CurrentNPC = null;
        if (!string.IsNullOrEmpty(ReturnScene))
            SceneManager.LoadScene(ReturnScene);
    }
}