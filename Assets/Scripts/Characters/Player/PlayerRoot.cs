using System.Collections;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

namespace Characters.Player
{

    public class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVCamera;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private SoundManager _soundManager; //gameMaster

        public PlayerController PlayerController => _playerController;

        public bool isInitialized = false;

        public void Initialize()
        {
            if(isInitialized == false)
            {
                isInitialized = true;
                
                _playerController.Initialize();
                _playerController.Freeze();
                _cinemachineVCamera.gameObject.SetActive(false);
            }
        }

        public void Activate()
        {
            _playerController.UnFreeze();
            _cinemachineVCamera.gameObject.SetActive(true);
        }

        public void SetPlayerToLastCheckPoint(CheckpointObject checkpointObject)
        {
            transform.position = checkpointObject.SpawnTransform.position;
            transform.forward = checkpointObject.SpawnTransform.forward;
        }
    }
}