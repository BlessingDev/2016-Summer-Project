using UnityEngine;
using System.Collections.Generic;


public class SoundManager : Manager<SoundManager>
{
    private Dictionary<string, AudioClip> preloadedAudio;

    private Dictionary<string, AudioSource> backgroundMusics;
    private Dictionary<string, AudioSource> effects;

    private List<AudioSource> effectPool;

    private bool soundOff = false;
    public bool IsSoundOff
    {
        get
        {
            return soundOff;
        }
        set
        {
            soundOff = value;

            if (value)
            {
                StopAllBackground();
                StopAllEffects();
            }
            else
            {
                PlayBackgroundMusicByLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    static private int curLevel = -1;

    // Use this for initialization
    void Start()
    {
        if (!inited)
        {
            Init();
            preloadedAudio = new Dictionary<string, AudioClip>();
            effects = new Dictionary<string, AudioSource>();
            backgroundMusics = new Dictionary<string, AudioSource>();
            effectPool = new List<AudioSource>();

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

            var ins = Instance;
        }
    }

    void Update()
    {
        var arr = effectPool.ToArray();
        for (int i = 0; i < arr.Length; i += 1)
        {
            if(!arr[i].isPlaying)
            {
                effectPool.Remove(arr[i]);
                Destroy(arr[i].gameObject);
            }
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (!destroyReserved)
        {
            if (!inited)
                Start();

            ClearEffects();

            if (curLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                curLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                PlayBackgroundMusicByLevel(level);
            }
        }
    }

    private void PlayBackgroundMusicByLevel(int level)
    {
        if (level == SceneManager.Instance.GetLevel("MainScene"))
        {
            StopAllBackground();
            PlayBackground("Light_Rain");
            PlayBackground("Prelude_No_19");
        }
        else if (level == SceneManager.Instance.GetLevel("GameScene"))
        {
            StopAllBackground();
            PlayBackground("Tuba_Waddle");
        }
        else if (level == SceneManager.Instance.GetLevel("ConversationScene"))
        {
            StopAllBackground();
            PlayBackground("End_of_Summer");
        }
    }

    public bool PlayBackground(string fileName)
    {
        if(!soundOff)
        {
            if (backgroundMusics.ContainsKey(fileName))
            {
                Debug.LogWarning("Already Playing Background " + fileName);
                StopBackground(fileName);
            }
            if (!preloadedAudio.ContainsKey(fileName))
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

        return false;
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
        if (backgroundMusics != null)
        {
            foreach (var iter in backgroundMusics)
            {
                Destroy(iter.Value.gameObject);
            }

            backgroundMusics.Clear();
        }
    }

    public bool PlayEffectForce(string fileName)
    {
        if(!soundOff)
        {
            AudioSource source;
            effects.TryGetValue(fileName, out source);
            PlayPoolSound(fileName);

            return true;
        }
        return false;
    }

    public void PlayEffect(string musicName)
    {
        if(!soundOff)
        {
            AudioSource source;

            if (!effects.ContainsKey(musicName))
            {
                AudioClip clip;
                preloadedAudio.TryGetValue(musicName, out clip);

                GameObject obj = new GameObject(musicName, new System.Type[] { typeof(AudioSource) });
                source = obj.GetComponent<AudioSource>();

                source.clip = clip;

                effects.Add(musicName, source);
            }

            effects.TryGetValue(musicName, out source);

            if (!source.isPlaying)
                source.Play();
        }
    }

    private void PlayPoolSound(string fileName)
    {
        if (!preloadedAudio.ContainsKey(fileName))
        {
            Debug.LogError("NOT Loaded Music File " + fileName);
            return;
        }

        AudioClip clip;
        preloadedAudio.TryGetValue(fileName, out clip);

        GameObject obj = new GameObject(fileName, new System.Type[] { typeof(AudioSource) });
        AudioSource source = obj.GetComponent<AudioSource>();

        source.clip = clip;

        source.Play();
        effectPool.Add(source);
    }

    public void StopAllEffects()
    {
        foreach(var iter in effectPool)
        {
            Destroy(iter.gameObject);
        }

        effectPool.Clear();

        foreach(var iter in effects)
        {
            Destroy(iter.Value.gameObject);
        }

        effects.Clear();
    }

    public void ClearEffects()
    {
        effectPool.Clear();
        effects.Clear();
    }

    public void StopEffects(string musicName)
    {
        var arr = effectPool.ToArray();

        for(int i = 0; i < arr.Length; i += 1)
        {
            if(arr[i].name == musicName)
            {
                effectPool.Remove(arr[i]);
                Destroy(arr[i]);
            }
        }

        if(effects.ContainsKey(musicName))
        {
            AudioSource source;
            effects.TryGetValue(musicName, out source);
            Destroy(source.gameObject);

            effects.Remove(musicName);
        }
    }
}
