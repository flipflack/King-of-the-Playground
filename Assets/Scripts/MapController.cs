using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject MapCanvasMini;
    //public Canvas MapCanvasMini;
    public bool open = false;
    
   public void ToggleMap()
   {
        //need to stop player movement later
        if (!open)
        {
            MapCanvasMini.SetActive(true);  
            open = true;
        }
        else
        {
            MapCanvasMini.SetActive(false);
            open = false;
        }
   }
    public void Slide()
    {
        SceneManager.LoadScene("Numbers Slide");
    }
    public void MonkeyBar()
    {
        SceneManager.LoadScene("Wrestling Monkeybars");
    }
    public void Swings()
    {
        SceneManager.LoadScene("Words Swings");
    }
    public void Sandbox()
    {
        SceneManager.LoadScene("(Tutorial) Marble Sandbox");
    }
    public void Roundabout()
    {
        SceneManager.LoadScene("Plushie Roundabout");
    }
    public void Tower()
    {
        SceneManager.LoadScene("The Tower");
    }
}
