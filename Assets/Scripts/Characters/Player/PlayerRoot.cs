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

        public void Initialize()
        {
            _playerController.Initialize();
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