using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class VictoryPanel : WindowPanel
    {
        [SerializeField] private UserIcon _userIcon;
        [SerializeField] private DefaultUsersIconsConfig usersIconsConfig;
        public UnityEvent OkButtonClicked = new UnityEvent();

        public void SetUp(UserUIData winnerData)
        {
            if (winnerData.Icon) _userIcon.UpdateIcon(winnerData.Icon);
            else _userIcon.UpdateIcon(usersIconsConfig.DefaultDefeatIcon);
        }

        public void OkButtonClick()
        {
            OkButtonClicked.Invoke();
        }
    }
}