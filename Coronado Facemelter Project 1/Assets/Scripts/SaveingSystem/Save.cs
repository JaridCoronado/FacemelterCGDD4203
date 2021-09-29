using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveSystem
{
    public class Save : MonoBehaviour
    {
        public static Save _save;
        //[SerializeField] private int maxSong = 6;
        [SerializeField] private Transform holder = null; // the caves holding the objecct
        [SerializeField] private GameObject SongBoject = null; // 
        private static string SavePath => $"{Application.persistentDataPath}/save.json";
        private void Start()
        {
            SaveList saveList = GetSave();
            UpdateSave(saveList);
            SaveData(saveList);
        }

        public static SaveList GetSave()
        {
            if (!File.Exists(SavePath))
            {
                File.Create(SavePath).Dispose();
                return new SaveList();
            }
            else
            {

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SavePath, FileMode.Open);
               var data = formatter.Deserialize(stream) as SaveList;
                stream.Close();
                return data;
                //using (StreamReader stream = new StreamReader(SavePath))
                //{
                //    return (SaveList)new JsonSerializer().Deserialize(stream, typeof(SaveList));
                //}
            }
        }

        public static void SaveData(SaveList SaveData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Create);
            formatter.Serialize(stream, SaveData);
            stream.Close();
            //using (StreamWriter stream = new StreamWriter(SavePath))
            //{
            //    new JsonSerializer().Serialize(stream, SaveData);
            //}
        }
        private void UpdateSave(SaveList savescore)
        {
            foreach (Transform child in holder)
            {
                Destroy(child.gameObject);
            }
            foreach (SaveStruct s in savescore.SongSaveList)
            {
                Instantiate(SongBoject, holder).GetComponent<SaveInitalise>().Initalise(s);
            }
        }

        //public void StoreData(string name, string songName, string imageName)
        //{
        //    SaveList saveList = GetSave();
        //    SaveStruct temp = new SaveStruct();
        //    temp.songName = songName;
        //    temp.imageName = imageName;
        //    temp.Line_1 = EditNode.Line_1;
        //    temp.Line_2 = EditNode.Line_2;
        //    temp.Line_3 = EditNode.Line_3;
        //    temp.Line_4 = EditNode.Line_4;
        //    temp.Line_5 = EditNode.Line_5;
        //    temp.Line_6 = EditNode.Line_6;
        //    temp.Line_7 = EditNode.Line_7;
        //    temp.Line_8 = EditNode.Line_8;
        //    temp.Line_9 = EditNode.Line_9;

        //    saveList.SongSaveList.Add(temp);
        //    SaveData(saveList);
        //}
    }

}

