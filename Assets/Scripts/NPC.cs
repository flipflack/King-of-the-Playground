using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad;
    public void Interact()
    {
        Debug.Log("wallahi shittings good morning");

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // 2. Loads the new scene
            SceneManager.LoadScene(sceneToLoad);
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
