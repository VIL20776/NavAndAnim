using UnityEngine;

public class GuardManager : MonoBehaviour
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
}
