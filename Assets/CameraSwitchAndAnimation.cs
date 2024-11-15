using System.Collections;
using UnityEngine;

public class CameraSwitchAndAnimation : MonoBehaviour
{
    public Animator animator;
    public Camera mainCamera;
    public Camera interactionCamera;
    public Transform player;
    public GameObject interactionObject;
    public float interactionRange = 5f;  // Range to trigger interaction
    public string animationName = "walk_drink";  // Name of the read animation

    private bool isInRange = false;
    private bool isAnimating = false;  // Flag to prevent triggering the animation multiple times

    void Update()
    {
        // Check if the player is in range of the interaction object
        if (Vector3.Distance(player.position, interactionObject.transform.position) < interactionRange)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }

        // If the player is in range and presses "F", and no animation is currently playing
        if (isInRange && Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            StartCoroutine(SwitchCameraAndPlayAnimation());
        }
    }

    private IEnumerator SwitchCameraAndPlayAnimation()
    {
        // Prevent starting the animation again until it's finished
        isAnimating = true;

        // Switch to the interaction camera
        mainCamera.gameObject.SetActive(false);
        interactionCamera.gameObject.SetActive(true);

        // Trigger the animation (ensure you pass the animation hash or string)
        animator.SetTrigger(animationName);  // Trigger the animation using a trigger

        // Wait for the animation to finish by checking the current animation state
        yield return new WaitUntil(() =>
        {
            // Check if the animation is still playing or has finished
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
        });

        // Switch back to the main camera
        interactionCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Reset the flag
        isAnimating = false;
    }
}
