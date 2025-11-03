using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance { get; private set; }
    [SerializeField] private GameObject loadingCanvas;

    private int currentScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(loadingCanvas);
        loadingCanvas.SetActive(false);
    }

    IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneIndex);
        asyncScene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);
        //SceneManager.UnloadSceneAsync(currentScene);
        while (asyncScene.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);

        asyncScene.allowSceneActivation = true;
        loadingCanvas.SetActive(false);
        //currentScene = sceneIndex;
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneWithDelay(sceneIndex));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneWithDelay((SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCountInBuildSettings - 1)));
    }

}
