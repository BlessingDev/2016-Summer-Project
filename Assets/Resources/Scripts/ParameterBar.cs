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
    Stress
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
}
