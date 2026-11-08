using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC Data", order = 1)]
public class NPC_Data : ScriptableObject
{
    public string npcName;
    public string startingNode;
    public string endingNode;

    public Sprite npcSprite;
    public enum dialogPhase { start, game, end }
    public dialogPhase currentPhase;


    public void resetData()
    {
        currentPhase = dialogPhase.start;
    }
}
