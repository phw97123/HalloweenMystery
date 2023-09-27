using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        Button skipButton = GetComponent<Button>();
        skipButton.onClick.AddListener(SkipTimeline);
    }

    private void SkipTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop();
            //다음 씬 로드
        }
        else
            Debug.Log("PlayableDirector not found");
    }
}
