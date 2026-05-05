using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject treePrefab;
    public Transform player;

    public int rowsOnScreen = 6;
    public float rowLength = 10f;

    private float spawnZ = 0;
    private Queue<GameObject> rows = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < rowsOnScreen; i++)
            SpawnRow();
    }

    void Update()
    {
        if (player.position.z - 20 > spawnZ - rowsOnScreen * rowLength)
        {
            SpawnRow();
            DeleteRow();
        }
    }

    void SpawnRow()
    {
        GameObject row = new GameObject("Row");
        row.transform.position = Vector3.forward * spawnZ;

        // деревья ближе
        Create(treePrefab, new Vector3(-4f, 0, 0), row.transform, true);
        Create(treePrefab, new Vector3(4f, 0, 0), row.transform, true);

        // здания дальше
        Create(buildingPrefab, new Vector3(-8f, 0, 0), row.transform, false);
        Create(buildingPrefab, new Vector3(-12f, 0, 0), row.transform, false);
        Create(buildingPrefab, new Vector3(8f, 0, 0), row.transform, false);
        Create(buildingPrefab, new Vector3(12f, 0, 0), row.transform, false);

        rows.Enqueue(row);
        spawnZ += rowLength;
    }

    void DeleteRow()
    {
        Destroy(rows.Dequeue());
    }

    void Create(GameObject prefab, Vector3 localPos, Transform parent, bool isTree)
    {
        GameObject obj = Instantiate(prefab, parent);
        obj.transform.localPosition = localPos;

        float h = isTree ? Random.Range(2.5f, 5f) : Random.Range(3f, 8f);
        float scaleXZ = isTree ? Random.Range(0.8f, 1.2f) : 1f;

        obj.transform.localScale = new Vector3(scaleXZ, h, scaleXZ);

        var renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Color baseColor = isTree ? new Color(0.2f, 0.6f, 0.2f) : Color.gray;
            float v = Random.Range(-0.1f, 0.1f);
            renderer.material.color = baseColor + new Color(v, v, v);
        }
    }
}