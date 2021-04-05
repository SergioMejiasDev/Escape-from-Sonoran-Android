using UnityEngine;

/// <summary>
/// Class that takes care of the horizontal movement of the background in the last battle.
/// </summary>
public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0, end = 0, begin = 0;
    
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        if (transform.position.x <= end)
        {
            Vector2 startPosition = new Vector2(begin, transform.position.y);
            transform.position = startPosition;
        }
    }
}
