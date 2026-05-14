using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool canMove = true;

    private Transform player;
    private EntityStatus entityStatus;

    private Vector3 currentDirection;
    private Vector3 homePosition;

    private bool returnToHome = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        entityStatus = GetComponent<EntityStatus>();

        // Guarda el lugar donde apareció la entidad
        homePosition = transform.position;
    }

    void Update()
    {
        if (entityStatus != null && entityStatus.isDead)
        {
            currentDirection = Vector3.zero;
            return;
        }
        if (entityStatus == null)
        {
            currentDirection = Vector3.zero;
            return;
        }

        // Volver a su lugar al amanecer
        if (returnToHome)
        {
            MoveTo(homePosition);

            float distance = Vector3.Distance(transform.position, homePosition);

            if (distance < 0.4f)
            {
                returnToHome = false;
                currentDirection = Vector3.zero;
            }

            return;
        }

        if (player == null || !canMove)
        {
            currentDirection = Vector3.zero;
            return;
        }

        // Perseguir solo si está hostil
        if (entityStatus.currentState == "Hostil")
        {
            MoveTo(player.position);
        }
        else
        {
            currentDirection = Vector3.zero;
        }
    }

    void MoveTo(Vector3 target)
    {
        currentDirection = (target - transform.position).normalized;
        currentDirection.y = 0;

        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        if (currentDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(currentDirection),
                Time.deltaTime * 6f
            );
        }
    }

    public void ReturnToCorral()
    {
        returnToHome = true;
    }

    public bool IsMoving()
    {
        return currentDirection.magnitude > 0.1f;
    }
}