using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> musicList;
    private AudioSource source;
    private int lastSongIndex;

    void Start()
    {
        source = GetComponent<AudioSource>();
        lastSongIndex = Random.Range(0, musicList.Count);
        source.clip = musicList[lastSongIndex];
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            int index = Random.Range(0, musicList.Count);

            if (index == lastSongIndex)
                return;

            source.clip = musicList[index];
            source.Play();
        }
    }
}