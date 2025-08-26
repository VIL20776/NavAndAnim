using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Guard[] guards;
    [SerializeField] private Transform objective;

    public void GuardCall()
    {
        foreach (var guard in guards)
        {
            guard.SetDestination(objective.position);
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
