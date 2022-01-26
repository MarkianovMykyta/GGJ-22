using Characters.Player;
using Enviroment;
using Souls;
using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Contexts
{
    public class Context : MonoBehaviour
    {
        public WaypointManager WaypointManager;
        public GameObject Target;
        public SoulManager SoulManager;

        public GameObject PlayerGO;
        public PlayerController PlayerController;
        public CinemachineVirtualCamera PlayerVCamera;
    }
}