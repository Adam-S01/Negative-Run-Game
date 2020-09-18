using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public GameObject MusicPrefab;
    public static float volumeGainGlobal = 0.7f;
    public Sound[] sounds;
    public static AudioManager instance;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
        else
        {
            Debug.LogError(this + " in " + gameObject.name + " has be destroyed because another " + this + " already exists");
            Destroy(gameObject);
            return;
        }
        foreach (Sound s in sounds)
        {
            if(s.source == null)
            {
                s.source = FindObjectOfType<Camera>().gameObject.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;
            s.source.playOnAwake = s.playOnAwake;
            s.source.volume = volumeGainGlobal;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        //MusicPrefab = Resources.Load<GameObject>("Prefab/Musique");
    }

    public void Start()
    {
        SpawnMusic();
        foreach (Sound s in sounds)
        {
            if(s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    public void SpawnMusic()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1){ 
            Instantiate(MusicPrefab);
        }
        else
        {
            Debug.Log("Could not spawn music");
            return;
        }
    }
    public void ChangeGainValue(Single param)
    {
        volumeGainGlobal = param;
        foreach (Sound s in sounds)
        {
            s.source.volume = volumeGainGlobal;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    #region Edit Sound

    public void PitchChange(string name, float _pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.pitch = _pitch;
    }

    public void VolumeChange(string name, float _volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.volume = _volume;
    }
    #endregion
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    
    [HideInInspector] public AudioSource source;
    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(.1f, 3f)] public float pitch = 1f;
    public bool playOnAwake;
    public bool loop;
}

