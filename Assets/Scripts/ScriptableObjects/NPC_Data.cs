using UnityEngine;


[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC Data", order = 1)]
public class NPC_Data : ScriptableObject
{
    public string npcName;
    public string startingNode;
    public Sprite npcSprite;
    public enum dialogPhase { start, game, end }
    public dialogPhase currentPhase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void resetData()
    {
        currentPhase = dialogPhase.start;
    }
}
