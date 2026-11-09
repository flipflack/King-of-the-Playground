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
    public void mainButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void countButton()
    {
        SceneManager.LoadScene("PatternMG");
    }
    public void wordButton()
    {
        SceneManager.LoadScene("WordMG");
    }
    public void sheepButton()
    {
        SceneManager.LoadScene("SheepMG");
    }
}
