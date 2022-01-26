using Contexts;
using UnityEngine;
using UnityEngine.InputSystem;

//[System.Serializable]

public class LocationMaster : MonoBehaviour
{
    [SerializeField] private LocationType _locationType;
    [SerializeField] private Context _context;

    private GameMaster _gameMaster;

    public CheckpointObject[] _checkpoints;

    private void Awake()
    {
        _gameMaster = GameMaster.Instance;
        if (_gameMaster != null)
        {
            _context.PlayerController.Initialize();
            Initialize();
        } 
        else
        {
            Activate();
        }
    }

    private void Initialize()
    {
        _gameMaster.LocationMaster = this;

        for (int i = 0; i < _checkpoints.Length; i++)
        {
            //Ignore Completed Checkpoint
            if (_gameMaster.LastCheckpoint.CheckPointIndex >= i)
                continue;

            var checkpointModel = new Checkpoint(i, _locationType);
            _checkpoints[i].Activate += () => _gameMaster.RegisterCheckpoint(checkpointModel);
        }

        var lastCheckpointObject = _checkpoints[_gameMaster.LastCheckpoint.CheckPointIndex];
        SetPlayerToLastCheckPoint(lastCheckpointObject);
    }

    public void Activate()
    {
        //Unblock Player Input
        InputSystem.EnableDevice(Keyboard.current);
        _context.PlayerController.PlayerInput.Enable();
        _context.PlayerVCamera.gameObject.SetActive(true);
    }

    public void SetPlayerToLastCheckPoint(CheckpointObject checkpointObject)
    {
        _context.PlayerGO.transform.position = checkpointObject.SpawnTransform.position;
        _context.PlayerGO.transform.forward = checkpointObject.SpawnTransform.forward;
    }
}
