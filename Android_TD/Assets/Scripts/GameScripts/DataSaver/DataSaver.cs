using UnityEngine;
using System.Collections;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSaver : MonoBehaviour
{
    public GameObject gameController;

    //Serializable makes it possible to save class into a file
    [Serializable]
    class PlayerData
    {
        public int seed;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Path for saved file
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        Debug.Log("Path for saved file: " + Application.persistentDataPath);

        PlayerData data = new PlayerData();
        data.seed = gameController.GetComponent<MapData>().testingSave;

        //BinaryFormatter writes player data into a file
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            gameController.GetComponent<MapData>().testingSave = data.seed;
        } else
        {
            Debug.Log("Load file does not exist");
        }
    }
}