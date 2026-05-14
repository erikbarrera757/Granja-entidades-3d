using UnityEngine;

public class EntityAnimationController : MonoBehaviour
{
    public EntityStatus entityStatus;
    public EntityMovement movement;
    public Animator animator;

    public float attackDistance = 1.5f;
    public float attackCooldown = 2f;

    private Transform player;
    private float attackTimer = 0f;
    private bool deadTriggered = false;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (entityStatus != null && entityStatus.isDead)
        {
            if (!deadTriggered)
            {
                animator.SetTrigger("die");
                deadTriggered = true;
            }

            return;
        }
        if (animator == null || entityStatus == null) return;

        attackTimer -= Time.deltaTime;

        bool isHostile = entityStatus.currentState == "Hostil";
        bool isNearPlayer = false;

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            isNearPlayer = distance <= attackDistance;
        }

        bool isWalking = false;

        if (movement != null)
        {
            isWalking = movement.IsMoving();
        }

        animator.SetBool("isWalking", isWalking);

        if (movement != null)
        {
            movement.canMove = isHostile && !isNearPlayer;
        }

        if (isHostile && isNearPlayer && attackTimer <= 0f)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;
        }
    }
}