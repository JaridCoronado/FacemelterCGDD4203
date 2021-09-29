using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;
using UnityEngine.UI;
using TMPro;
using EditMode;
using System.IO;
using UnityEngine.Networking;

public class AIEdit : MonoBehaviour
{
    FileManager FM;
    Save _saveData;

    [SerializeField] private GameObject _fileScene;
    [SerializeField] private GameObject _aiEditScene;
    [SerializeField] private GameObject _editScene;
    [SerializeField] private GameObject _upLoadImage;
    [SerializeField] private GameObject _next;
    [SerializeField] private GameObject _saveName;
    [SerializeField] private TextMeshProUGUI _inputText;
    [SerializeField]
    private AudioSource audioSource;
    bool _songObjectAddit = false;
    bool _imageUpdatComplete = false;

    [SerializeField] private Text _songLength;
    bool _songObjectCreate = false;
    GameObject _songPlayer;

    [SerializeField] private int _timeStep; // determing what spectrum value going to trigger a beat time

    public static Dictionary<string, float> Line_1 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_2 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_3 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_4 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_5 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_6 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_7 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_8 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_9 = new Dictionary<string, float>();

    /// <summary>
    /// Android path naming here
    /// </summary>
    string androidPath;
    // Start is called before the first frame update
    void Start()
    {
        FM = this.gameObject.GetComponent<FileManager>();
        _saveData = gameObject.GetComponent<Save>();
        _saveName.SetActive(false);
        _next.SetActive(false);
        androidPath = Application.persistentDataPath + "/";//+ songName;
    }

    // Update is called once per frame
    void Update()
    {
        //AI_LoadSongAndImage();
    }

    public void AI_LoadSongAndImage()
    {
        if (!_fileScene.activeSelf && !_editScene.activeSelf)
        {

            if (!_songObjectAddit)
            {
                //for testing uncommone testing and comm line form 59-86
                //Testing();
                if (FM.FindSongName().ToLower().EndsWith(".mp3"))
                {
                    StartCoroutine(LoadSongCoroutine(FM.FindSongName()));
                }
            }
            if (!_imageUpdatComplete)
            {
                if (FM.FindImageName().ToLower().EndsWith(".png") || FM.FindImageName().ToLower().EndsWith(".jpg"))
                {
                    StartCoroutine(LoadImageCoroutine(FM.FindImageName()));
                }
            }

        }
    }
    private IEnumerator LoadImageCoroutine(string imageName)
    {
        string path = (Application.platform == RuntimePlatform.Android) ? androidPath + imageName : Application.persistentDataPath + "/" + imageName;

        if (Application.platform == RuntimePlatform.Android)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D text = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            yield return text;
            if (!_imageUpdatComplete)
            {
                text.filterMode = FilterMode.Trilinear;
                text.LoadImage(bytes);
                _upLoadImage.GetComponent<RawImage>().texture = text;
                _imageUpdatComplete = true;
            }
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer|| Application.platform == RuntimePlatform.WindowsEditor)
        {
            WWW image = new WWW(path);//FM.FindSongName());
            yield return image;
            if (!_imageUpdatComplete)
            {
                _upLoadImage.GetComponent<RawImage>().texture = image.texture;
                _imageUpdatComplete = true;
            }
        }
    }
    private IEnumerator LoadSongCoroutine(string songName)
    {
        string path = (Application.platform == RuntimePlatform.Android) ? androidPath + songName : Application.persistentDataPath + "/" + songName;

        if (Application.platform == RuntimePlatform.Android)
        {
            using (UnityWebRequest song = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                song.url = song.url.Replace("http://localhost", "file://");
                yield return song.SendWebRequest();

                if (song.isHttpError || song.isNetworkError)
                {
                    Debug.LogError(song.error);
                }
                else
                {
                    if (song.isDone && !_songObjectCreate)
                    {
                        GameObject songObject = new GameObject("Music");
                        songObject.AddComponent<AudioSource>();
                        songObject.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(song);
                        songObject.GetComponent<AudioSource>().Play();
                        songObject.AddComponent<AudioSpectrum>();
                        _songPlayer = songObject;
                        System.TimeSpan time = System.TimeSpan.FromSeconds(_songPlayer.GetComponent<AudioSource>().clip.length);
                        _songLength.text = time.ToString("m':'ss");
                        _songObjectAddit = true;
                        _songObjectCreate = true;
                    }
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            using (UnityWebRequest song = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return song.SendWebRequest();

                if (song.isHttpError || song.isNetworkError)
                {
                    Debug.LogError(song.error);
                }
                else
                {
                    Debug.Log(song.url);
                    if (song.isDone && !_songObjectCreate)
                    {
                        GameObject songObject = new GameObject("Music");
                        songObject.AddComponent<AudioSource>();
                        songObject.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(song);
                        songObject.GetComponent<AudioSource>().Play();
                        songObject.AddComponent<AudioSpectrum>();
                        _songPlayer = songObject;
                        System.TimeSpan time = System.TimeSpan.FromSeconds(_songPlayer.GetComponent<AudioSource>().clip.length);
                        _songLength.text = time.ToString("m':'ss");
                        _songObjectAddit = true;
                        _songObjectCreate = true;
                    }
                }
            }
        }

    }
    private void Testing()
    {
        StartCoroutine(LoadSongCoroutine("Wake.mp3"));
        StartCoroutine(LoadImageCoroutine("git bash code.PNG"));
    }

    public void Generate()
    {
        _next.SetActive(true);
        SongByteToFloat();
        string songName = FM.FindSongName();
        string imageName = FM.FindImageName();
        SaveSystem.SaveFuncation.AIStoreData(songName, imageName);
    }
    public void Save()
    {
        if (_inputText.text.Length <= 1)
        {
            Debug.LogError("Need to have a text");
            _saveName.SetActive(true);
            return;
        }
        if (_inputText.text.Length > 1)
        {
            string songName = GameObject.Find("Music").GetComponent<AudioSource>().clip.name;
            string imageName = _upLoadImage.GetComponent<Image>().sprite.name;
            Debug.Log(_inputText.text);
            Debug.Log("songName: " + songName + " imagName: " + imageName);
            SaveSystem.SaveFuncation.AIStoreData(songName, imageName);
            //_saveData.StoreData(_inputText.text, songName, imageName);
            _saveName.SetActive(false);
        }
    }
    /// <summary>
    /// cover the data into float then use this for AI pitch is uncomplete
    /// </summary>
    /// <param name="path"></param>
    private void SongByteToFloat()
    {
        GameObject music = GameObject.Find("Music");
        if (music != null)
        {
            var clipLength = music.GetComponent<AudioSource>().clip.length;
            var temp = _timeStep;
            int amountNodeAppear;
            int difference = _timeStep;
            for (int i = 0; i < clipLength; i += difference)
            {
                if (_timeStep > clipLength)
                {
                    break;
                }

                if (i == _timeStep)
                {
                    amountNodeAppear = Random.Range(1, 3);
                    Debug.Log("amountNodeAppear is " + amountNodeAppear);
                    int whichLine = Random.Range(1, 10);
                    int tempNumber = 0;
                    if (amountNodeAppear > 1)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            var randNumber = Random.Range(1, 10);
                            while (tempNumber == randNumber)
                            {
                                randNumber = Random.Range(1, 10);
                            }
                            tempNumber = randNumber;
                            Debug.Log("The node is add to Line " + randNumber + "The Node key is " + i); /*+ "The time is " + _songLength);*/
                            AddNode(randNumber, i);
                        }
                    }
                    if (amountNodeAppear <= 1)
                    {
                        Debug.Log("The node is add to Line " + whichLine + "The Node key is " + i); /*+ "The time is " + _songLength);*/
                        AddNode(whichLine, i);
                    }
                    difference = Random.Range(1, 6);
                    _timeStep += difference;
                    Debug.Log("Random number from " + difference);
                }
            }
        }
    }

    private void AddNode(int NodeLocation, int i)
    {
        switch (NodeLocation)
        {
            case 1:
                Line_1.Add(i.ToString(), i);
                break;
            case 2:
                Line_2.Add(i.ToString(), i);
                break;
            case 3:
                Line_3.Add(i.ToString(), i);
                break;
            case 4:
                Line_4.Add(i.ToString(), i);
                break;
            case 5:
                Line_5.Add(i.ToString(), i);
                break;
            case 6:
                Line_6.Add(i.ToString(), i);
                break;
            case 7:
                Line_7.Add(i.ToString(), i);
                break;
            case 8:
                Line_8.Add(i.ToString(), i);
                break;
            case 9:
                Line_9.Add(i.ToString(), i);
                break;
        }
    }

    public void BackToEdit()
    {
        Destroy(_songPlayer);
        _aiEditScene.SetActive(false);
        _editScene.SetActive(true);
    }
}

