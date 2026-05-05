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
        // åñëè èãðîê óæå ÏÐÎÅÕÀË ãåì
        if (!collected && player.position.z > transform.position.z +5f)
        {
            Missed();
        }
    }

    public void Collect()
    {
        if (collected) return;

        collected = true;
        HappinessSystem.Instance.Add(5);
        Destroy(gameObject);
    }

    void Missed()
    {
        collected = true;
        HappinessSystem.Instance.Add(-3);
        Destroy(gameObject);
    }
}
