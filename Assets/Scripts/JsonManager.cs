using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    //[SerializeField] private Astar astar;
    [SerializeField] private string streamingAssetsPath;
    [SerializeField] private string persistentPath;

    public JsonData jsonData;

    private void OnEnable()
    {
        jsonData = new JsonData();
        //astar = GetComponent<Astar>();
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath + "//GameData.json");
        persistentPath = Path.Combine(Application.persistentDataPath + "//GameData.json");

        StartCoroutine("LoadJson");
    }

    IEnumerator LoadJson()
    {
        string path = string.Empty;
        
        if (File.Exists(persistentPath))
        {
            path = persistentPath;
        }
        else
        {
            path = streamingAssetsPath;
        }

        string jsonString = File.ReadAllText(path);
        jsonData = JsonUtility.FromJson<JsonData>(jsonString);

        foreach (var npc in jsonData.npcData)
        {
            GameData.Instance.npcNameList.Add(npc.name);
        }

        foreach (var enemy in jsonData.enemyData)
        {
            switch (enemy.dungeonName)
            {
                case "Wood":
                    GameData.Instance.woodDungeonEnemy.Add(enemy.name);
                    break;
                case "Abyss":
                    GameData.Instance.abyssDungeonEnemy.Add(enemy.name);
                    break;
                case "Cellar":
                    GameData.Instance.cellarDungeonEnemy.Add(enemy.name);
                    break;
            }
        
            GameData.Instance.enemyHealthList.Add(enemy.health);
            GameData.Instance.enemyDropMoneyList.Add(enemy.dropMoney);
            GameData.Instance.enemyNameList.Add(enemy.name);
        }
        
        foreach (var dungeon in jsonData.dungeonData)
        {
            GameData.Instance.dungeonActiveNumber.Add(dungeon.dungeonParentNumber);
        }

        //if (GameData.)
        //while (true)
        //{
        //    Debug.Log("async isdone is false");

        //    if (sceneController.asyncIsDone)
        //    {
        //        Debug.Log("asyncIsDone");

        //        while (!astar.endOfSetNode)
        //        {
        //            Debug.Log("wait for astar");

        //            if (astar.endOfSetNode)
        //            {
        //                Debug.Log("end of set astar");

        //                break;
        //            }

        //            yield return new WaitForFixedUpdate();
        //        }

        //        Debug.Log("before in foreach");

        //        foreach (var node in astar.GetNode())
        //        {
        //            if (node.layerNumber == (int)GameLayer.Building)
        //            {
        //                string nodePosToString = $"{node.xPosition}_{node.yPosition}";

        //                if (!GameData.Instance.buildingDictionary.ContainsKey(nodePosToString))
        //                {
        //                    GameData.Instance.buildingDictionary.Add(nodePosToString, node.buildingName);
        //                }
        //            }
        //        }

        //        break;
        //    }

        //    yield return new WaitForFixedUpdate();
        //}

        //GameData.Instance.gameSpeed = jsonData.gameInfo.gameInfoGameSpeed;
        //GameData.Instance.frameRate = jsonData.gameInfo.gameInfoFrameRate;
        //GameData.Instance.money = jsonData.gameInfo.gameInfoMoney;

        yield return null;
    }
    
    public void SaveJson()
    {
        string jsonString = JsonUtility.ToJson(jsonData);
        File.WriteAllText(persistentPath, jsonString);
    }
}
