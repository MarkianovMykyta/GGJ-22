using System;
using System.Collections;
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
    [SerializeField] private float _artificialFirstLoadingTime = 0f;
    [SerializeField] private bool _cleanStart;
    //[Space]
    //[Header("Menu")]
    //[SerializeField] private MainMenu _mainMenu;

    private PlayerWrapper _playerWrapper;
    private LevelManager _levelManager;
    private LocationMaster _locationMaster;

    public bool IsHaveSave => _playerWrapper.LastCheckpoint.LocationType != LocationType.None;
    public Checkpoint LastCheckpoint => _playerWrapper.LastCheckpoint;
    public Settings Settings => _playerWrapper.Settings;

    public LocationMaster LocationMaster
    {
        set
        {
            _locationMaster = value;
        }
    }

    #region Monobehaviour Methods

    private void Awake()
    {
        CheckInstance();

        _levelManager = new LevelManager();
        _playerWrapper = new PlayerWrapper();

        _playerWrapper.Load();
        if (_cleanStart)
        {
            _playerWrapper.Initialize();
        }

        _globalUI.Initialize();
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

        _globalUI.ShowLoadingScreen(true);
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log($"Register new checpoint {checkpoint}");
        _playerWrapper.RegisterCheckpoint(checkpoint);
    }

    private void AwaitAnyPressKey()
    {
        _globalUI.ShowPressAnyKey(true);
        StartCoroutine(WaitPressKey(StartGame));
    }

    private void StartGame()
    {
        LevelStarted?.Invoke();
        _levelManager.UnloadPreviousScene();

        _globalUI.ShowLoadingScreen(false);
        _globalUI.ShowPressAnyKey(false);

        _locationMaster.Activate();
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
            if (_levelManager.LoadingOperation.isDone && _locationMaster != null)
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

    private Scene CurrenScene;

    public LevelManager()
    {
        CurrenScene = SceneManager.GetActiveScene();
    }

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
        SceneManager.UnloadSceneAsync(CurrenScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.sceneUnloaded += (val) => CurrenScene = SceneManager.GetActiveScene();
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

    public Checkpoint()
    {
        CheckPointIndex = 0;
        LocationType = LocationType.None;
    }

    public Checkpoint(int checkPointIndex, LocationType locationType)
    {
        CheckPointIndex = checkPointIndex;
        LocationType = locationType;
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
