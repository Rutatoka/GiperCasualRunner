using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int tilesOnScreen = 6;
    public float tileLength = 10f;
    public Transform player;

    private float spawnZ = 0;
    private Queue<GameObject> tiles = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
            SpawnTile();
    }

    void Update()
    {
        if (player.position.z - 20 > spawnZ - tilesOnScreen * tileLength)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        tiles.Enqueue(tile);
        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        Destroy(tiles.Dequeue());
    }
}
