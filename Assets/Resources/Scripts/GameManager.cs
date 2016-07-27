using UnityEngine;
using System.Collections;

public class GameManager : Manager<GameManager>
{

    private bool pause = false;
    public bool IsPause
    {
        get
        {
            return pause;
        }
    }
    private float timeRate = 0;                    // 현실 시간과 게임 내 시간의 비
    private float mTime = 0;                       // 누적 시간

    private int stress;
    public int Stress
    {
        get
        {
            return stress;
        }

        set
        {
            if(value <= 100 && value >= 0)
            {
                stress = value;
            }
            else
            {
                Debug.LogWarning("Stress.value invalid value");
            }
        }
    }


    private GameObject player = null;
    public GameObject Player
    {
        get
        {
            return player;
        }
    }

	// Use this for initialization
	void Start()
    {
        timeRate = 24f / 180f; // 1일은 3분
        stress = 0;
	}

    void Update()
    {
        if (!pause)
        {
            TimeUpdate();
        }
    }

    private void TimeUpdate()
    {
        mTime += Time.smoothDeltaTime * timeRate;
    }
}
