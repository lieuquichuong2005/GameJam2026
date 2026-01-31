using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Exposed parameter names")]
    public string masterParam = "MasterVol";
    public string musicParam = "MusicVol";
    public string sfxParam = "SfxVol";

    const string PREF_MASTER = "audio_master";
    const string PREF_MUSIC = "audio_music";
    const string PREF_SFX = "audio_sfx";

    // Slider values are 0..1
    public void SetMaster(float value01) => SetVolume(masterParam, PREF_MASTER, value01);
    public void SetMusic(float value01) => SetVolume(musicParam, PREF_MUSIC, value01);
    public void SetSfx(float value01) => SetVolume(sfxParam, PREF_SFX, value01);

    public float GetMaster() => PlayerPrefs.GetFloat(PREF_MASTER, 1f);
    public float GetMusic() => PlayerPrefs.GetFloat(PREF_MUSIC, 1f);
    public float GetSfx() => PlayerPrefs.GetFloat(PREF_SFX, 1f);

    void Awake()
    {
        // Apply saved settings on startup
        ApplySaved();
    }

    public void ApplySaved()
    {
        SetVolume(masterParam, PREF_MASTER, GetMaster(), save: false);
        SetVolume(musicParam, PREF_MUSIC, GetMusic(), save: false);
        SetVolume(sfxParam, PREF_SFX, GetSfx(), save: false);
    }

    void SetVolume(string mixerParam, string prefKey, float value01, bool save = true)
    {
        value01 = Mathf.Clamp01(value01);

        // Convert linear [0..1] to dB.
        // 1.0 => 0 dB, 0.0 => -80 dB (silent)
        float db = (value01 > 0.0001f) ? Mathf.Log10(value01) * 20f : -80f;

        if (mixer != null)
            mixer.SetFloat(mixerParam, db);

        if (save)
        {
            PlayerPrefs.SetFloat(prefKey, value01);
            PlayerPrefs.Save();
        }
    }
}
