using TMPro;
using UnityEngine;
using Yarn.Unity;

public class NPC_Interaction : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public string Interaction;
    public bool canInteract;
    public DialogueRunner dialog;
    public NPC_Data npcData;
}
