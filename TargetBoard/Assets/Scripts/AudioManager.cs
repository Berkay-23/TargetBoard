using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {   
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume =  s.volume * PlayerPrefs.GetFloat("EffectVolume");
        s.source.Play();
    }

    public IEnumerator FadeInAndPlay(string name, double maxVolume, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        s.source.pitch = pitch;

        float fadeInSpeed = 0.1f;
        s.source.volume = 0f;

        while (s.source.volume < maxVolume * PlayerPrefs.GetFloat("MusicVolume"))
        {
            s.source.volume += Time.deltaTime * fadeInSpeed;
            yield return null;
        }

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public IEnumerator FadeInAndStop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        float fadeInSpeed = 0.5f;

        while (s.source.volume > 0f)
        {
            s.source.volume -= Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        s.source.Stop();
    }
}
