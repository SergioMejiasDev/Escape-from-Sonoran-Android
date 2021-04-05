using UnityEngine;

/// <summary>
/// Moving platforms class.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public bool isVertical;
    public int direction = 1;
    Vector3 startingPosition;
    [SerializeField] float speed = 0;
    [SerializeField] float movementDistance = 0;
    
    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (!isVertical)
        {
            transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector2.up * speed * direction * Time.deltaTime);
        }

        ChangeDirection();
    }

    /// <summary>
    /// Function where the platform changes direction.
    /// </summary>
    void ChangeDirection()
    {
        if (!isVertical)
        {
            if ((transform.position.x) > startingPosition.x + movementDistance)
            {
                direction = -1;
            }

            else if ((transform.position.x) < startingPosition.x - movementDistance)
            {
                direction = 1;
            }
        }
        
        else
        {
            if ((transform.position.y) > startingPosition.y + movementDistance)
            {
                direction = -1;
            }

            else if ((transform.position.y) < startingPosition.y - movementDistance)
            {
                direction = 1;
            }
        }
    }
}