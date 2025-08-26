using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent => GetComponent<NavMeshAgent>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Collision");
        if (collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("SampleScene 1");
        }
    }
}
