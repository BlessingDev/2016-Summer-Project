using UnityEngine;
using System.Collections.Generic;

public enum ScheduleType
{
    TakeARest,
    English,
    BasicMath,
    Korean,
    Science,
    SocialStudy,
    WorldHistory,
    Art,
    Music,
    Economy,
    LawAndPolitics,
    SocietyAndCulture,
    KoreanHistory,
    Ethics,
    Geography,
    Physics,
    LifeScience,
    EarthScience,
    Chemistry,
    Movie,
    MathCompetition,
    Volunteer,
    Parttime,
    ReadingAndGrammar,
    DifferencialAndIntegral,
    ProbabilityAndStatistics,
    Classic,
    EnglishReadingAndWriting,
    PCRoom
}

public abstract class Schedule : MonoBehaviour
{
    protected ScheduleType type;
    public ScheduleType Type
    {
        get
        {
            return type;
        }
    }
    protected List<ParameterCategory> categories = new List<ParameterCategory>();
    public List<ParameterCategory> Categories
    {
        get
        {
            return categories;
        }
    }

    protected bool ratable;             // 실패나 성공이 존재하는가
    public bool IsRatable
    {
        get
        {
            return ratable;
        }
    }

    private bool removable = true;       // 편집 제한된 스케줄인가
    public bool IsRemovable
    {
        get
        {
            return removable;
        }
        set
        {
            Debug.Log("limited is Changed");
            removable = value;
        }
    }

    private bool inited = false;

	// Use this for initialization
	protected void Start()
    {
        if(!inited)
            Init();
        DontDestroyOnLoad(this);
	}

    public virtual void Init()
    {
        inited = true;
    }

    public abstract void Effect(Schedule obj);

    public virtual void Failed()
    {
        Debug.LogWarning("Schedule.Failed Called");
    }
}
