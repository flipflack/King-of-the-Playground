using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class NPC : MonoBehaviour, IInteractable
{
    //references
    public DialogueRunner dialog;
    public NPC_Data npcData; // attach to canvas or manager


    [SerializeField] private string sceneToLoad;
    public void Interact()
    {
        Debug.Log($"interacted with {npcData.npcName}"); //specifies NPC to make ur life easier

        if (npcData != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            // 2. Loads the new scene
            GameManager.Instance.BeginConversation(npcData,sceneToLoad);
            Debug.Log("entered new scene");
        }
        else
        {
            Debug.LogWarning("enter a name in the inspector what are you doing");
        }
    }

    public void OnNotTouchingPlayer()
    {
     
    }

    public void OnTouchingPlayer()
    {
      
    }
}
