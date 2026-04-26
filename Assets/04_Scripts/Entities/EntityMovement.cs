using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform player;
    private EntityStatus entityStatus;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        entityStatus = GetComponent<EntityStatus>();
    }

    void Update()
    {
        if (player == null || entityStatus == null) return;

        if (entityStatus.currentState == "Hostil")
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}