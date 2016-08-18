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

        scenes.Add("LogoScene", 0);
        scenes.Add("MainScene", 1);
        scenes.Add("GameScene", 2);
        scenes.Add("ScheduleScene", 3);
        scenes.Add("ShopScene", 4);
        scenes.Add("ConversationScene", 5);
        scenes.Add("NoteScene", 6);
        scenes.Add("ExamScene", 7);
        scenes.Add("AlbumScene", 8);
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
