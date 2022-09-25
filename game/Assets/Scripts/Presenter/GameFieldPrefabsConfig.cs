using UnityEngine;

namespace Presenter
{
    [CreateAssetMenu(fileName = "GameFieldPrefabsConfig", menuName = "Presenter/GameFieldPrefabsConfig", order = 0)]
    public class GameFieldPrefabsConfig : ScriptableObject
    {
        public WallView[] wallViewPrefabs;
        public AttackerCornerView[] AttackerCornerViewPrefabs;
        public Material[] GroundMaterials;
    }
}