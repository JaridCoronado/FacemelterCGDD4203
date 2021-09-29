using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayEditSong : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelName = null;
    public void PlayGame()
    {
        NodeToScene._songName = _levelName.text;
        SceneManager.LoadScene(3); 
    }
}
