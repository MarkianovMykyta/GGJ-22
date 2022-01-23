using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LocationType
{
    None,
    Town,
    Canlization,
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
    
    [SerializeField] private MainMenu _mainMenu;

    private PlayerWrapper _playerWrapper;
    private LevelManager _levelManager;

    private void Awake()
    {
        CheckInstance();

        _playerWrapper = new PlayerWrapper();
        _playerWrapper.Load();

        _mainMenu.Initialize(_playerWrapper.Settings);
        _levelManager.LoadScene(_playerWrapper.LastCheckpoint.LocationType);
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

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        _playerWrapper.RegisterCheckpoint(checkpoint);
    }
}

public class LevelManager
{
    public void LoadScene(LocationType locationType)
    {
        SceneManager.LoadScene(locationType.ToString(), LoadSceneMode.Additive); //...
    }
}


public class PlayerWrapper
{
    private const string key = "key-1";

    public Settings Settings => _playerModel.PlayerSettings;
    public Checkpoint LastCheckpoint => _playerModel.LastCheckPoint;

    private PlayerModel _playerModel;

    public void Load()
    {
        if (PlayerPrefs.HasKey(key))
        {
            string model = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(model))
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
        if (string.IsNullOrEmpty(model))
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
}

[System.Serializable]
public class Settings
{
    public bool IsFullScreen;
    public float Sensetive;
}
