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
}