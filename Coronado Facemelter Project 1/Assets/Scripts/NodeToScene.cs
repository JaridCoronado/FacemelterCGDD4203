using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using System.IO;

public class NodeToScene : MonoBehaviour
{
    public static string _songName = "Wake";
    Save save;
    SaveList saveList;

    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject line_1;
    [SerializeField] private GameObject line_2;
    [SerializeField] private GameObject line_3;
    [SerializeField] private GameObject line_4;
    [SerializeField] private GameObject line_5;
    [SerializeField] private GameObject line_6;
    [SerializeField] private GameObject line_7;
    [SerializeField] private GameObject line_8;
    [SerializeField] private GameObject line_9;
    [SerializeField] private GameObject _node;

    private List<float> line1 = new List<float>();
    private List<float> line2 = new List<float>();
    private List<float> line3 = new List<float>();
    private List<float> line4 = new List<float>();
    private List<float> line5 = new List<float>();
    private List<float> line6 = new List<float>();
    private List<float> line7 = new List<float>();
    private List<float> line8 = new List<float>();
    private List<float> line9 = new List<float>();

    private float[] _audioSpectrum;
    [SerializeField] private GameObject _songObject;

    string _songPath = "Song/";
    string _imagePath = "Image/";
    // Start is called before the first frame update

    bool _songObjectCreate = false;
    bool _imageUpdatComplete = false;

    public Text audioTimeText;

    private AudioSource audioSource;
    private int currentHour;
    private int currentMinute;
    private int currentSecond;
    private int clipHour;
    private int clipMinute;
    private int clipSecond;

    int _count1 = 0;
    int _count2 = 0;
    int _count3 = 0;
    int _count4 = 0;
    int _count5 = 0;
    int _count6 = 0;
    int _count7 = 0;
    int _count8 = 0;
    int _count9 = 0;
    /// <summary>
    /// Android path naming here
    /// </summary>
    string androidPath;
    private void Awake()
    {
        save = new Save();
        saveList = new SaveList();
        saveList = Save.GetSave();
        _audioSpectrum = new float[128];
        Debug.Log(_songName);
        androidPath = Application.persistentDataPath + "/";// make to android path
    }
    void Start()
    {
        foreach (SaveStruct s in saveList.SongSaveList)
        {
            if (s.songName == _songName)
            {
                Debug.Log("The song is " + s.songName + " The imge is " + s.imageName);
                StartCoroutine(LoadSongCoroutine(s.songName));
                StartCoroutine(LoadImageCoroutine(s.imageName));

                line1 = new List<float>(s.Line_1.Count);
                line2 = new List<float>(s.Line_2.Count);
                line3 = new List<float>(s.Line_3.Count);
                line4 = new List<float>(s.Line_4.Count);
                line5 = new List<float>(s.Line_5.Count);
                line6 = new List<float>(s.Line_6.Count);
                line7 = new List<float>(s.Line_7.Count);
                line8 = new List<float>(s.Line_8.Count);
                line9 = new List<float>(s.Line_9.Count);

                foreach (var data in s.Line_1) line1.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_2) line2.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_3) line3.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_4) line4.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_5) line5.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_6) line6.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_7) line7.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_8) line8.Add((int)Mathf.Abs(data.Value) - 3);
                foreach (var data in s.Line_9) line9.Add((int)Mathf.Abs(data.Value) - 3);

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
                _background.GetComponent<RawImage>().texture = text;
                _imageUpdatComplete = true;
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            WWW image = new WWW(path);//FM.FindSongName());
            yield return image;
            if (!_imageUpdatComplete)
            {
                _background.GetComponent<RawImage>().texture = image.texture;
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
                        audioSource = songObject.GetComponent<AudioSource>();
                        clipHour = (int)audioSource.clip.length / 3600;
                        clipMinute = (int)(audioSource.clip.length - clipHour * 3600) / 60;
                        clipSecond = (int)(audioSource.clip.length - clipHour * 3600 - clipMinute * 60);
                        System.TimeSpan time = System.TimeSpan.FromSeconds(songObject.GetComponent<AudioSource>().clip.length);
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
                        audioSource = songObject.GetComponent<AudioSource>();
                        clipHour = (int)audioSource.clip.length / 3600;
                        clipMinute = (int)(audioSource.clip.length - clipHour * 3600) / 60;
                        clipSecond = (int)(audioSource.clip.length - clipHour * 3600 - clipMinute * 60);
                        System.TimeSpan time = System.TimeSpan.FromSeconds(songObject.GetComponent<AudioSource>().clip.length);
                        _songObjectCreate = true;
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        ShowAudioTime();
        SpectrumValue = (int)audioSource.time;
        Debug.Log(SpectrumValue);
        if (audioSource.GetComponent<AudioSource>().clip.length > 1 && audioSource != null)
        {

            // line 1
            if (_count1 < line1.Count && Mathf.Approximately(SpectrumValue, line1[_count1]))
            {
                if (Mathf.Approximately(SpectrumValue, line1[_count1]))//(SpectrumValue -line1[i]) < 1|| (SpectrumValue - line1[i]) >1)
                {
                    Vector3 location = new Vector3(line_1.GetComponent<BoxCollider2D>().bounds.center.x, line_1.GetComponent<BoxCollider2D>().bounds.max.y, line_1.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_1.transform;
                    _count1++;
                }
            }
            //line 2
            if (_count2 < line2.Count && Mathf.Approximately(SpectrumValue, line2[_count2]))
            {
                if (Mathf.Approximately(SpectrumValue, line2[_count2]))//(SpectrumValue -line1[i]) < 1|| (SpectrumValue - line1[i]) >1)
                {
                    Vector3 location = new Vector3(line_2.GetComponent<BoxCollider2D>().bounds.center.x, line_2.GetComponent<BoxCollider2D>().bounds.max.y, line_2.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_2.transform;
                    _count2++;
                }
            }
            //line 3
            if (_count3 < line3.Count && Mathf.Approximately(SpectrumValue, line3[_count3]))
            {
                if (Mathf.Approximately(SpectrumValue, line3[_count3]))//(SpectrumValue -line1[i]) < 1|| (SpectrumValue - line1[i]) >1)
                {
                    Vector3 location = new Vector3(line_3.GetComponent<BoxCollider2D>().bounds.center.x, line_3.GetComponent<BoxCollider2D>().bounds.max.y, line_3.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_3.transform;
                    _count3++;
                }
            }
            //line 4
            if (_count4 < line4.Count && Mathf.Approximately(SpectrumValue, line4[_count4]))
            {
                if (Mathf.Approximately(SpectrumValue, line4[_count4]))//(SpectrumValue -line4[i]) < 4|| (SpectrumValue - line4[i]) >4)
                {
                    Vector3 location = new Vector3(line_4.GetComponent<BoxCollider2D>().bounds.center.x, line_4.GetComponent<BoxCollider2D>().bounds.max.y, line_4.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_4.transform;
                    _count4++;
                }
            }
            //line 5
            if (_count5 < line5.Count && Mathf.Approximately(SpectrumValue, line5[_count5]))
            {
                if (Mathf.Approximately(SpectrumValue, line5[_count5]))//(SpectrumValue -line5[i]) < 5|| (SpectrumValue - line5[i]) >5)
                {
                    Vector3 location = new Vector3(line_5.GetComponent<BoxCollider2D>().bounds.center.x, line_5.GetComponent<BoxCollider2D>().bounds.max.y, line_5.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_5.transform;
                    _count5++;
                }
            }
            //line 6
            if (_count6 < line6.Count && Mathf.Approximately(SpectrumValue, line6[_count6]))
            {
                if (Mathf.Approximately(SpectrumValue, line6[_count6]))//(SpectrumValue -line6[i]) < 6|| (SpectrumValue - line6[i]) >6)
                {
                    Vector3 location = new Vector3(line_6.GetComponent<BoxCollider2D>().bounds.center.x, line_6.GetComponent<BoxCollider2D>().bounds.max.y, line_6.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_6.transform;
                    _count6++;
                }
            }
            // line 7
            if (_count7 < line7.Count && Mathf.Approximately(SpectrumValue, line7[_count7]))
            {
                if (Mathf.Approximately(SpectrumValue, line7[_count7]))//(SpectrumValue -line7[i]) < 7|| (SpectrumValue - line7[i]) >7)
                {
                    Vector3 location = new Vector3(line_7.GetComponent<BoxCollider2D>().bounds.center.x, line_7.GetComponent<BoxCollider2D>().bounds.max.y, line_7.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_7.transform;
                    _count7++;
                }
            }
            // line 8
            if (_count8 < line8.Count && Mathf.Approximately(SpectrumValue, line8[_count8]))
            {
                if (Mathf.Approximately(SpectrumValue, line8[_count8]))//(SpectrumValue -line8[i]) < 8|| (SpectrumValue - line8[i]) >8)
                {
                    Vector3 location = new Vector3(line_8.GetComponent<BoxCollider2D>().bounds.center.x, line_8.GetComponent<BoxCollider2D>().bounds.max.y, line_8.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_8.transform;
                    _count8++;
                }
            }
            // line 9
            if (_count9 < line9.Count && Mathf.Approximately(SpectrumValue, line9[_count9]))
            {
                if (Mathf.Approximately(SpectrumValue, line9[_count9]))//(SpectrumValue -line9[i]) < 9|| (SpectrumValue - line9[i]) >9)
                {
                    Vector3 location = new Vector3(line_9.GetComponent<BoxCollider2D>().bounds.center.x, line_9.GetComponent<BoxCollider2D>().bounds.max.y, line_9.transform.position.z);
                    var instatiateObject = Instantiate(_node, location, Quaternion.identity);
                    instatiateObject.transform.parent = line_9.transform;
                    _count9++;
                }
            }
        }
    }
    private void ShowAudioTime()
    {

        currentHour = (int)audioSource.time / 3600;
        currentMinute = (int)(audioSource.time - currentHour * 3600) / 60;
        currentSecond = (int)(audioSource.time - currentHour * 3600 - currentMinute * 60);
        audioTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2} / {3:D2}:{4:D2}:{5:D2}",
            currentHour, currentMinute, currentSecond, clipHour, clipMinute, clipSecond);
        //audioTimeSlider.value = audioSource.time / audioClip.length;

    }
    public static float SpectrumValue { get; private set; }


}
