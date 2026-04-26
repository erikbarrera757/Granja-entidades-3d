using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 2f;
    public float attackDistance = 1.5f;

    private float lastAttackTime;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        EntityStatus status = GetComponent<EntityStatus>();
        if (status == null) return;

        if (status.currentState != "Hostil") return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            GameManager.Instance.LoseStability(damage);

            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                player.position = new Vector3(0, 1, 0);
                cc.enabled = true;
            }
            else
            {
                player.position = new Vector3(0, 1, 0);
            }

            lastAttackTime = Time.time;
            Debug.Log("La entidad atacó al jugador.");
        }
    }
}