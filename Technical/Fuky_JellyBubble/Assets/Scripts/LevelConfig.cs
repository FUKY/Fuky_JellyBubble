using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

public class LevelConfig : MonoSingleton<LevelConfig> {
    //public Dictionary<GemType, int> gemNeccesary;//Nhung cuc can thiet de qua man

    public Dictionary<int, Dictionary<GemType, int>> levelConfigs;// = new Dictionary<int,Dictionary<GemType,int>>();

    private string file;

    void Start() 
    {
        levelConfigs = new Dictionary<int, Dictionary<GemType, int>>();
        LoadLevel();
        //levelConfig = {1, {GemType.DAT, 2}};
    }

    void LoadLevel() 
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets/Resources/Level/level.json"))
        {
            json = r.ReadToEnd();
            Debug.Log(System.String.Format("Level = {0}", json));
            IDictionary response = (IDictionary)Json.Deserialize(json);
            LoadConfigFromLevel(response);
        }
    }

    void LoadConfigFromLevel(IDictionary data) 
    {
        //IDictionary level = (IDictionary)data["level"];
        List<object> levels = (List<object>)data["level"];
        int numLevel = 0;
        foreach (object level in levels)
        {
            numLevel++;

            IDictionary dataLevel = (IDictionary)level;
            object water = dataLevel["water"];
            int countWater = int.Parse(water.ToString());

            object worm = dataLevel["worm"];
            int countWorm = int.Parse(worm.ToString());

            object sun = dataLevel["sun"];
            int countSun = int.Parse(sun.ToString());

            //Config 1 level
            Dictionary<GemType, int> levelConfig = new Dictionary<GemType, int>();
            levelConfig.Add(GemType.WATER, countWater);
            levelConfig.Add(GemType.SUN, countWater);
            levelConfig.Add(GemType.WORM, countWorm);

            //add vao list Config
            levelConfigs.Add(numLevel, levelConfig);
            //Debug.Log("water = " + countWater);
        }

        //int countWater1 = level1Config[GemType.WATER];
    }

    public Dictionary<GemType, int> GetLevelConfigByLevel(int level) 
    {
        Dictionary<GemType, int> levelConfig = levelConfigs[level];
        return levelConfig;
    }
}
