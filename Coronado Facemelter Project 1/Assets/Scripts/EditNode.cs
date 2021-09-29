using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SaveSystem;
using EditMode;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;

public class EditNode : MonoBehaviour
{
    // Start is called before the first frame update
    FileManager FM;
    //Save _saveData;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _fileScene;
    [SerializeField] private GameObject _aiEditScene;
    [SerializeField] private GameObject _editScene;
    [SerializeField] private GameObject _node;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private GameObject _nodeInfo;
    [SerializeField] private GameObject _upLoadImage;
    [SerializeField] private TextMeshProUGUI _nodeTime;
    [SerializeField] private TextMeshProUGUI _songLength;
    [SerializeField] private GameObject _saveNameObject;
    [SerializeField] private TextMeshProUGUI _inputText;
    [SerializeField] private GameObject _next;
    public GameObject _canvas;
    public GameObject _TabBar;
    private GameObject _soundPlayer;
    bool _songObjectAddit = false;
    bool _songPlaying = false;
    bool _imageUpdatComplete = false;
    bool _songObjectCreate = false;

    public static Dictionary<string, float> Line_1 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_2 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_3 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_4 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_5 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_6 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_7 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_8 = new Dictionary<string, float>();
    public static Dictionary<string, float> Line_9 = new Dictionary<string, float>();
    public static Dictionary<int, Dictionary<string, float>> lines = new Dictionary<int, Dictionary<string, float>>();
    GameObject _destoryObject;

    string _songName = "ReZero Season 2 Part 2 - Opening FullLong shotby Mayu Maeshima";// = "Wake";
    string _imageName = "78";// = "demo";

    /// <summary>
    /// Android path naming here
    /// </summary>
    string androidPath;

    void Start()
    {
        FM = this.gameObject.GetComponent<FileManager>();
        _next.SetActive(false);
        //_saveData = gameObject.GetComponent<Save>();
        androidPath = Application.persistentDataPath + "/";// make to my android path
    }

    // Update is called once per frame
    void Update()
    {
        if (!_fileScene.activeSelf && !_aiEditScene.activeSelf)
        {
            if (!_saveNameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var mouse = Input.mousePosition;
                    RaycastHit2D hit = Physics2D.Raycast(mouse, _camera.transform.forward, _targetLayer);
                    if (hit)
                    {
                        if (hit.collider.CompareTag("Tap"))
                        {
                            _nodeInfo.SetActive(true);
                            TimeSpan time = TimeSpan.FromSeconds(TimeOfNode(hit.collider.gameObject.transform.parent.GetComponent<BoxCollider2D>().bounds.max.x, hit.collider.bounds.center.x));
                            _nodeTime.text = time.ToString("m':'ss");
                            _destoryObject = hit.collider.gameObject;

                        }
                        if (hit.collider.CompareTag("Bars"))
                        {
                            var instantiatePoint = new Vector3(Input.mousePosition.x, hit.collider.transform.position.y, Input.mousePosition.z);
                            var instatiateObject = Instantiate(_node, instantiatePoint, Quaternion.identity);
                            instatiateObject.transform.parent = hit.collider.gameObject.transform;
                            instatiateObject.transform.localScale *= 2;
                            instatiateObject.name = instatiateObject.name + hit.collider.transform.childCount;
                            float NodeTime = TimeOfNode(hit.collider.bounds.max.x, Input.mousePosition.x);
                            TimeSpan time = TimeSpan.FromSeconds(NodeTime);
                            Debug.Log("The Instantiate Object time is: " + time.ToString("m':'ss"));
                            SaveNodeToList(hit.collider.name, NodeTime, instatiateObject.name);
                            _nodeInfo.SetActive(false);
                        }
                    }
                    else
                    {
                        _nodeInfo.SetActive(false);
                        Debug.LogError("Hit Nothing");
                    }
                }
            }

        }

    }

    public void LoadSongAndImage()
    {
        if (!_fileScene.activeSelf && !_aiEditScene.activeSelf)
        {
            if (!_songObjectAddit)
            {
                //For testing Uncomme line for 64 -78 and commen line form 79- 107
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
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
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
                        _soundPlayer = songObject;
                        System.TimeSpan time = System.TimeSpan.FromSeconds(songObject.GetComponent<AudioSource>().clip.length);
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
                        _soundPlayer = songObject;
                        System.TimeSpan time = System.TimeSpan.FromSeconds(songObject.GetComponent<AudioSource>().clip.length);
                        _songLength.text = time.ToString("m':'ss");
                        _songObjectAddit = true;
                        _songObjectCreate = true;
                    }
                }
            }
        }

    }


        private float TimeOfNode(float colliderX, float NodeX)
    {
        var barCenter = _TabBar.GetComponent<BoxCollider2D>().bounds.center.x;
        float totalDis = Mathf.Abs(colliderX - barCenter);
        float totalTime = (NodeX - barCenter) * _soundPlayer.GetComponent<AudioSource>().clip.length;
        float NodeTime = totalTime/ totalDis;
        return NodeTime;
    }

    public void NodeDestory()
    {
        switch (_destoryObject.transform.parent.name)
        {
            case "BarLine_1":
                Line_1.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name,Line_1));
                Debug.Log("Remove "+ GetAnyKeky<float>(_destoryObject.name, Line_1) + " from" + "Line 1");
                break;
            case "BarLine_2":
                Line_2.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_2));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_2) + " from" + "Line 2");
                break;
            case "BarLine_3":
                Line_3.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_3));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_3) + " from" + "Line 3");
                break;
            case "BarLine_4":
                Line_4.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_4));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_4) + " from" + "Line 4");
                break;
            case "BarLine_5":
                Line_5.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_5));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_5) + " from" + "Line 5");
                break;
            case "BarLine_6":
                Line_6.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_6));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_6) + " from" + "Line 6");
                break;
            case "BarLine_7":
                Line_7.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_7));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_7) + " from" + "Line 7");
                break;
            case "BarLine_8":
                Line_8.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_8));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_8) + " from" + "Line 8");
                break;
            case "BarLine_9":
                Line_9.Remove(_destoryObject.name);
                Debug.LogFormat("Removeing Object Key: {0},Removeing Object Value: {1}", _destoryObject.name, GetAnyKeky<float>(_destoryObject.name, Line_9));
                Debug.Log("Remove " + GetAnyKeky<float>(_destoryObject.name, Line_9) + " from" + "Line 9");
                break;
        }
        Destroy(_destoryObject);
    }

    public void SaveName()
    {
        if (_inputText.text.Length <=1)
        {
            Debug.LogError("Need to have a text");
            _saveNameObject.SetActive(true);
            return;
        }
        if(_inputText.text.Length >1)
        {
            string songName = GameObject.Find("Music").GetComponent<AudioSource>().clip.name;
            string imageName = _upLoadImage.GetComponent<Image>().sprite.name;
            Debug.Log(_inputText.text);
            Debug.Log("songName: " + songName + " imagName: " + imageName);
            SaveSystem.SaveFuncation.StoreData( songName, imageName);
            //_saveData.StoreData(_inputText.text, songName, imageName);
            _saveNameObject.SetActive(false);
        }
    }
    public void Save()
    {
        string songName = FM.FindSongName();
        string imageName = FM.FindImageName();
        SaveSystem.SaveFuncation.StoreData(songName, imageName);
        _next.SetActive(true);
    }

    public void NextStep()
    {
        _next.SetActive(false);
        _canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        SceneManager.LoadScene(0);
    }

    public void SaveNodeToList(string name, float KeyVaule, string objectName)
    {
        switch (name)
        {
            case "BarLine_1":
                Line_1.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_1: {0}, Adding Value to Line_1: {1}", objectName, KeyVaule);
                break;
            case "BarLine_2":
                Line_2.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_2: {0}, Adding Value to Line_2: {1}", objectName, KeyVaule);
                break;
            case "BarLine_3":
                Line_3.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_3: {0}, Adding Value to Line_3: {1}", objectName, KeyVaule);
                break;
            case "BarLine_4":
                Line_4.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_4: {0}, Adding Value to Line_4: {1}", objectName, KeyVaule);
                break;
            case "BarLine_5":
                Line_5.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_5: {0}, Adding Value to Line_5: {1}", objectName, KeyVaule);
                break;
            case "BarLine_6":
                Line_6.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_6: {0}, Adding Value to Line_6: {1}", objectName, KeyVaule);
                break;
            case "BarLine_7":
                Line_7.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_7: {0}, Adding Value to Line_7: {1}", objectName, KeyVaule);
                break;
            case "BarLine_8":
                Line_8.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_8: {0}, Adding Value to Line_8: {1}", objectName, KeyVaule);
                break;
            case "BarLine_9":
                Line_9.Add(objectName, KeyVaule);
                Debug.LogFormat("Adding Key to Line_9: {0}, Adding Value to Line_9: {1}", objectName, KeyVaule);
                break;
        } 
    }
    private static T GetAnyKeky<T>(string Key ,Dictionary<string,float> name)
    {
        float Object;
        object retType;
        name.TryGetValue(Key, out Object);
        try
        {
            retType = Object;
        }
        catch
        {
            retType = default(float);
        }
        return (T)retType;
    }
    public void StopSong()
    {
        if (_songPlaying)
        {
            _soundPlayer.GetComponent<AudioSource>().Stop();
            _songPlaying = false;
        }else if (!_songPlaying)
        {
            _soundPlayer.GetComponent<AudioSource>().Play();
            _songPlaying = true;
        }
    }

    public void ActiveZoom()
    {
        if (_canvas.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
        {
            _canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }else if (_canvas.GetComponent<Canvas>().renderMode != RenderMode.WorldSpace)
        {
            _canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }

    }

    public void ReturnAI()
    {
        Destroy(_soundPlayer);
        _canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        _editScene.SetActive(false);
        _aiEditScene.SetActive(true);
    }
}
