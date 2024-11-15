using System.Collections;
using UnityEngine;

public class OpenCloseDoor1 : MonoBehaviour
{
    public Animator openAndClose;
    public bool open;
    public Transform player;
    public float interactDistance = 15f; // Adjust this as needed

    void Start()
    {
        open = false;
    }

    void Update()
    {
        if (player)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            Debug.Log("Distance to door 2: " + dist); // Debugging distance
            if (dist < interactDistance)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!open)
                    {
                        StartCoroutine(Opening());
                    }
                    else
                    {
                        StartCoroutine(Closing());
                    }
                }
            }
        }
    }

    IEnumerator Opening()
    {
        Debug.Log("You are opening door 2");
        openAndClose.Play("Opening1"); // Directly play the "Opening" animation
        open = true;
        yield return new WaitForSeconds(0.5f); // Adjust wait time as needed
    }

    IEnumerator Closing()
    {
        Debug.Log("You are closing door 2");
        openAndClose.Play("Closing1"); // Directly play the "Closing" animation
        open = false;
        yield return new WaitForSeconds(0.5f); // Adjust wait time as needed
    }
}
