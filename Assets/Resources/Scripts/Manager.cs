using UnityEngine;
using System.Collections;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    static private T instance = null;
    static public T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            if (instance == null)
                instance = new GameObject(typeof(T).Name, new System.Type[] { typeof(T) }).GetComponent<T>();

            if (!inited)
                instance.Init();

            return instance;
        }
    }
    static protected bool inited = false;
    static public bool IsInited
    {
        get
        {
            return inited;
        }
    }

    protected bool destroyReserved = false;

    virtual public void Init()
    {
        if (FindObjectsOfType<T>().Length > 1)
        {
            if(this != instance)
            {
                Destroy(this);
                destroyReserved = true;
            }
            return;
        }

        inited = true;
        DontDestroyOnLoad(gameObject);
    }

    public virtual void OnLevelWasLoaded(int level)
    {
        if (FindObjectsOfType<T>().Length > 1)
        {
            if (this != instance)
            {
                Destroy(this);
                destroyReserved = true;
            }
            return;
        }

        StopAllCoroutines();
    }

	// Use this for initialization
	void Start ()
    {
        if(!inited)
            Init();
	}
}
