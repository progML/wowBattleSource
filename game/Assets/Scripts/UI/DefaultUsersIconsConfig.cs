using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "DefaultUsersIconsConfig", menuName = "UI/DefaultUsersIconsConfig", order = 0)]
    public class DefaultUsersIconsConfig : ScriptableObject
    {
        public Sprite DefaultVictoryIcon;
        public Sprite DefaultDefeatIcon;
    }
}