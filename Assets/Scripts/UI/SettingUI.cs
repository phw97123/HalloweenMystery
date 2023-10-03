using Managers;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingUI : UIPopup
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button soundOnButton;
        [SerializeField] private Button soundOffButton;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Button musicOnButton;
        [SerializeField] private Button musicOffButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button quitButton;
        private GameManager _gameManager;
        private UIManager _uiManager;
        private SoundManager _soundManager;
        private bool _isUIStateChanged = false;
        private float _soundVolume = 1;
        private float _musicVolume = 1;
        private bool _isSoundOn = true;
        private bool _isMusicOn = true;

        public event Action<float> OnSoundVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        public event Action OnDisabled;

        private const string SoundVolumeKey = "SoundVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundOnKey = "SoundOn";
        private const string MusicOnKey = "MusicOn";

        private void Awake()
        {
            _soundManager = SoundManager.Instance;

            _uiManager = UIManager.Singleton;
            _gameManager = GameManager.Instance;

        }

        private void Start()
        {
            _soundManager.SubscribeSettingsUI(this);

            closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
            soundOnButton.onClick.AddListener(SoundOff);
            soundOffButton.onClick.AddListener(SoundOn);
            musicOnButton.onClick.AddListener(MusicOff);
            musicOffButton.onClick.AddListener(MusicOn);
            soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);

            _soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey);
            _musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
            _isSoundOn = PlayerPrefs.GetInt(SoundOnKey) == 1;
            _isMusicOn = PlayerPrefs.GetInt(MusicOnKey) == 1;

            _isUIStateChanged = true; 

            quitButton.onClick.AddListener(() => {
                SavePlayerPrefs();
                ShowQuitDialog();
            });
        }

        private void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, _soundVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
            PlayerPrefs.SetInt(SoundOnKey, _isSoundOn ? 1 : 0);
            PlayerPrefs.SetInt(MusicOnKey, _isMusicOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void Update()
        {
            if (!_isUIStateChanged) { return; }

            _isUIStateChanged = false;
            soundOnButton.gameObject.SetActive(_isSoundOn);
            soundOffButton.gameObject.SetActive(!_isSoundOn);
            if (_isSoundOn)
            {
            }
                soundSlider.value = _soundVolume;


            musicOnButton.gameObject.SetActive(_isMusicOn);
            musicOffButton.gameObject.SetActive(!_isMusicOn);
            if (_isMusicOn)
            {
            }
                musicSlider.value = _musicVolume;
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            OnDisabled?.Invoke();
        }

        private void ShowQuitDialog()
        {
            UIPopup dialogPopup = _uiManager.ShowUIPopupByName(nameof(AlertDialogUI));
            AlertDialogUI dialogUI = dialogPopup.GetComponent<AlertDialogUI>();
            dialogUI.CancelButton.gameObject.SetActive(true);
            dialogUI.SubmitButton.gameObject.SetActive(true);
            dialogUI.MessageText.text = "정말로 끝내시겠습니까?";
            dialogUI.SubmitButton.onClick.AddListener(QuitGame);
            dialogUI.CancelButton.onClick.AddListener(() => { dialogUI.gameObject.SetActive(false); });
        }

        private void QuitGame()
        {
          
            _gameManager.QuitGame();
        }

        private void ChangeSoundVolume(float value)
        {
            _isUIStateChanged = true;
            _isSoundOn = value > 0f;
            _soundVolume = value;
            OnSoundVolumeChanged?.Invoke(value);
        }

        private void SoundOn()
        {
            _isSoundOn = true;
            _soundVolume = 1f;
            OnSoundVolumeChanged?.Invoke(_soundVolume);
            _isUIStateChanged = true;
        }

        private void SoundOff()
        {
            _isSoundOn = false;
            soundSlider.value = 0f;
            OnSoundVolumeChanged?.Invoke(0f);
            _isUIStateChanged = true;
        }

        private void ChangeMusicVolume(float value)
        {
            _isUIStateChanged = true;
            _isMusicOn = value > 0f;
            _musicVolume = value;
            OnMusicVolumeChanged?.Invoke(_musicVolume);
        }

        private void MusicOn()
        {
            _isMusicOn = true;
            if (_musicVolume <= 0)
            {
                _musicVolume = 1f;
            }

            OnMusicVolumeChanged?.Invoke(_musicVolume);
            _isUIStateChanged = true;
        }

        private void MusicOff()
        {
            _isMusicOn = false;
            musicSlider.value = 0f;
            OnMusicVolumeChanged?.Invoke(0f);
            _isUIStateChanged = true;
        }
    }
}