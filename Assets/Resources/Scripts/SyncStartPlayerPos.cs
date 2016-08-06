using UnityEngine;
using System.Collections;

public class SyncStartPlayerPos : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        transform.localPosition = GameManager.Instance.PlayerPos;
	}
}
