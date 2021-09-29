using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class SaveFuncation : MonoBehaviour
    {
        public static void StoreData(string songName, string imageName)
        {
            SaveList saveList = SaveSystem.Save.GetSave();
            SaveStruct temp = new SaveStruct();
            temp.songName = songName;
            temp.imageName = imageName;
            temp.Line_1 = EditNode.Line_1;
            temp.Line_2 = EditNode.Line_2;
            temp.Line_3 = EditNode.Line_3;
            temp.Line_4 = EditNode.Line_4;
            temp.Line_5 = EditNode.Line_5;
            temp.Line_6 = EditNode.Line_6;
            temp.Line_7 = EditNode.Line_7;
            temp.Line_8 = EditNode.Line_8;
            temp.Line_9 = EditNode.Line_9;
            saveList.SongSaveList.Add(temp);
            SaveSystem.Save.SaveData(saveList);
        }

        public static void AIStoreData(string songName, string imageName)
        {
            SaveList saveList = SaveSystem.Save.GetSave();
            SaveStruct temp = new SaveStruct();
            temp.songName = songName;
            temp.imageName = imageName;
            temp.Line_1 = AIEdit.Line_1;
            temp.Line_2 = AIEdit.Line_2;
            temp.Line_3 = AIEdit.Line_3;
            temp.Line_4 = AIEdit.Line_4;
            temp.Line_5 = AIEdit.Line_5;
            temp.Line_6 = AIEdit.Line_6;
            temp.Line_7 = AIEdit.Line_7;
            temp.Line_8 = AIEdit.Line_8;
            temp.Line_9 = AIEdit.Line_9;
            saveList.SongSaveList.Add(temp);
            SaveSystem.Save.SaveData(saveList);
        }
    }
}

