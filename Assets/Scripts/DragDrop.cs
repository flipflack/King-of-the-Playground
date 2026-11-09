using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    //we love code monkey 

    [SerializeField] Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public char letter; //chars are for single characters wowie didnt know that
    public ItemSlot currentSlot;
    public Vector3 originalPosition;
    private Vector3 dragStartPosition;
    private bool successfullyDropped;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalPosition = rectTransform.position;
    }

    public void LetterSetter (char newLetter)
    {
        letter = newLetter;
        GetComponentInChildren<TMP_Text>().text = letter.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        canvasGroup.blocksRaycasts = false;
        dragStartPosition = rectTransform.position;
        successfullyDropped = false;

        if(currentSlot !=null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }
        transform.SetAsLastSibling();

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        //return to previos pos if not placed in a slot (ehhh not so sure abt this)
        //if(!successfullyDropped)
        //{
        //    rectTransform.position = dragStartPosition;
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void PlaceInSlot(ItemSlot slot)
    {
        if (slot.occupiedCard != null)
            return;

        currentSlot = slot;
        slot.occupiedCard = this;
        rectTransform.position = slot.transform.position;
        successfullyDropped = true;
        slot.wordScramble.CheckAnswer();
    }
    public void SetOriginalPosition()
    {
        originalPosition = rectTransform.position;
    }
    public void ReturnHome()
    {
        currentSlot = null; 
        rectTransform.position = originalPosition;
    }
}
