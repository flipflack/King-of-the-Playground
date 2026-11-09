using UnityEngine;

public class Minigame : MonoBehaviour
{
    public bool IsFinished { get; private set; }
    public bool DidPlayerWin { get; private set; }

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
    }
}