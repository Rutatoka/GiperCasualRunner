using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject gemPrefab;

    void Start()
    {
        SpawnGem();
    }

    void SpawnGem()
    {
        if (Random.value > 0.3f)
        {
            Vector3 pos = transform.position + new Vector3(RandomLane(), 1f, 5f);
            Instantiate(gemPrefab, pos, Quaternion.identity);
        }
    }

    float RandomLane()
    {
        int lane = Random.Range(0, 3);
        return (lane - 1) * 2.5f;
    }
}
