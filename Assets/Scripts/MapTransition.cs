using Unity.Cinemachine;
using UnityEngine;


public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D Mapboundaries;
    [SerializeField] private Direction direction;
    [SerializeField] private float updatepos = 2f;

    CinemachineConfiner2D confiner;

    enum Direction { Up, Down, Left, Right };

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = Mapboundaries;
            UpdatePlayerPos(collision.gameObject);
        }
    }

    private void UpdatePlayerPos(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += updatepos;
                break;
            case Direction.Down:
                newPos.y -= updatepos;
                break;
            case Direction.Left:
                newPos.x -= updatepos;
                break;
            case Direction.Right:
                newPos.x += updatepos;
                break;
        }

        confiner.BoundingShape2D = Mapboundaries;
        player.transform.position = newPos;
    }


}
