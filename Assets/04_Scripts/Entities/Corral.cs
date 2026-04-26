using UnityEngine;
using System.Collections.Generic;

public class Corral : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int maxEntities = 3;

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

        GameObject newEntity = Instantiate(
            entityPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        entitiesInside.Add(newEntity);
    }
}