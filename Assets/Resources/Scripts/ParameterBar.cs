using UnityEngine;
using System.Collections;

public enum ParameterCategory
{
    Math,
    English,
    Art,
    ForeignLanguages,
    Korean,
    Science,
    Social,
    Stress,
    Volunteer
}

public class ParameterBar : BarControl
{
    [SerializeField]
    private ParameterCategory category;
    public ParameterCategory Category
    {
        get
        {
            return category;
        }
    }

    [SerializeField]
    private Transform barEnd;
    public Transform BarEnd
    {
        get
        {
            return barEnd;
        }
    }
}
