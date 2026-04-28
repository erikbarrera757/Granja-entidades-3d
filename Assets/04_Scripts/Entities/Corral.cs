using UnityEngine;
using System.Collections.Generic;

public class Corral : MonoBehaviour
{
    public string corralName = "Corral 1";

    public Transform[] spawnPoints;
    public int maxEntities = 3;

    public int level = 1;
    public int maxLevel = 3;

    private List<GameObject> entitiesInside = new List<GameObject>();

    public bool HasSpace()
    {
        return entitiesInside.Count < maxEntities;
    }

    public void AddEntity(GameObject entityPrefab)
    {
        if (!HasSpace()) return;

        int index = entitiesInside.Count;
        Transform spawnPoint = spawnPoints[index];

        GameObject newEntity = Instantiate(entityPrefab, spawnPoint.position, spawnPoint.rotation);
        entitiesInside.Add(newEntity);

        EntityStatus status = newEntity.GetComponent<EntityStatus>();
        if (status != null)
        {
            status.currentCorral = this;
        }
    }

    public int GetUpgradeCost()
    {
        if (level >= maxLevel) return 0;

        return level * 50;
    }

    public bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        level++;
        Debug.Log(corralName + " subió a nivel " + level);
    }
}