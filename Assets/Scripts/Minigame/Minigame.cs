using UnityEngine;
using UnityEngine.Events;

public class Minigame : MonoBehaviour
{
    public bool IsFinished { get; private set; }
    public bool DidPlayerWin { get; private set; }

    public UnityEvent onResetState;
    public UnityEvent onStartGame;

    public void Complete(bool playerWon)
    {
        DidPlayerWin = playerWon;
        IsFinished = true;
    }

    // Called by MinigameManager before each play so the minigame can be replayed
    public void ResetState()
    {
        IsFinished = false;
        DidPlayerWin = false;

        //runs the individual minigames reset functions 
        onResetState?.Invoke();
    }
    public void StartGame()
    {
        onStartGame?.Invoke();  
    }
}