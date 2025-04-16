using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector2 offset;

    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, gameObject.transform.position.z);
    }
}
