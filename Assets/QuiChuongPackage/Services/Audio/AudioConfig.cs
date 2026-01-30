using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

[System.Serializable]
public class AudioEntry
{
    [SerializeField] private AudioId id;

    public AudioId Id => id; // readonly runtime access

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;

    public bool loop;

#if UNITY_EDITOR
    public void Editor_SetId(AudioId newId)
    {
        id = newId;
    }
#endif
}


[CreateAssetMenu(
    fileName = "AudioConfig",
    menuName = "Config/Audio Config"
)]
public class AudioConfig : ScriptableObject
{
    [SerializeField] private List<AudioEntry> entries = new();

    private Dictionary<AudioId, AudioEntry> lookup;

    public AudioEntry Get(AudioId id)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<AudioId, AudioEntry>();
            foreach (var e in entries)
                lookup[e.Id] = e;
        }

        lookup.TryGetValue(id, out var entry);
        return entry;
    }

#if UNITY_EDITOR
    public List<AudioEntry> EditorEntries => entries;
#endif
}