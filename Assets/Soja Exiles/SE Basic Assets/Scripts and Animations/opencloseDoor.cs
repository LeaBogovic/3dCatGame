using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour
{
    public Animator openAndClose;
    public bool open;
    public Transform player;
    public float interactDistance = 15f;  // Adjust this as needed

    void Start()
    {
        open = false;
    }

    void Update()
    {
        if (player)
        {
            float dist = Vector3.Distance(player.position, transform.position);
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
        Debug.Log("You are opening the door");
        openAndClose.Play("Opening");
        open = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Closing()
    {
        Debug.Log("You are closing the door");
        openAndClose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(0.5f);
    }
}
