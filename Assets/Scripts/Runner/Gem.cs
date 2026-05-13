using UnityEngine;

public class Gem : MonoBehaviour
{
    private bool collected = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // если игрок уже ПРОЕХАЛ гем
        if (!collected && player.position.z > transform.position.z +5f)
        {
            Missed();
        }
    }

    public void Collect()
    {
        if (collected) return;

        collected = true;
        HappinessSystem.Instance.Add(1);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateDailyTaskProgress("Заработай монет", 1);
        }
        Destroy(gameObject);
    }

    void Missed()
    {
        collected = true;
        HappinessSystem.Instance.Add(-1);
        Destroy(gameObject);
    }
}
