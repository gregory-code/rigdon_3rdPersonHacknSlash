using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    List<AudioSource> songList = new List<AudioSource>();
    private AudioSource currentSong;

    void Start()
    {
        GameObject[] songs = GameObject.FindGameObjectsWithTag("song");
        foreach(GameObject song in songs)
        {
            songList.Add(song.GetComponent<AudioSource>());
        }

        GetSong();
    }


    void Update()
    {
        if(currentSong.isPlaying == false)
        {
            GetSong();
        }
    }

    private void GetSong()
    {
        currentSong = songList[Random.Range(0, songList.Count)];
        currentSong.Play();
    }
}
