using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private GameMaster _gameMaster;
        [Space]
        [Header("Main buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _applyNewGame;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        [Header("Settings")]
        [SerializeField] private Slider _sensetiveSlider;
        [SerializeField] private TextMeshProUGUI _sensetiveText;
        [SerializeField] private TMP_Dropdown _resolution;
        [SerializeField] private TMP_Dropdown _quality;
        [SerializeField] private Button _apply;

        [Header("Popups")]
        [SerializeField] private GameObject _notFoundSavePopup;
        [SerializeField] private Button _newGameFromNotFoundPopup;

        private void Awake()
        {
            _gameMaster = GameMaster.Instance;
            if (_gameMaster == null)
            {
                _gameMaster = Instantiate(Resources.Load<GameMaster>("GameMaster"));
            }

            Initialize(_gameMaster.Settings);
        }

        public void Initialize(Settings settings)
        {
            _continue.onClick.AddListener(Continue);
            _applyNewGame.onClick.AddListener(_gameMaster.LoadStartLocation);
            _newGameFromNotFoundPopup.onClick.AddListener(_gameMaster.LoadStartLocation);
            _exit.onClick.AddListener(Application.Quit);

            //_gameMaster.LevelStarted += Close;
        }

        private void Continue()
        {
            if(_gameMaster.IsHaveSave)
            {
                _gameMaster.LoadLastLocation();
            }
            else
            {
                _notFoundSavePopup.SetActive(true);
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}