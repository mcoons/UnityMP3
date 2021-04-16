using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using SimpleFileBrowser;

using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour

{
    //public AudioClip[] musicClips;
    public List<AudioClip> musicClips = new List<AudioClip>();

    private AudioSource source;
    private int currentTrack;
    private int fullLength;
    private int playTime;
    private int seconds;
    private int minutes;

    private bool isPaused = false;

    public TextMeshProUGUI clipTitleText;
    public TextMeshProUGUI clipTimeText;

    public Slider scrubberSlider;
    public Slider volumeSlider;

    public Camera camera;
    private int lastPixelHeight;
    private int currentPixelHeight;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        volumeSlider.value = source.volume;
        currentTrack = 0;

        //PlayMusic();

        lastPixelHeight = camera.pixelHeight;
        currentPixelHeight = lastPixelHeight;


        //// Set filters (optional)
        //// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        //// if all the dialogs will be using the same filters
        //FileBrowser.SetFilters(true, new FileBrowser.Filter("All Files", ".*"));

        //// Set default filter that is selected when the dialog is shown (optional)
        //// Returns true if the default filter is set successfully
        //// In this case, set Images filter as the default filter
        //FileBrowser.SetDefaultFilter(".mp3");

        //// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        //// Note that when you use this function, .lnk and .tmp extensions will no longer be
        //// excluded unless you explicitly add them as parameters to the function
        //FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        //// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        //// It is sufficient to add a quick link just once
        //// Name: Users
        //// Path: C:\Users
        //// Icon: default (folder icon)
        //FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        //// Show a save file dialog 
        //// onSuccess event: not registered (which means this dialog is pretty useless)
        //// onCancel event: not registered
        //// Save file/folder: file, Allow multiple selection: false
        //// Initial path: "C:\", Initial filename: "Screenshot.png"
        //// Title: "Save As", Submit button text: "Save"
        //// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

        //// Show a select folder dialog 
        //// onSuccess event: print the selected folder's path
        //// onCancel event: print "Canceled"
        //// Load file/folder: folder, Allow multiple selection: false
        //// Initial path: default (Documents), Initial filename: empty
        //// Title: "Select Folder", Submit button text: "Select"
        //FileBrowser.ShowLoadDialog((paths) => { Debug.Log("Selected: " + paths.Length); },
        //                          () => { Debug.Log("Canceled"); },
        //                          FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select");



    }

    private void Update()
    {
        currentPixelHeight = camera.pixelHeight;

        if (currentPixelHeight != lastPixelHeight)
        {
            Debug.Log(currentPixelHeight);
            lastPixelHeight = currentPixelHeight;
        }

    }

    void UpdatePlaylist(string[] paths)
    {
        foreach (var item in paths)
        {
            Debug.Log(item);
        }
    }

    public void GetFiles()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("All Files", ".*"));
        FileBrowser.SetDefaultFilter(".mp3");

        FileBrowser.ShowLoadDialog((paths) => { UpdatePlaylist(paths); },
                          () => { Debug.Log("Canceled"); },
                          FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select");
    }


    public void PlayMusic()
    {
        if (source.isPlaying)
        {
            return;
        }
        if (isPaused)
        {
            isPaused = false;
            source.UnPause();
            return;
        }

        //currentTrack--;

        //if (currentTrack < 0)
        //{
        //    currentTrack = musicClips.Count - 1;
        //}
        source.clip = musicClips[currentTrack];
        UpdateCurrentTitle();
        source.Play();

        StartCoroutine("WaitForMusicEnd");
    }


    IEnumerator WaitForMusicEnd()
    {
        while (source.isPlaying || isPaused)
        {
            UpdatePlayTime();
            yield return null;
        }

        NextTitle();
    }

    public void NextTitle()
    {
        scrubberSlider.value = 0;

        if (isPaused)
        {
            isPaused = false;
            source.UnPause();
        }

        source.Stop();
        currentTrack++;
        if (currentTrack > musicClips.Count -1)
        {
            currentTrack = 0;
        }
        source.clip = musicClips[currentTrack];
        source.Play();

        UpdateCurrentTitle();

        StartCoroutine("WaitForMusicEnd");
    }

    public void PreviousTitle()
    {
        scrubberSlider.value = 0;

        if (isPaused)
        {
            isPaused = false;
            source.UnPause();
        }

        source.Stop();
        currentTrack--;
        if (currentTrack < 0)
        {
            currentTrack = musicClips.Count - 1;
        }
        source.clip = musicClips[currentTrack];
        source.Play();

        UpdateCurrentTitle();

        StartCoroutine("WaitForMusicEnd");
    }

    public void StopMusic()
    {
        StopCoroutine("WaitForMusicEnd");
        scrubberSlider.value = 0;

        if (isPaused)
        {
            isPaused = false;
            source.UnPause();
        }
        source.Stop();
        UpdatePlayTime();
    }

    public void PauseMusic()
    {
        if (source.isPlaying)
        {
            isPaused = true;
            source.Pause();
        }
        else
        {
            isPaused = false;
            source.UnPause();
        }
    }

    public void OnVolumeChange(System.Single value)
    {
        source.volume = value;
    }

    public void MuteMusic()
    {
        source.mute = !source.mute;
    }

    public void OnScrubberChange(System.Single value)
    {
        source.time = scrubberSlider.value;
    }

    void UpdateCurrentTitle()
    {
        clipTitleText.text = source.clip.name;
        fullLength = (int)source.clip.length;
    }

    void UpdatePlayTime()
    {
        playTime = (int)source.time;
        seconds = playTime % 60;
        minutes = (playTime / 60) % 60;
        clipTimeText.text = minutes + ":" + seconds.ToString("D2") + "/" + ((fullLength / 60) % 60) + ":" + (fullLength % 60).ToString("D2");

        UpdateScrubber();
    }

    void UpdateScrubber()
    {
        scrubberSlider.minValue = 0;
        scrubberSlider.maxValue = source.clip.length;
        scrubberSlider.value = source.time;
    }


    public void OnScaleChange(System.Single value)
    {
        source.volume = value;
    }
}
