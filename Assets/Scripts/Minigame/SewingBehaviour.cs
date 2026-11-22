using UnityEngine;
using UnityEngine.EventSystems;

public class SewingBehaviour : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public SewingStats sewStatsS;

    private void Awake()
    {
        sewStatsS = GetComponent<SewingStats>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        sewStatsS.sewingMG.BeginSewing(sewStatsS);
    }
   
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        //have to be holding down mouse button to sew
        if(eventData.pointerPress != null)
        { 
            sewStatsS.sewingMG.TryStitch(sewStatsS);
        }
        Debug.Log("mouseOver");
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        sewStatsS.sewingMG.StopSewing();
    }
    
    
}
