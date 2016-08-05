using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : Manager<SceneManager>
{
    private Dictionary<string, int> scenes;

    public override void Init()
    {
        base.Init();

        scenes = new Dictionary<string, int>();

        scenes.Add("GameScene", 0);
        scenes.Add("ScheduleScene", 1);
        scenes.Add("ShopScene", 2);
        scenes.Add("ConversationScene", 3);
        scenes.Add("NoteScene", 4);
        scenes.Add("ExamScene", 5);
    }
	
    public bool ChangeScene(string sceneName)
    {
        int num;
        if(scenes.TryGetValue(sceneName, out num))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(num);
            return true;
        }
        else
        {
            Debug.LogError("sceneName " + sceneName + " DOESN'T EXIST");
            return false;
        }
    }

    public void ChangeScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public int GetLevel(string name)
    {
        int index;
        if(scenes.TryGetValue(name, out index))
        {
            return index;
        }
        else
        {
            Debug.LogError("Could Not FIND scene name " + name);
            return -1;
        }
    }
}
