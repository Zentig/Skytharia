using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource AudioSource1, AudioSource2;
    public AudioClip CurrentSong, NextSong;
    [HideInInspector] public AudioSource currentAudioSource, oldAudioSource;
    [SerializeField] float crossfadeTime;
    bool isCrossfading;


    void Awake()
    {
        FindSongs("Assets/Sounds/Songs");
        currentAudioSource = AudioSource1;
        CurrentSong = audioClips[0];
        
    }

    void Update()
    {
        if (!isCrossfading && GetRemainingTime(currentAudioSource) <= crossfadeTime)
        {
            StartCoroutine(CrossfadeSequence());
        }
    }

    private int GetCurrentSongIndex()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            if (currentAudioSource.clip == audioClips[i])
            {
                return i;
            }
        }
        return -1; // Return -1 if not found
    }

    void GetNextSong()
    {
        CurrentSong = currentAudioSource.clip;
        int currentIndex = GetCurrentSongIndex();

        // Check if the current index is the last one in the list
        if (currentIndex == audioClips.Count - 1)
        {
            // If it is, wrap around to the first song
            NextSong = audioClips[0];
        }
        else
        {
            // Otherwise, just get the next song in the list
            NextSong = audioClips[currentIndex + 1];
        }
    }

    void Swap()
    {
        if (oldAudioSource == AudioSource1)
        {
            currentAudioSource = AudioSource2;
        }
        else
        {
            currentAudioSource = AudioSource1;
        }
    }

    private float GetRemainingTime(AudioSource source)
    {
        if (source != null && source.clip != null)
        {
            return source.clip.length - source.time;
        }
        return 0f;
    }

    void FindSongs(string folderPath)
    {
        audioClips.Clear();

        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.mp3"); // Searches for mp3 files, change the extension if needed

            foreach (string file in files)
            {
                //loads each song from the path
                audioClips.Add((AudioClip)AssetDatabase.LoadAssetAtPath(file, typeof(AudioClip)));
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + folderPath);
        }
    }
    IEnumerator CrossfadeSequence()
    {
        isCrossfading = true;

        GetNextSong();
        StartCoroutine(Fade(currentAudioSource, crossfadeTime, 0));

        // Swap and start fading in the next song
        oldAudioSource = currentAudioSource;
        Swap();
        currentAudioSource.clip = NextSong;
        currentAudioSource.Play();
        StartCoroutine(Fade(currentAudioSource, crossfadeTime, 1));

        // Wait for the crossfade to complete
        yield return new WaitForSeconds(crossfadeTime);

        isCrossfading = false;
    }

    //its public in case you need to call it from somewhere else
    public IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }


    //use these if you wanna change stuff externally
    public void AddSong(AudioClip song)
    {
        audioClips.Add(song);
    }
    public void RemoveSong(AudioClip song)
    {
        audioClips.Remove(song);
    }
    public List<AudioClip> GetSongs()
    {
        return audioClips;
    }
}
