using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.IO;
using System;
using SaveSystem;
using UnityEditor;
using SimpleFileBrowser;

namespace EditMode
{
    public class FileManager : MonoBehaviour
    {
        string _imageSavePath; // save path for image
        string _songSavePath; // save path for song
        [HideInInspector] public string _imageName; // define the name of image that player upload
        [HideInInspector] public string _songName; // define the name of song that player upload
        Texture2D _png; // use to store image in folder
        byte[] _mp3; // Use to store song in folder
        [SerializeField] private RawImage _image;
        [SerializeField] private GameObject _fileScene;
        [SerializeField] private GameObject _editScene;
        [SerializeField] private GameObject _aiEditScen;
        [SerializeField] private GameObject _chooseMenu;
        [SerializeField] private Text songConfrimation;
        string[] _typeOfImage = { "png", "jpg" };
        string m_DeviceType;

        private WWW www = null;
        static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Android path naming here
        /// </summary>
        string androidPath;
        private void Awake()
        {
            _editScene.SetActive(false);
            _fileScene.SetActive(true);
            _aiEditScen.SetActive(false);
            _chooseMenu.SetActive(false);
            songConfrimation.text = "";
            androidPath = Application.persistentDataPath + "/";// make to android
        }
        public void OpenImageExplorer()
        {
            Debug.Log(SystemInfo.deviceType);
             if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
             {
                FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
                StartCoroutine(ShowLoadDialogCoroutine());
            }
            else if(Application.platform == RuntimePlatform.Android)
             {
                 try
                 {
                    FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
                    print(_imageName + " has been saved!");
                    StartCoroutine(ShowLoadDialogCoroutine());
                }
                 catch (Exception e)
                 {
                     Debug.Log(e);
                 }
             }
           
        }
        public void OpenSongExplorer()
        {
               if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
               {
                FileBrowser.SetFilters(true, new FileBrowser.Filter("Song", ".mp3", ".wav"));
                StartCoroutine(ShowLoadDialogCoroutine());
                }
               else if (Application.platform == RuntimePlatform.Android)
               {
                   try
                   {
                    FileBrowser.SetFilters(true, new FileBrowser.Filter("Song", ".mp3", ".wav"));
                    StartCoroutine(ShowLoadDialogCoroutine());
                }
                   catch (Exception e)
                   {
                       Debug.Log(e);
                   }
               }
        }
        void GetImage()
        {
            if (_imageSavePath != null)
            {

                UpdateImage();
            }
        }
        void UpdateImage()
        {
            WWW www =(Application.platform == RuntimePlatform.Android)? new WWW("jar:file://" + _imageSavePath) : new WWW("file:///" + _imageSavePath);
            _image.texture = www.texture;
            _png = www.texture;
        }
        void GetSong()
        {
            if (_songSavePath != null)
            {
                UpdateSong();
            }
        }
        void UpdateSong()
        {
            WWW www = (Application.platform == RuntimePlatform.Android) ? new WWW("jar:file://" + _imageSavePath) : new WWW("file:///" + _songSavePath);
            _mp3 = www.bytes;
            songConfrimation.text = "The " + _songName + " is save";
        }
        public void Save()
        {
            if (_imageSavePath == null || _songSavePath == null)
            {
                Debug.LogError("Need to have a song and a picture to save");
                return;
            }
            if (_imageSavePath != null && _songSavePath != null)
            {
                _chooseMenu.SetActive(true);
                string path = (Application.platform == RuntimePlatform.Android) ? androidPath + _imageName : Application.persistentDataPath + "/" + _imageName;
                string songPath = (Application.platform == RuntimePlatform.Android) ? androidPath + _songName : Application.persistentDataPath + "/" + _songName;
                byte[] bytes = _png.EncodeToPNG(); ;
                FileInfo imageInfo = new FileInfo(path);
                FileInfo songInfo = new FileInfo(songPath);
                if (bytes != null && !imageInfo.Exists)
                {
                    File.WriteAllBytes(path, bytes);
                }
                if (!songInfo.Exists)
                {
                    File.WriteAllBytes(songPath, _mp3);
                }

            }
            
        }

        public void OpenUserEditMode()
        {
            _chooseMenu.SetActive(false);
            _fileScene.gameObject.SetActive(false);
            _editScene.gameObject.SetActive(true);
        }

        public void OpenAIEditMode()
        {
            _chooseMenu.SetActive(false);
            _fileScene.gameObject.SetActive(false);
            _aiEditScen.SetActive(true);
        }

        
        public string FindSongName()
        {
            return _songName;
        }
        public string FindImageName()
        {
            return _imageName;
        }
        IEnumerator ShowLoadDialogCoroutine()
        {
            // Show a load file dialog and wait for a response from user
            // Load file/folder: both, Allow multiple selection: true
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Load File", Submit button text: "Load"
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
            if (FileBrowser.Success)
            {
                // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
                for (int i = 0; i < FileBrowser.Result.Length; i++)
                _imageSavePath = _songSavePath = FileBrowser.Result[0];

                // Read the bytes of the first file via FileBrowserHelpers
                // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                // Or, copy the first file to persistentDataPath
                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);

                if (_imageSavePath.ToLower().EndsWith(".png") && _imageSavePath.Length > 1 || _imageSavePath.ToLower().EndsWith(".jpg") && _imageSavePath.Length > 1)
                {
                    string[] world = (Application.platform == RuntimePlatform.Android) ? _imageSavePath.Split('/'): _imageSavePath.Split('\\');
                    _imageName = world[world.Length-1];
                    GetImage();
                }
                if (_songSavePath.ToLower().EndsWith(".mp3") && _songSavePath.Length > 1)
                {
                    string[] world = (Application.platform == RuntimePlatform.Android) ? _imageSavePath.Split('/') : _imageSavePath.Split('\\');
                    _songName = world[world.Length - 1];
                    GetSong();
                }
            }
        }
    }
}
