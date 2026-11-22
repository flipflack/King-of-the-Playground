using System;
using UnityEngine;

public enum MinigameType { marbles, counting, word, arm, sewing, word2, sewing2}

[Serializable]
//universal settings for all the minigames 
//basically all the things the minigames have in common and need UI wise 
public struct MinigameSettings
{
    public MinigameType type;

    public GameObject minigameCanvas;
    public Minigame minigame;

    public string instructions;
    public string countdownPrompt;

    public float gameTime;
}
