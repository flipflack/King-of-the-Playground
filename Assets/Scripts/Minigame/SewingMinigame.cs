using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SewingMinigame : MonoBehaviour
{
    //thank you so much random among us wires minigame tutorial 
    public SewingStats[] stitchPoints;
    public Canvas canvas;
    public RectTransform threadContainer;
    public Image threadPrefab;
    public Color threadColor = new Color(0.45f, 0.12f, 0.08f);
    public float threadThickness = 5f;
    public Minigame minigame;
    public int completedStitches = 0;
    private bool isSewing = false;
    public SewingStats currentPoint;
    private Image tempThread;
    private List<Image> completedThreads = new List<Image>();

    private void Awake()
    {
        //put each hole into controller 
        for(int i = 0; i <stitchPoints.Length; i++)
        {
            if(stitchPoints[i] != null)
            {
                stitchPoints[i].sewingMG = this;
                stitchPoints[i].stictchNum = (Stitch)i;
            }    
        }
        CreateTempThread();
    }
    public void ResetSewingGame()
    {
        completedStitches = 0;
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
            isSewing = true;

            tempThread.gameObject.SetActive(true);
            UpdateTempThread(GetPointPosition(currentPoint), GetMousePosition());
            return;
        }
        
        //click the completed stitch to cont to next
        if(point == stitchPoints[completedStitches -1])
        {
            currentPoint = point;
            isSewing = true;
            tempThread.gameObject.SetActive(true);
        }
    }
    
    public void TryStitch(SewingStats point)
    {
        if (!isSewing || minigame.IsFinished)
            return;
        //only do next stitch if in order
        if (completedStitches >= stitchPoints.Length)
            return;

        if (point != stitchPoints[completedStitches])
            return;
        
        //drawing perma thread
        //who knew you could pass methods into args that pretty wicked sick 
        Image newThread = CreateThreadSegment(GetPointPosition(currentPoint),GetPointPosition(point));
        
        completedThreads.Add(newThread);
        currentPoint = point;
        completedStitches++;

        //all points done
        if(completedStitches >= stitchPoints.Length)
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
        if (isSewing && currentPoint != null)
        {
            UpdateTempThread(GetPointPosition(currentPoint), GetMousePosition());
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
        thread.raycastTarget = false;
        thread.color = threadColor;
        SetThreadTransform(thread.rectTransform, start, end);   
        
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

        threadRect.anchoredPosition = start;
        threadRect.sizeDelta = new Vector2(length, threadThickness);
        threadRect.localRotation = Quaternion.Euler(0,0,angle);
    }

    private Vector2 GetPointPosition(SewingStats point)
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, point.rectTransform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(threadContainer, screenPosition, canvas.worldCamera, out Vector2 localPosition);
        return localPosition;
    }
    private Vector2 GetMousePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(threadContainer, Input.mousePosition, canvas.worldCamera, out Vector2 localPosition);
        return localPosition;
    }
}
