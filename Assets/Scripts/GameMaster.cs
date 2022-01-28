using Characters.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum LocationType
{
    None,
    Town,
    Canalization,
    Forest,
    BanditHome,
    Mountain,
};

[System.Flags]
public enum ItemType
{
    None,
    Bottle,
    Dagger,
};

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Action LevelStarted;

    [Header("Global")]
    [SerializeField] private GlobalUI _globalUI;
    [Space]
    [SerializeField] private GameLocationSequence _gameLocationSequence;
    [Space]
    [SerializeField] private float _artificialFirstLoadingTime = 0f;
    [SerializeField] private bool _cleanStart;

    [Header("Player")]
    [SerializeField] private PlayerRoot _playerRootPrefab;

    private PlayerWrapper _playerWrapper;
    private LevelManager _levelManager;
    private List<LocationMaster> _locationMasters;
    private PlayerRoot _playerRoot;

    public bool IsHaveSave => _playerWrapper.LastCheckpoint.LocationType != LocationType.None;
    public Checkpoint LastCheckpoint => _playerWrapper.LastCheckpoint;
    public Settings Settings => _playerWrapper.Settings;
    public LocationMaster CurrentLocationMaster
    {
        get
        {
           if(_locationMasters.Count > 0)
           {
                return _locationMasters[0];
           }

            return null;//very bad.
        }
    }
    public PlayerRoot PlayerRoot
    {
        get
        {
            //check on level
            if(_playerRoot == null && CurrentLocationMaster != null)
            {
                _playerRoot = CurrentLocationMaster.Context.PlayerRoot;

                if(_playerRoot != null)
                    DontDestroyOnLoad(_playerRoot);
            }

            if (_playerRoot == null)
            {
                _playerRoot = Instantiate(_playerRootPrefab);
                DontDestroyOnLoad(_playerRoot);
            }

            return _playerRoot;
        }
    }

    #region Monobehaviour Methods

    private void Awake()
    {
        CheckInstance();

        _levelManager = new LevelManager();
        _playerWrapper = new PlayerWrapper();
        _locationMasters = new List<LocationMaster>();

        _playerWrapper.Load();
        if (_cleanStart)
        {
            _playerWrapper.Initialize();
        }

        _globalUI.Initialize();
    }

    private void Start()
    {
        if (!_levelManager.IsMenu)
        {
            StartGame();
            LoadNextLocation();
        }
    }

    #endregion Monobehaviour Methods

    public void LoadStartLocation()
    {
        _playerWrapper.Initialize();
        LoadLastLocation();
    }

    public void LoadLastLocation()
    {
        StartCoroutine(_levelManager.LoadLevelAsync(_playerWrapper.LastCheckpoint.LocationType));
        StartCoroutine(ArtificialLoadIncrease(AwaitAnyPressKey));
        LoadNextLocation();

        _globalUI.ShowLoadingScreen(true);
    }

    private void LoadNextLocation()
    {
        LocationType nextLocation = _gameLocationSequence.GetNextLocation(_playerWrapper.LastCheckpoint);
        if (nextLocation != LocationType.None)
        {
            if(_levelManager.LoadingOperation != null && !_levelManager.LoadingOperation.isDone)
                _levelManager.LoadingOperation.completed += (val) => StartCoroutine(_levelManager.LoadLevelAsync(nextLocation));
            else
                StartCoroutine(_levelManager.LoadLevelAsync(nextLocation));
        }
    }

    public void LocationStateDone()
    {
        var removeLocation = _locationMasters[0];
        _locationMasters.RemoveAt(0);

        LocationType nextLocation = _gameLocationSequence.GetNextLocation(_playerWrapper.LastCheckpoint);

        CurrentLocationMaster.Initialize();

        if(nextLocation == removeLocation.LocationType)
        {
            _locationMasters.Add(removeLocation);
            return;
        }

        if (nextLocation == CurrentLocationMaster.LocationType)
        {
            return;
        }

        LoadNextLocation();
        _levelManager.LoadingOperation.completed += (val) => CurrentLocationMaster.Activate();
    }

    public void RegisterLocationMaster(LocationMaster locationMaster) => _locationMasters.Add(locationMaster);

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log($"Register new checpoint {checkpoint}");
        _playerWrapper.RegisterCheckpoint(checkpoint);
    }

    private void AwaitAnyPressKey()
    {
        _globalUI.ShowPressAnyKey(true);
        StartCoroutine(WaitPressKey(UnloadMenuAndStart));
    }

    private void UnloadMenuAndStart()
    {
        LevelStarted?.Invoke();
        _levelManager.UnloadPreviousScene();
        _globalUI.ShowLoadingScreen(false);
        _globalUI.ShowPressAnyKey(false);

        StartGame();
    }

    private void StartGame()
    {
        ///
        _playerRoot.Activate();
        CurrentLocationMaster.Activate();
    }

    private IEnumerator WaitPressKey(Action callback)
    {
        while (true)
        {
            if (Keyboard.current.anyKey.wasReleasedThisFrame)
            {
                callback();
                break;
            }
            yield return null;
        }
    }

    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator ArtificialLoadIncrease(Action callback)
    {
        yield return new WaitForSeconds(_artificialFirstLoadingTime);

        while (true)
        {
            if (_levelManager.LoadingOperation.isDone && CurrentLocationMaster != null)
            {
                callback();
                break;
            }
            yield return null;
        }
    }
}

public class LevelManager
{
    public AsyncOperation LoadingOperation;

    private Scene CurrentScene;

    public LevelManager()
    {
        CurrentScene = SceneManager.GetActiveScene();
    }

    public bool IsMenu => SceneManager.GetActiveScene().buildIndex == 0;

    public IEnumerator LoadLevelAsync(LocationType locationType)
    {
        LoadingOperation = SceneManager.LoadSceneAsync(locationType.ToString(), LoadSceneMode.Additive);

        while (!LoadingOperation.isDone)
        {
            yield return null;
        }
    }

    public void UnloadPreviousScene()
    {
        SceneManager.UnloadSceneAsync(CurrentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.sceneUnloaded += (val) => CurrentScene = SceneManager.GetActiveScene();
    }
}


public class PlayerWrapper
{
    private const string key = "key-1";

    public Settings Settings => _playerModel.PlayerSettings;
    public Checkpoint LastCheckpoint => _playerModel.LastCheckPoint;

    private PlayerModel _playerModel;

    public void Initialize()
    {
        _playerModel.LastCheckPoint = new Checkpoint(0, LocationType.Town);
        _playerModel.Items = ItemType.None;
        Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(key))
        {
            string model = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(model))
            {
                _playerModel = JsonUtility.FromJson<PlayerModel>(model);
            }
            else
            {
                throw new System.Exception("Load failed, file broken");
            }
        } 
        else
        {
            _playerModel = new PlayerModel();
            Save();
        }
    }
    public void Save()
    {
        string model = JsonUtility.ToJson(_playerModel);
        if (!string.IsNullOrEmpty(model))
        {
            PlayerPrefs.SetString(key, model);
        }
        else
        {
            throw new System.Exception("Save failed");
        }
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        _playerModel.LastCheckPoint = checkpoint;
        Save();
    }
}


[System.Serializable]
public class PlayerModel
{
    public ItemType Items;

    public Checkpoint LastCheckPoint;
    public Settings PlayerSettings;

    public PlayerModel()
    {
        PlayerSettings = new Settings();
        LastCheckPoint = new Checkpoint();
    }
}

[System.Serializable]
public class Checkpoint
{
    public int CheckPointIndex;
    public LocationType LocationType;
    public LocationState LocationState;

    public Checkpoint()
    {
        CheckPointIndex = 0;
        LocationType = LocationType.None;
        LocationState = LocationState.Stage_1;
    }

    public Checkpoint(int checkPointIndex, LocationType locationType, LocationState stage = LocationState.Stage_1)
    {
        CheckPointIndex = checkPointIndex;
        LocationType = locationType;
        LocationState = stage;
    }

    public override string ToString()
    {
        return $"Index: {CheckPointIndex}, Location {LocationType}";
    }
}

[System.Serializable]
public class Settings
{
    public bool IsFullScreen;
    public float Sensetive;
}
