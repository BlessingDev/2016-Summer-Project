using UnityEngine;
using System.Collections;

public class TestComp : MonoBehaviour
{
    [SerializeField]
    private BarControl con = null;

	// Use this for initialization
	void Start ()
    {
        if (con != null)
            con.SetValue(50);
	}
}
