using UnityEngine.Audio;
using UnityEngine;

//Make attributes appear in the inspector
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volumn;

    [Range(.1f, 3f)]
    public float pitch;

    // It will not show in the inspector, eventhough it's public
    [HideInInspector]
    public AudioSource source;
}
