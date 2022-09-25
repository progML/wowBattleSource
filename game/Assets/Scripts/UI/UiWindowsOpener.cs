using System;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class UserUIData
    {
        public Sprite Icon;
        public string Name;

        public UserUIData(Sprite icon, string name)
        {
            Icon = icon;
            Name = name;
        }
    }

    public class UiWindowsOpener : MonoBehaviour
    {
        [SerializeField] private VictoryPanel victoryPanel;
        [SerializeField] private DefeatPanel defeatPanel;
        [SerializeField] private StartGamePanel startGamePanel;
        [SerializeField] private FightUi fightUi;


        public VictoryPanel VictoryPanel => victoryPanel;
        public DefeatPanel DefeatPanel => defeatPanel;
        public StartGamePanel StartGamePanel => startGamePanel;
        public FightUi FightUi => fightUi;


        private WindowPanel _openedWindow;


        public void ShowFightUI(UserUIData attackerData, UserUIData defenderData,
            int attacksMaxCount, int attacksCurCount,
            int defenderHealth, int defenderMaxHealth)
        {
            ShowPanel(fightUi);
            fightUi.SetUp(attackerData, defenderData,
                attacksMaxCount, attacksCurCount,
                defenderHealth, defenderMaxHealth);
        }


        public void ShowVictoryPanel(UserUIData winnerData)
        {
            ShowPanel(victoryPanel);
            victoryPanel.SetUp(winnerData);
        }

        public void ShowDefeatPanel(UserUIData winnerData)
        {
            ShowPanel(defeatPanel);
            defeatPanel.SetUp(winnerData);
        }

        public void ShowStartPanel(UserUIData attackerData, UserUIData defenderData)
        {
            ShowPanel(startGamePanel);
            startGamePanel.SetUp(attackerData, defenderData);
        }

        public void ShowPanel(WindowPanel newPanel)
        {
            if (_openedWindow) _openedWindow.Hide();
            newPanel.Show();
            _openedWindow = newPanel;
        }

        private void Awake()
        {
            victoryPanel.Hide();
            defeatPanel.Hide();
            startGamePanel.Hide();
            fightUi.Hide();
        }
    }
}