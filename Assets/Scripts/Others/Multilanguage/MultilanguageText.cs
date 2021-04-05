using UnityEngine;

/// <summary>
/// Asset that will allow us to write the texts of the game in different languages.
/// </summary>
[CreateAssetMenu(menuName = "My Assets/Multilanguage Text")]
public class MultilanguageText : ScriptableObject
{
    [TextArea(14, 10)] public string english = null;
    [TextArea(14, 10)] public string spanish = null;
}
