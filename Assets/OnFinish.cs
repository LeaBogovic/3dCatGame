using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinish : StateMachineBehaviour
{
    [SerializeField] private string animation; // The name of the animation to play

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Convert the animation name to its hash
        int animationHash = Animator.StringToHash(animation);

        // Get the PlayerMovement1 component
        PlayerMovement1 playerMovement = animator.GetComponent<PlayerMovement1>();

        if (playerMovement != null)
        {
            // Change the animation using the hash
            playerMovement.ChangeAnimation(animationHash, 0.2f, stateInfo.length);
        }
        else
        {
            Debug.LogWarning("PlayerMovement1 component not found on the GameObject!");
        }
    }
}
