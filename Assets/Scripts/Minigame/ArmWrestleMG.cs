using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmWrestleMG : MonoBehaviour
{
    //float for "npc" that constantly goes down on an update 
    //the player presses space that increases the float at a slightly higher speed than the npc
    //checking a certian treshold for winning

    public float wrestleMatch= 10f;
    public float playerPower= 0.3f;
    public float bullyPower= 0.5f;
    public TextMeshProUGUI matchCount;

    public void Update()
    {
        wrestleMatch += bullyPower * Time.deltaTime;
        //Debug.Log(wrestleMatch);
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            wrestleMatch -= playerPower;
            Debug.Log("pressed");
        }
        if(wrestleMatch <= 1.5)
        {
            Debug.Log("win");
        }
        if(wrestleMatch >= 15.5)
        {
            Debug.Log("lose");
        }
        matchCount.text = wrestleMatch.ToString();
    }

}

