using UnityEngine;
using UnityEngine.UI;

public class AudioSettingUI : MonoBehaviour
{
    public AudioSettingManager audioSettings;
    public Slider master;
    public Slider music;
    public Slider sfx;

    void OnEnable()
    {
        master.SetValueWithoutNotify(audioSettings.GetMaster());
        music.SetValueWithoutNotify(audioSettings.GetMusic());
        sfx.SetValueWithoutNotify(audioSettings.GetSfx());
    }
}
