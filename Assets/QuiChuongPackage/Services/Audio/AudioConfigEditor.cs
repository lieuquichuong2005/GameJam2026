#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(AudioConfig))]
public class AudioConfigEditor : Editor
{
    private AudioConfig config;

    private void OnEnable()
    {
        config = (AudioConfig)target;
        SyncWithEnum();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "AudioConfig được đồng bộ theo AudioId enum.\n" +
            "AudioId là READ-ONLY, không thể thêm hoặc xóa entry.",
            MessageType.Info
        );

        DrawEntries();
    }

    private void DrawEntries()
    {
        var entries = config.EditorEntries;

        EditorGUI.BeginDisabledGroup(true);
        foreach (var entry in entries)
        {
            EditorGUILayout.BeginVertical("box");

            // 🔑 HIỂN THỊ KEY
            EditorGUILayout.EnumPopup("Audio Id", entry.Id);

            EditorGUI.EndDisabledGroup();

            // Các field còn lại edit bình thường
            entry.clip = (AudioClip)EditorGUILayout.ObjectField(
                "Clip", entry.clip, typeof(AudioClip), false);

            entry.volume = EditorGUILayout.Slider("Volume", entry.volume, 0f, 1f);
            entry.loop = EditorGUILayout.Toggle("Loop", entry.loop);

            EditorGUILayout.EndVertical();
        }
    }

    private void SyncWithEnum()
    {
        var enumValues = Enum.GetValues(typeof(AudioId))
            .Cast<AudioId>()
            .ToList();

        var entries = config.EditorEntries;

        // Add missing
        foreach (var id in enumValues)
        {
            if (!entries.Any(e => e.Id == id))
            {
                var e = new AudioEntry();
                e.Editor_SetId(id);
                entries.Add(e);
            }
        }

        // Remove orphan
        entries.RemoveAll(e => !enumValues.Contains(e.Id));

        // Sort
        entries.Sort((a, b) => a.Id.CompareTo(b.Id));

        EditorUtility.SetDirty(config);
    }
}
#endif