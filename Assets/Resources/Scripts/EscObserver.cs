using UnityEngine;

public abstract class EscObserver : MonoBehaviour
{
	
    void update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscAction();
        }
    }

    public abstract void EscAction();
}
