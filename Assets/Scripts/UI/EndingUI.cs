using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static ResourceManager;
using TMPro;

public class EndingUI : UIPopup
{
    private GameManager _gameManager;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_FontAsset gameClearFont;
    [SerializeField] private TMP_FontAsset gameOverFont;

    [SerializeField] private Image endingImage;
    [SerializeField] private Sprite[] endingImages;

    public enum ButtonType { RedoButton, HomeButton, QuitButton }
    public Button[] ButtonArr;
    public bool[] ButtonCheck;
    public enum Ending { GameOver, GameClear }
    private Ending _ending;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        ButtonArr[(int)ButtonType.RedoButton].onClick.AddListener(() => { OnClickRedoButton(); });
        ButtonArr[(int)ButtonType.HomeButton].onClick.AddListener(() => { OnClickHomeButton(); });
        ButtonArr[(int)ButtonType.QuitButton].onClick.AddListener(() => { OnClickQuitButton(); });
        ButtonCheck = new bool[ButtonArr.Length];
    }


    void Start()
    {
        for(int i = 0; i < ButtonArr.Length; i++)
        {
            SetActiveButton(ButtonArr[i], false);
        }

        //_ending값 게임매니저한테서 받아오기
        switch (_ending)
        {
            case Ending.GameOver:
                GameOver();
                break;
            case Ending.GameClear:
                GameClear();
                break;
        }
    }

    public void GameOver() 
    {
        gameOverText.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("GameOver"));

        gameOverText.font = gameOverFont;
        gameOverText.text = "GAME OVER";
        endingImage.sprite = endingImages[(int)Ending.GameOver];

        ButtonCheck[(int)ButtonType.RedoButton] = true;
        ButtonCheck[(int)ButtonType.QuitButton] = true;

        Invoke("DisplayButton", 2.0f);
    }
    public void GameClear()
    {
        gameOverText.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("GameClear"));

        gameOverText.font = gameClearFont;
        gameOverText.text = "당신은 친구들을 구해냈습니다";
        endingImage.sprite = endingImages[(int)Ending.GameClear];

        ButtonCheck[(int)ButtonType.HomeButton] = true;
        ButtonCheck[(int)ButtonType.QuitButton] = true;

        Invoke("DisplayButton", 4.0f);
    }

    public void DisplayButton()
    {
        for (int i = 0; i < ButtonArr.Length; i++)
        {
            if (ButtonCheck[i] == true) SetActiveButton(ButtonArr[i], true);
        }
    }
    public void OnClickRedoButton()
    {
        GameManager.Instance.ChangeScene(Scenes.RoomScene);
    }

    public void OnClickHomeButton()
    {
        //GameManager.Instance.ChangeScene(Scenes.StartScene);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void SetActiveButton(Button button, bool active)
    {
        button.gameObject.SetActive(active);
    }
}
