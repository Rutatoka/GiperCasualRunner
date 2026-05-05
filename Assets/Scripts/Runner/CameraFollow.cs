using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float height = 5f;
    public float forwardOffset = 6f;

    public float smooth = 10f;

    public float maxXOffset = 2.5f;

    void LateUpdate()
    {
        float targetX = Mathf.Lerp(transform.position.x, player.position.x, smooth * Time.deltaTime);


        Vector3 targetPos = new Vector3(
            targetX,
            height,
            player.position.z - forwardOffset
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, smooth * Time.deltaTime);
    }
}
