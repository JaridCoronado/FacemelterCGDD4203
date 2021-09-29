using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace SaveSystem
{
    public class SaveInitalise: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI SongName = null;
        [SerializeField] private GameObject newImage = null;
        bool _imageUpdatComplete = false;
        string path ="Image/";
        /// <summary>
        /// Android path naming here
        /// </summary>
        string androidPath = Application.persistentDataPath + "/";// android save path
        public void Initalise(SaveStruct Data)
        {
            SongName.text = Data.songName;
            StartCoroutine(LoadImageCoroutine(Data.imageName));
        }
        private IEnumerator LoadImageCoroutine(string imageName)
        {
            string path = (Application.platform == RuntimePlatform.Android) ? androidPath + imageName : Application.persistentDataPath + "/" + imageName;
            WWW image = new WWW(path);
            yield return image;
            if (!_imageUpdatComplete)
            {
                newImage.GetComponent<RawImage>().texture = image.texture;
                _imageUpdatComplete = true;
            }
        }
    }

}
