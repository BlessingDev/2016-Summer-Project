using UnityEngine;
using System.Collections.Generic;


public class SoundManager : Manager<SoundManager>
{
    private Dictionary<string, AudioClip> preloadedAudio;

    private Dictionary<string, AudioSource> backgroundMusics;
    private Dictionary<string, AudioSource> effects;

    static private int curLevel;

	// Use this for initialization
	void Start ()
    {
        if(!inited)
        {
            Init();
            preloadedAudio = new Dictionary<string, AudioClip>();
            effects = new Dictionary<string, AudioSource>();
            backgroundMusics = new Dictionary<string, AudioSource>();

            var audios = Resources.LoadAll<AudioClip>("Sounds/Effects/");
            for (int i = 0; i < audios.Length; i += 1)
            {
                preloadedAudio.Add(audios[i].name, audios[i]);
            }

            audios = Resources.LoadAll<AudioClip>("Sounds/Backgrounds/");
            for (int i = 0; i < audios.Length; i += 1)
            {
                preloadedAudio.Add(audios[i].name, audios[i]);
            }
        }
    }
	
    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (!inited)
            Start();

        PlayBackgroundMusicByLevel(level);
    }

    private void PlayBackgroundMusicByLevel(int level)
    {
        if(level == SceneManager.Instance.GetLevel("MainScene"))
        {
            StopAllBackground();
            PlayBackground("Light_Rain");
            PlayBackground("Prelude_No_19");
        }
        else if(level == SceneManager.Instance.GetLevel("GameScene"))
        {
            StopAllBackground();
            PlayBackground("Tuba_Waddle");
        }
    }

    public bool PlayBackground(string fileName)
    {
        if(backgroundMusics.ContainsKey(fileName))
        {
            Debug.LogWarning("Already Playing Background " + fileName);
            StopBackground(fileName);
        }
        if(!preloadedAudio.ContainsKey(fileName))
        {
            Debug.LogError("NOT Loaded Music File " + fileName);
            return false;
        }

        AudioClip clip;
        preloadedAudio.TryGetValue(fileName, out clip);

        GameObject obj = new GameObject(fileName, new System.Type[] { typeof(AudioSource) });
        DontDestroyOnLoad(obj);
        AudioSource source = obj.GetComponent<AudioSource>();
        backgroundMusics.Add(fileName, source);

        source.clip = clip;
        source.loop = true;
        source.Play();
        return true;
    }

    public bool StopBackground(string fileName)
    {
        if (!backgroundMusics.ContainsKey(fileName))
        {
            Debug.LogWarning("Already Playing Background " + fileName);
            return false;
        }

        AudioSource source;
        backgroundMusics.TryGetValue(fileName, out source);
        Destroy(source.gameObject);

        return true;
    }

    public void StopAllBackground()
    {
        foreach(var iter in backgroundMusics)
        {
            Destroy(iter.Value.gameObject);
        }

        backgroundMusics.Clear();
    }

    public bool SetEffect(string fileName)
    {
        if (effects.ContainsKey(fileName))
        {
            Debug.LogWarning("Already Playing Background " + fileName);
            return false;
        }
        if (!preloadedAudio.ContainsKey(fileName))
        {
            Debug.LogError("NOT Loaded Music File " + fileName);
            return false;
        }

        AudioClip clip;
        preloadedAudio.TryGetValue(fileName, out clip);

        GameObject obj = new GameObject(fileName, new System.Type[] { typeof(AudioSource) });
        AudioSource source = obj.GetComponent<AudioSource>();
        effects.Add(fileName, source);

        source.clip = clip;
        return true;
    }

    public bool PlayEffect(string fileName)
    {
        if (!effects.ContainsKey(fileName))
        {
            Debug.LogWarning("NOT Found Setted Effect " + fileName);
            return false;
        }

        AudioSource source;
        effects.TryGetValue(fileName, out source);
        if(!source.isPlaying)
            source.Play();

        return true;
    }
}
