using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour, IInteractable
{
    public NPC_Data npcData; // attach to canvas or manager


    [SerializeField] private string sceneToLoad;
    public void Interact()
    {
        
        if (npcData != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"interacted with {npcData.npcName}"); //specifies NPC to make ur life easier

            // loads scene
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
