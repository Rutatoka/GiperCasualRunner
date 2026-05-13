using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public GameObject[] treePrefabs;
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

        // 🌳 Деревья — фиксированная X, но могут смещаться по Z в пределах ряда
        GameObject randomTree = treePrefabs[Random.Range(0, treePrefabs.Length)];
        Create(randomTree, new Vector3(-4f, 3f, Random.Range(-3f, 3f)), row.transform, true);
        Create(randomTree, new Vector3(4f,3f, Random.Range(-3f, 3f)), row.transform, true);

        // 🏠 Дома — теперь идут ВДОЛЬ дороги (по Z)
        // Левая сторона (X = -10f)
        for (float z = -5f; z <= 5f; z += 3f)
        {
            GameObject randomBuilding = buildingPrefabs[Random.Range(1, buildingPrefabs.Length)];
            Create(randomBuilding, new Vector3(-8f, 0f, z), row.transform, false);
        }

        // Правая сторона (X = 10f)
        for (float z = -5f; z <= 5f; z += 3f)
        {
            GameObject randomBuilding = buildingPrefabs[Random.Range(1, buildingPrefabs.Length)];
            Create(randomBuilding, new Vector3(8f, 0f, z), row.transform, false);
        }

        for (float z = -5f; z <= 5f; z += 3f)
        {
            GameObject randomBuilding = buildingPrefabs[0];
            Create(randomBuilding, new Vector3(14f, 0f, z), row.transform, false);
        }
        for (float z = -5f; z <= 5f; z += 3f)
        {
            GameObject randomBuilding = buildingPrefabs[0];
            Create(randomBuilding, new Vector3(-14f, 0f, z), row.transform, false);
        }

        rows.Enqueue(row);
        spawnZ += rowLength;
    }

    void DeleteRow()
    {
        if (rows.Count > 0)
            Destroy(rows.Dequeue());
    }

    void Create(GameObject prefab, Vector3 localPos, Transform parent, bool isTree)
    {
        GameObject obj = Instantiate(prefab, parent);
        obj.transform.localPosition = localPos;
        if (!isTree)
        {
            // 🔄 Зеркальное отражение через scale
            Vector3 scale = obj.transform.localScale;
            if (localPos.x > 0) // слева — смотрит вправо
                scale.x = Mathf.Abs(scale.x); // положительный
            else if (localPos.x < 0) // справа — смотрит влево
                scale.x = -Mathf.Abs(scale.x); // отрицательный
            obj.transform.localScale = scale;
        }

    }
}