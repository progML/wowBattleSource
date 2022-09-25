using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.BaseRedactor
{
    public class WallsContainer : MonoBehaviour
    {
        [Header("Monitoring")]
        [SerializeField] private Dictionary<int, WallView> walls;

        public WallView this[int index] => walls[index];
        public int this[WallView line]
        {
            get
            {
                foreach (var keyValuePair in walls)
                    if (keyValuePair.Value == line)
                        return keyValuePair.Key;

                throw new Exception("No such walls");
            }
        }

        public int WallsCount => walls.Count;

        public void DestroyWalls()
        {
            if (walls != null)
                foreach (var vp in walls)
                    Destroy(vp.Value.gameObject);

            walls = new Dictionary<int, WallView>();
        }

        public void AddWall(int i, WallView wallView)
        {
            walls.Add(i, wallView);
        }
    }
}