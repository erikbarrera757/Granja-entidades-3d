using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    void Update()
    {
        if (animator == null || controller == null) return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        bool isWalking = horizontalVelocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }
}