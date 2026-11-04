using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
    void OnTouchingPlayer();
    void OnNotTouchingPlayer();

}

public class Interactor : MonoBehaviour
{

    [SerializeField] private GameObject interactPrompt;
    private IInteractable currentInteractable;

    // Update is called once per frame
    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }
    void Update()
    {
        if(Keyboard.current.eKey.IsActuated() && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if(interactable != null)
        {
            currentInteractable = interactable;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            currentInteractable.OnTouchingPlayer();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            currentInteractable.OnNotTouchingPlayer();
            currentInteractable = null;
        }
    }
}
