using UnityEngine;

namespace Presenter
{
    [CreateAssetMenu(fileName = "UnitDeathTypes", menuName = "Presenter/UnitDeathTypes", order = 0)]
    public class UnitDeathTypes : ScriptableObject
    {
        public UnitDeathType[] unitDeathTypes;
    }
}