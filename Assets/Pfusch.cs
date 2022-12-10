using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public sealed class Pfusch : MonoBehaviour
{
	static Pfusch()
	{
        AudioListener.volume *= 0.2f;
    }
}
