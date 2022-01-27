using Characters.Player;
using Enviroment;
using Souls;
using System;
using System.Collections;
using Cinemachine;
using Dialogs;
using Effects;
using UnityEngine;

namespace Contexts
{
    public class Context : MonoBehaviour
    {
        public WaypointManager WaypointManager;
        public GameObject Target;
        public SoulManager SoulManager;
        public EffectsManager EffectsManager;
        public DialogManager DialogManager;

        public GameObject PlayerGO;
        public PlayerController PlayerController;
        public CinemachineVirtualCamera PlayerVCamera;
    }
}