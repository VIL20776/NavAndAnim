using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject mainPanel;

    void Start()
    {
        SceneManager.LoadScene("LoadScreen", LoadSceneMode.Additive);
    }

    public void PlaySelect()
    {
        SceneHandler.instance.LoadScene(1);
    }

    public void LoadScene1()
    {
        SceneHandler.instance.LoadScene(1);
    }
    public void LoadScene2()
    {
        SceneHandler.instance.LoadScene(2);
    }
    public void LoadScene3()
    {
        SceneHandler.instance.LoadScene(3);
    }

    public void LevelSelect()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        levelPanel.SetActive(!levelPanel.activeSelf);
    }

    public void QuitSelect()
    {
        Application.Quit();
    }
}
