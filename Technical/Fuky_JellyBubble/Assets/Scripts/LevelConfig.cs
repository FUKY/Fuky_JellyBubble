using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

public class LevelConfig : MonoBehaviour {
    public Dictionary<GemType, int> gemNeccesary;//Nhung cuc can thiet de qua man

    public Dictionary<int, Dictionary<GemType, int>> levelConfig;

    private string file;

    void Start() 
    {

        //levelConfig = {1, {GemType.DAT, 2}};
    }

    void LoadLevel() 
    {
        using (StreamReader r = new StreamReader("file.json"))
        {
            string json = r.ReadToEnd();
            IDictionary response = (IDictionary)Json.Deserialize(json);
        }
    }
}
