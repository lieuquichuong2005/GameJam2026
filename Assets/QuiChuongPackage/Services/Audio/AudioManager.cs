using UnityEngine;

public class AudioService : IAudioService
{
    private readonly AudioConfig config;
    private readonly AudioSource sfx;
    private readonly AudioSource bgm;

    public AudioService(
        AudioConfig config,
        AudioSourceProvider provider
    )
    {
        this.config = config;
        sfx = provider.sfx;
        bgm = provider.bgm;
    }

    public void Play(AudioId id)
    {
        var entry = config.Get(id);
        if (entry == null) return;

        sfx.PlayOneShot(entry.clip, entry.volume);
    }

    public void PlayBgm(AudioId id)
    {
        var entry = config.Get(id);
        if (entry == null) return;

        bgm.clip = entry.clip;
        bgm.volume = entry.volume;
        bgm.loop = entry.loop;
        bgm.Play();
    }

    public void StopBgm()
    {
        bgm.Stop();
    }
}