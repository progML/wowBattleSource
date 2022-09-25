using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "DefaultUsersProfileConfig", menuName = "UI/DefaultUsersProfileConfig", order = 0)]
    public class DefaultUsersProfileConfig : ScriptableObject
    {
        public Sprite Icon;
        public string Name;
    }
}