using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace Kuhpik
{
    /// <summary>
    /// Used to store game data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class GameData
    {
        // Example (I use public fields for data, but u free to use properties\methods etc)
        // public float LevelProgress;
        // public Enemy[] Enemies;
        public Transform CameraPoint;
        public Camera Cam;
        public PlayerComponent Player;
        public CharacterController Char;
        public Animator Anim;
        public Inventory Inventory;
        public int BackPackMax;
        public Vector3 Joystick;
        public CameraController CameraController;
        public BoatComponent currentBoat;

        [Header("-------------Boosters-------------")]
        public float BoostSpeedTimer;
        public float BoostPowerTimer;
    }
}