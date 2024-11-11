using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        // Example
        // [SerializeField] [BoxGroup("Moving")] private float moveSpeed;
        // public float MoveSpeed => moveSpeed;
        public float playerMoveSpeed;
        public float playerEnergy;

        public List<Instrument> Instruments;

        public List<BuildingData> Buildings;

        public List<Vector3> IslandSpawnPositions;

        [Foldout("Boosters")] public float BoosterSpeedTime = 30f;
        [Foldout("Boosters")] public float BoosterPowerTime = 30f;

    }
}