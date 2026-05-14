using UnityEngine;
using System.Collections.Generic;

public class Corral : MonoBehaviour
{
    [Header("Modelos visuales")]
    public GameObject level1Model;
    public GameObject level2Model;
    public GameObject level3Model;

    public string corralName = "Corral 1";
    public AudioSource hammerAudio;

    public Transform[] spawnPoints;
    public int maxEntities = 3;

    public int level = 1;
    public int maxLevel = 3;

    private List<GameObject> entitiesInside = new List<GameObject>();

    void Start()
    {
        UpdateVisual();
    }

    public bool HasSpace()
    {
        CleanNullEntities();
        return entitiesInside.Count < maxEntities;
    }

    public void AddEntity(GameObject entityPrefab)
    {
        CleanNullEntities();

        if (!HasSpace()) return;

        int index = entitiesInside.Count;

        if (spawnPoints == null || spawnPoints.Length == 0 || index >= spawnPoints.Length)
        {
            Debug.LogWarning(corralName + " no tiene suficientes spawn points.");
            return;
        }

        Transform spawnPoint = spawnPoints[index];

        GameObject newEntity = Instantiate(entityPrefab, spawnPoint.position, spawnPoint.rotation);
        entitiesInside.Add(newEntity);

        EntityStatus status = newEntity.GetComponent<EntityStatus>();

        if (status != null)
        {
            status.currentCorral = this;
        }
    }

    public void RemoveEntity(GameObject entity)
    {
        if (entitiesInside.Contains(entity))
        {
            entitiesInside.Remove(entity);
        }
    }

    void CleanNullEntities()
    {
        entitiesInside.RemoveAll(entity => entity == null);
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

        UpdateVisual();

        if (hammerAudio != null)
        {
            hammerAudio.Play();
        }

        Debug.Log(corralName + " subió a nivel " + level);
    }

    void UpdateVisual()
    {
        if (level1Model != null)
            level1Model.SetActive(level == 1);

        if (level2Model != null)
            level2Model.SetActive(level == 2);

        if (level3Model != null)
            level3Model.SetActive(level == 3);
    }
}