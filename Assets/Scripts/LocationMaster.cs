using Contexts;
using System;
using UnityEngine;

[Serializable]
public class LevelStateObjectsPair
{
    public LocationState LocationState;
    public GameObject[] ObjectsToActivate;
    public GameObject[] ObjectsToDeactivate;
}

public class LocationMaster : MonoBehaviour
{
    [SerializeField] private LocationType _locationType;
    [SerializeField] private Context _context;

    private GameMaster _gameMaster;

    public CheckpointObject[] _checkpoints;
    public LevelStateObjectsPair[] _levelStateObjectsPairs;

    public Context Context => _context;
    public LocationType LocationType => _locationType;

    private void Awake()
    {
        DefineGameMaster();
    }

    private void DefineGameMaster()
    {
        _gameMaster = GameMaster.Instance;
        if (_gameMaster != null)
        {
            _gameMaster.RegisterLocationMaster(this);

            if (_context.PlayerRoot == null)
            {
                _context.PlayerRoot = _gameMaster.PlayerRoot;
            }
            else
            {
                _context.PlayerRoot.Initialize();
                return;
            }

            if(_gameMaster.CurrentLocationMaster == this)
                Initialize();
        }
    }

    public void Initialize()
    {
        _context.PlayerRoot.Initialize();
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            //Ignore Completed Checkpoint
            if (_gameMaster.LastCheckpoint.CheckPointIndex >= i)
                continue;

            Checkpoint checkpointModel = new Checkpoint(i, _locationType);
            _checkpoints[i].Activate += () => _gameMaster.RegisterCheckpoint(checkpointModel);

            if(i == _checkpoints.Length - 1)
            {
                _checkpoints[i].Activate += _gameMaster.LocationStateDone;
            }
        }


        CheckpointObject lastCheckpointObject = _checkpoints[_gameMaster.LastCheckpoint.CheckPointIndex];
        _context.PlayerRoot.SetPlayerToLastCheckPoint(lastCheckpointObject);

        LevelStateObjectsPair currentlocationState = GetLocationStateObject(_gameMaster.LastCheckpoint.LocationState);
        SetupEnviromentByState(currentlocationState);
    }

    private void SetupEnviromentByState(LevelStateObjectsPair locationState)
    {
        for (int i = 0; i < locationState.ObjectsToActivate.Length; i++)
        {
            locationState.ObjectsToActivate[i].SetActive(true);
        }
    }

    private LevelStateObjectsPair GetLocationStateObject(LocationState locationState)
    {
        for (int i = 0; i < _levelStateObjectsPairs.Length; i++)
        {
            LevelStateObjectsPair current = _levelStateObjectsPairs[i];
            if (current.LocationState == locationState)
            {
                return current;
            }
        }

        Debug.LogWarning($"Not found {locationState}");
        return new LevelStateObjectsPair(); //null object
    }

    public void Activate()
    {
    }
}
