using System.Collections;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour
{
    public Animator openAndClose;
    public bool open;
    public Transform player;
    public float interactDistance = 15f;  // Adjust this as needed

    // Reference to 3D TextMesh for prompt
    public TextMesh promptText; // Drag your 3D TextMesh object here

    void Start()
    {
        open = false;
        promptText.gameObject.SetActive(false); // Initially hide the prompt
    }

    void Update()
    {
        if (player)
        {
            // Calculate the distance from the player to the door
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist < interactDistance)
            {
                // Show the prompt when the player is within range
                if (!promptText.gameObject.activeSelf)
                {
                    promptText.gameObject.SetActive(true);
                }

                // Rotate the prompt text to always face the camera (optional)
                promptText.transform.LookAt(player); // This makes the text face the player

                // Check for 'E' press to open/close the door
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
            else
            {
                // Hide the prompt when out of range
                if (promptText.gameObject.activeSelf)
                {
                    promptText.gameObject.SetActive(false);
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
