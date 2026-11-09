using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public DragDrop occupiedCard;
    public WordScramble wordScramble;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (occupiedCard != null)
            return;
       if ( eventData.pointerDrag != null)
        {
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            DragDrop draggedCard = eventData.pointerDrag.GetComponent<DragDrop>();
            if (draggedCard != null)
            {
                draggedCard.PlaceInSlot(this);
            }

        }
    }
    public void ClearSlot()
    {
        occupiedCard = null;    
    }
}
