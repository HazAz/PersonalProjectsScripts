using UnityEngine;

/// <summary>
/// Script that changes the color of the character.
/// </summary>
public class CharacterColorChanger : MonoBehaviour
{
    public void GenerateColor()
    {
        GetComponent<Renderer>().sharedMaterial.color = Random.ColorHSV();
    }

    public void Reset()
    {
        GetComponent<Renderer>().sharedMaterial.color= Color.white;
    }
}
