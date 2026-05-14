using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 2f;
    public float attackDistance = 1.5f;

    private float lastAttackTime;
    private Transform player;
    private EntityStatus status;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        status = GetComponent<EntityStatus>();
    }

    void Update()
    {
        if (player == null || status == null) return;

        if (status.currentState != "Hostil") return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseStability(damage);
            }

            lastAttackTime = Time.time;

            Debug.Log("La entidad atacó al jugador.");
        }
    }
}