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

    private bool _isSkip = false;
    private bool _isTimelineFinishied = false; 

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

    private void Update()
    {
        if (_isTimelineFinishied && !_isSkip)
        {
            AchiveManager.Instance.UnlockAchieve(Achievement.NoSkipGoing); 
        }
    }

    public void SkipTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop();
            _isSkip = true;
        }
        else
            Debug.Log("PlayableDirector not found");
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        _isTimelineFinishied = true;
        Invoke("LoadNextScene", 0.01f); 
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NEXTSCENE_NAME);
    }
}
