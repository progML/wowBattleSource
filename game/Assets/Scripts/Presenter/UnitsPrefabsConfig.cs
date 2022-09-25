using UnityEngine;

namespace Presenter
{
    [CreateAssetMenu(fileName = "UnitsPrefabsConfig", menuName = "Presenter/UnitsPrefabsConfig", order = 0)]
    public class UnitsPrefabsConfig : ScriptableObject
    {
        public AttackerUnit[] unitsPrefabs;
    }
}