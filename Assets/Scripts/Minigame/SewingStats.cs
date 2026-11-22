using UnityEngine;

//public enum Stitch { one, two, three, four};
public class SewingStats : MonoBehaviour
{
    public bool movable = false;
    public bool moving = false;
    public Vector3 startPos;
    //public Stitch stictchNum;

    public SewingMinigame sewingMG;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  
    }

   
}
