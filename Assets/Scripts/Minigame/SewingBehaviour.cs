using UnityEngine;
using UnityEngine.EventSystems;

public class SewingBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
   
    public void OnPointerUp(PointerEventData eventData)
    {
        sewStatsS.sewingMG.StopSewing();
    }
    
    
}
