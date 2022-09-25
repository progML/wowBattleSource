using UnityEngine;

namespace Presenter
{
    [CreateAssetMenu(fileName = "UnitDeathType", menuName = "Presenter/UnitDeathType", order = 0)]
    public class UnitDeathType : ScriptableObject
    {
        public GameObject UnitDeathEffect;
        public GameObject WallDefendEffect;
    }
}