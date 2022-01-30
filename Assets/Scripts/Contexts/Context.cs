using Characters.Player;
using Souls;
using System;
using System.Collections;
using Cinemachine;
using Dialogs;
using Effects;
using Environment;
using UnityEngine;

namespace Contexts
{
    public class Context : MonoBehaviour
    {
        public WaypointManager WaypointManager;
        public GameObject Target;
        public SoulManager SoulManager;
        public EffectsManager EffectsManager;

        public PlayerRoot PlayerRoot;
        //public GameObject PlayerGO;
        //public PlayerController PlayerController;
        //public CinemachineVirtualCamera PlayerVCamera;
    }
}