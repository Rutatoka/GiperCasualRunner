using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 15f;

    private int targetLane = 1;
    private Vector2 startTouch;

    private void Update()
    {
        MoveForward();
        HandleSwipe();
        MoveToLane();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    void HandleSwipe()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            startTouch = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - startTouch;

            if (Mathf.Abs(delta.x) > 50)
            {
                if (delta.x > 0)
                    targetLane = Mathf.Min(2, targetLane + 1);
                else
                    targetLane = Mathf.Max(0, targetLane - 1);
            }
        }
#else
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            startTouch = touch.position;

        if (touch.phase == TouchPhase.Ended)
        {
            Vector2 delta = touch.position - startTouch;

            if (Mathf.Abs(delta.x) > 50)
            {
                if (delta.x > 0)
                    targetLane = Mathf.Min(2, targetLane + 1);
                else
                    targetLane = Mathf.Max(0, targetLane - 1);
            }
        }
#endif
    }

    void MoveToLane()
    {
        float targetX = (targetLane - 1) * laneDistance;

        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, laneChangeSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            other.GetComponent<Gem>().Collect();
        }

    }
}
