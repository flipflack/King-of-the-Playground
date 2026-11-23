using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC Data", order = 1)]
public class NPC_Data : ScriptableObject
{
    public string npcName;
    public string startingNode;
    public string endingNode;

    public Sprite npcSprite;

    [Tooltip("Blurred background shown behind this character in the Confrontation scene. Leave empty to keep the camera's plain color.")]
    public Sprite backgroundSprite;

    public enum dialogPhase { start, game, end }
    public dialogPhase currentPhase;

    public string GetNodeForPhase()
    {
        return currentPhase == dialogPhase.end ? endingNode : startingNode;
    }
    public void resetData()
    {
        currentPhase = dialogPhase.start;
    }
    private void OnEnable()
    {
        resetData();
    }
}
