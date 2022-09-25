using System;
using Presenter;
using Presenter.Misc;
using ServerRest;
using UnityEngine;
using UnityEngine.Events;

namespace Meta.BaseRedactor
{
    public class WallsSpawner : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private GameFieldPrefabsConfig gameFieldPrefabsConfig;
        [SerializeField] private WallsContainer wallsContainer;


        [Header("Settings")]
        [SerializeField] private float lineSpaceBetween;

        public UnityEvent WallsSpawned = new UnityEvent();


        [ContextMenu("Spawn10Walls")]
        public void Spawn10Walls()
        {
            SpawnWalls(10);
        }

        public void SpawnWalls(int wallsCount)
        {
            wallsContainer.DestroyWalls();

            Vector3 spawnPosition = transform.position;
            for (int i = 0; i < wallsCount; i++)
            {
                var newWall = SpawnWall();
                newWall.transform.SetParent(transform);
                newWall.transform.position = spawnPosition;
                spawnPosition += transform.right * lineSpaceBetween;
                wallsContainer.AddWall(i, newWall);
            }

            WallsSpawned.Invoke();
        }

        private void Start()
        {
            SpawnWalls(ServerGameStatus.Instance.staticGameParams.baseLen);
        }

        private WallView SpawnWall()
        {
            var randomWallPrefab = gameFieldPrefabsConfig.wallViewPrefabs.GetRandom();
            var newWall = Instantiate(randomWallPrefab);
            return newWall;
        }
    }
}