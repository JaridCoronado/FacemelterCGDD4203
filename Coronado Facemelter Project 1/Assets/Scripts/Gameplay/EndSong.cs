using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSong : MonoBehaviour
{
    [SerializeField] private AudioSource song;

    public void EndSongButton()
    {
        song.Stop();
    }
}
