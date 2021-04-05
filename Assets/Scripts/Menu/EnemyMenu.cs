using UnityEngine;

/// <summary>
/// Class in charge of moving the enemies that appear in the main menu.
/// </summary>
public class EnemyMenu : MonoBehaviour
{
    float speed = 2;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
