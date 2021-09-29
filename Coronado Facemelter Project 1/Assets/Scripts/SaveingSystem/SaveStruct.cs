using System;
using System.Collections.Generic;
using UnityEngine;
namespace SaveSystem
{
    [Serializable]
    public struct SaveStruct
    {
        public string songName;
        public string imageName;
        public Dictionary<string, float> Line_1;
        public Dictionary<string, float> Line_2;
        public Dictionary<string, float> Line_3;
        public Dictionary<string, float> Line_4;
        public Dictionary<string, float> Line_5;
        public Dictionary<string, float> Line_6;
        public Dictionary<string, float> Line_7;
        public Dictionary<string, float> Line_8;
        public Dictionary<string, float> Line_9;
    }
}
