using UnityEngine;
using UnityEngine.EventSystems;

public class SewingBehaviour : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler, IDragHandler
{
    bool mouseDown = false;
    public SewingStats sewStatsS;
    private RectTransform rectTransform;
    private Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sewStatsS = gameObject.GetComponent<SewingStats>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
        Debug.Log("mouseDown");

    }
   
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        sewStatsS.movable = true;
        Debug.Log("mouseOver");
    }
    
   
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!sewStatsS.moving)
        {
            sewStatsS.movable = false;
        }
        Debug.Log("mouseExit");
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
        gameObject.transform.position = sewStatsS.startPos;
        Debug.Log("mouseUp");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("moving yo shi");
        //Works for Screen Space - Overlay
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rectTransform.position = eventData.position;
        }
        //Works for Screen Space - Camera and World Space
        else
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 worldPoint
            );
            rectTransform.position = worldPoint;
        }
    }
}
