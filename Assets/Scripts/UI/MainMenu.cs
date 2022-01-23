using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameMaster _gameMaster;
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

        public void Initialize(Settings settings)
        {
            //...
        }

        private void Awake()
        {
            _exit.onClick.AddListener(Application.Quit);
        }
    }
}