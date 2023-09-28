using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TimelineController : MonoBehaviour
{
    public Button skipButton;
    private PlayableDirector playableDirector;

    private const string NEXTSCENE_NAME = "RoomScene";

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        skipButton.onClick.AddListener(SkipTimeline); 
    }

    private void Start()
    {
        playableDirector.stopped += OnTimelineFinished; 
    }

    public  void SkipTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop();
            LoadNextScene();
        }
        else
            Debug.Log("PlayableDirector not found"); 
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        LoadNextScene(); 
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NEXTSCENE_NAME); 
    }
}
