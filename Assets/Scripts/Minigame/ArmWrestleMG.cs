using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmWrestleMG : MonoBehaviour
{
    //float for "npc" that constantly goes down on an update 
    //the player presses space that increases the float at a slightly higher speed than the npc
    //checking a certian treshold for winning
    //arm (rectangle) that moves left or right to display who is winning. Updates with float value

    private float wrestleMatch= 0;
    public float playerPower= 1f;
    public float bullyPower= 2.6f;
    //private float delay = 5f;
    //private float timeSinceSpacePressed = 0f;
    public TextMeshProUGUI matchCount;
    public GameObject arm;
    //public bool isAFK = false;

    public void Update()
    {
        wrestleMatch -= bullyPower * Time.deltaTime;
        //Debug.Log(wrestleMatch);
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            wrestleMatch += playerPower;
            //timeSinceSpacePressed = 0f;
            //Debug.Log("pressed");
            CheckScore();
        }
        //if(!Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    timeSinceSpacePressed += Time.deltaTime;
        //    //Debug.Log("afk for: " + timeSinceSpacePressed);
        //}
        //if the player isn't clicking for a while the bully will overtake quicker
        //if (timeSinceSpacePressed >= delay)
        //{
        //    bullyPower = 10f;
        //    Debug.Log("Changed Bully to: " + bullyPower);
        //}
        if (wrestleMatch <= -20)
        {
            bullyPower = 3.2f;
            //Debug.Log("Changed Bully to: " + bullyPower);
        }
        if (wrestleMatch >= -10)
        {
            bullyPower = 2.6f;
            //Debug.Log("Changed Bully to: " + bullyPower);
        }
        if (wrestleMatch >= 25)
        {
            playerPower = 2f;
            //Debug.Log("Changed Player to: " + playerPower);
        }
        if (wrestleMatch <= 22)
        {
            playerPower = 1f;
            //Debug.Log("Changed Player to: " + playerPower);
        }
        matchCount.text = wrestleMatch.ToString("0");
        arm.transform.rotation = Quaternion.Euler(0f, 0f, wrestleMatch * 2);
    }

    private void CheckScore()
    {
      
        if (wrestleMatch <= -40)
        {
            Debug.Log("lose");
        }
        if (wrestleMatch >= 40)
        {
            Debug.Log("win");
        }
    }
}

