using UnityEngine;
using System.Collections;
using System.IO;

public class FileManager : Manager<FileManager>
{
    public void WriteFile(string[] strs, string fileName)
    {
        string path = PathForDocumentsFile(fileName);
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(file);

        for(int i = 0; i < strs.Length; i += 1)
        {
            writer.WriteLine(strs[i]);
        }

        writer.Close();
        file.Close();
    }

    public string ReadFile(string fileName)
    {
        string path = PathForDocumentsFile(fileName);

        if(File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            return reader.ReadToEnd();
        }
        else
        {
            Debug.LogError("COULD NOT OPEN File " + fileName);
            return null;
        }
    }

    public string PathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
}
