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
    private bool dragAllowed;

    public WordScramble wordScramble;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (rectTransform == null)
        {
            Debug.LogError("DragDrop needs to be on the same GameObject as a RectTransform: " + gameObject.name);
            return;
        }

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

        //so unity doens't keep dragging the card after the game ends bruh
        dragAllowed = wordScramble != null && wordScramble.CanInteract;
        if (!dragAllowed)
            return;

        dragStartPosition = rectTransform.position;
        successfullyDropped = false;

        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        if (!dragAllowed)
            return;
        float parentScale = rectTransform.parent != null ? rectTransform.parent.lossyScale.x / canvas.transform.lossyScale.x : 1f;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor / parentScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        if (!dragAllowed)
            return;
        canvasGroup.blocksRaycasts = true;
        //return to previos pos if not placed in a slot (ehhh not so sure abt this)
        //if(!successfullyDropped)
        //{
        //    rectTransform.position = dragStartPosition;
        //}
        dragAllowed = false;
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
    //public void SetOriginalPosition()
    //{
    //    rectTransform = GetComponent<RectTransform>();

    //    if (rectTransform == null)
    //    {
    //        Debug.LogError(
    //            "No RectTransform found on letter card: " + gameObject.name,
    //            gameObject
    //        );
    //        return;
    //    }

    //    originalPosition = rectTransform.position;
    //}
    //public void ReturnHome()
    //{
    //    rectTransform = GetComponent<RectTransform>();

    //    if (rectTransform == null)
    //    {
    //        Debug.LogError("No RectTransform found on: " + gameObject.name);
    //        return;
    //    }

    //    currentSlot = null;
    //    rectTransform.position = originalPosition;
    //}
}
