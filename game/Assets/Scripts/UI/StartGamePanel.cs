using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class StartGamePanel : WindowPanel
    {
        [SerializeField] private UserProfilePanel _attackerProfile;
        [SerializeField] private UserProfilePanel _defenderProfile;

        public UnityEvent StartButtonClicked = new UnityEvent();

        public void SetUp(UserUIData attackerData, UserUIData defenderData)
        {
            _attackerProfile.SetUp(attackerData);
            _defenderProfile.SetUp(defenderData);
        }

        public void StartButtonClick()
        {
            StartButtonClicked.Invoke();
        }
    }
}