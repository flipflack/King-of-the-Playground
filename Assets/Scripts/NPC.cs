using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("wallahi shittings good morning");
    }

    public void OnNotTouchingPlayer()
    {
     
    }

    public void OnTouchingPlayer()
    {
      
    }
}
