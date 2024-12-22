using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnimator;    // Reference to the door's Animator component
    public float interactionDistance = 3f;  // Distance to interact with the door
    private Transform player;         // Reference to the player's transform

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // Assuming your player has the "Player" tag
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Check the distance between player and door
        float distance = Vector3.Distance(player.position, transform.position);

        // If within interaction range and pressing "E"
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            doorAnimator.SetTrigger("OpenDoorTrigger");
        }
    }
}
