using TMPro;
using UnityEngine;

namespace UI
{
    public class UserProfilePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private UserIcon _userIcon;
        [SerializeField] private DefaultUsersIconsConfig usersIconsConfig;

        public void SetUp(UserUIData userData)
        {
            if (userData.Icon) _userIcon.UpdateIcon(userData.Icon);
            else _userIcon.UpdateIcon(usersIconsConfig.DefaultVictoryIcon);
            _name.text = userData.Name;
        }
    }
}