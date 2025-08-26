using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlaySelect()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitSelect()
    {
        Application.Quit();
    }
}
