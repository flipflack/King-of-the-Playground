using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SewingMinigame : MonoBehaviour
{
    //thank you so much random among us wires minigame tutorial 
    public SewingStats[] stitchPoints;
    public Canvas canvas;
    public RectTransform threadContainer;
    public Image threadPrefab;
    public Color threadColor = new Color(0.45f, 0.12f, 0.08f);
    private float threadThickness = 3f;
    public Minigame minigame;
    public int completedStitches = 0;
    private bool isSewing = false;
    public SewingStats currentPoint;
    private Image tempThread;
    private List<Image> completedThreads = new List<Image>();
    private int nextStitchIndex = 1;

    private void Awake()
    {
        //put each hole into controller 
        for(int i = 0; i <stitchPoints.Length; i++)
        {
            if(stitchPoints[i] != null)
            {
                stitchPoints[i].sewingMG = this;
                //stitchPoints[i].stictchNum = (Stitch)i;
            }    
        }
        CreateTempThread();
    }
    public void ResetSewingGame()
    {
        completedStitches = 0;
        nextStitchIndex = 1;
        isSewing = false;
        currentPoint = null;

        //remove threads from prev attempt
        foreach (Image thread in completedThreads)
        {
            if (thread != null)
            {
                Destroy(thread.gameObject);
            }
        }
        completedThreads.Clear();
        if(tempThread != null)
        {
            tempThread.gameObject.SetActive(false);
        }
    }

    public void StartSewingGame()
    {
        isSewing = false;
    }

    public void BeginSewing(SewingStats point)
    {
        if (minigame.IsFinished)
            return;
        //make sure first click is on first stitch
        if(completedStitches == 0)
        {
            if (point != stitchPoints[0])
                return;
            currentPoint = point;
            nextStitchIndex = 1;
            isSewing = true;

            tempThread.gameObject.SetActive(true);
            //UpdateTempThread(GetPointPosition(currentPoint), GetMousePosition());
            return;
        }
        
        if(point == currentPoint)
        {
            isSewing = true;
            tempThread.gameObject.SetActive(true);
        }
        //click the completed stitch to cont to next
        //if(point == stitchPoints[completedStitches -1])
        //{
        //    currentPoint = point;
        //    isSewing = true;
        //    tempThread.gameObject.SetActive(true);
        //}
    }
    
    public void TryStitch(SewingStats point)
    {
        if (!isSewing || minigame.IsFinished)
            return;
        //only do next stitch if in order
        if (nextStitchIndex >= stitchPoints.Length)
            return;

        if (point != stitchPoints[nextStitchIndex])
            return;
        Debug.Log("Current hole: " + currentPoint.name);
        Debug.Log("Next hole: " + point.name);

        Vector2 start = GetPointPosition(currentPoint);
        Vector2 end = GetPointPosition(point);

        //drawing perma thread
        Image newThread = CreateThreadSegment(start, end);
        
        completedThreads.Add(newThread);
        currentPoint = point;
        nextStitchIndex++;
        completedStitches++;
        Debug.Log("Stitched hole " + completedStitches);

        //all points done
        if (nextStitchIndex >= stitchPoints.Length)
        {
            isSewing = false;
            tempThread.gameObject.SetActive(false);
            minigame.Complete(true);
        }
    }

    public void StopSewing()
    {
        isSewing = false;

        if(tempThread != null)
        {
            tempThread.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (!isSewing || currentPoint == null)
            return;

        //draw temp thread
        UpdateTempThread(GetPointPosition(currentPoint), GetMousePosition());

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current == null)
                return;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                SewingBehaviour stitch = result.gameObject.GetComponent<SewingBehaviour>();
                if (stitch != null)
                {
                    SewingStats stats = result.gameObject.GetComponent<SewingStats>();

                    if(stats != null)
                    {
                        TryStitch(stats);
                        break;
                    }
                }
            }
        }
        
    }

    private void CreateTempThread()
    {
        tempThread = Instantiate(threadPrefab, threadContainer);
        tempThread.raycastTarget = false;
        tempThread.color = threadColor;
        tempThread.gameObject.SetActive(false);
    }
    private Image CreateThreadSegment(Vector2 start, Vector2 end)
    {
        Image thread = Instantiate(threadPrefab, threadContainer);
        thread.gameObject.SetActive(true);
        thread.raycastTarget = false;
        thread.color = threadColor;

        SetThreadTransform(thread.rectTransform, start, end);
        Debug.Log("Created thread segment from " + start + " to " + end);
        return thread;
    }

    private void UpdateTempThread(Vector2 start, Vector2 end)
    {
        SetThreadTransform(tempThread.rectTransform, start, end);
    }

    private void SetThreadTransform(RectTransform threadRect, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float length = direction.magnitude;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        threadRect.anchorMin = new Vector2(0.5f, 0.5f);
        threadRect.anchorMax = new Vector2(0.5f, 0.5f);
        threadRect.pivot = new Vector2(0f, 0.5f);

        threadRect.anchoredPosition = start;
        threadRect.sizeDelta = new Vector2(length, threadThickness);
        threadRect.localRotation = Quaternion.Euler(0f,0f,angle);
    }

    private Camera GetCanvasCamera()
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        return canvas.worldCamera;
    }

    private Vector2 GetPointPosition(SewingStats point)
    {
        Camera cam = GetCanvasCamera();

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(cam, point.rectTransform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(threadContainer, screenPosition, cam, out Vector2 localPosition);

        return localPosition;
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = GetCanvasCamera();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(threadContainer, Input.mousePosition, cam, out Vector2 localPosition);

        return localPosition;
    }
}
