using Presenter;
using Presenter.Misc;
using UnityEngine;

namespace Gameplay
{
    public class GameStatePresenter : MonoBehaviour
    {
        [SerializeField] private AttackSender attackSender;
        [SerializeField] private GameFieldSpawner gameFieldSpawner;

        public void AttackLine(AttackLine attackLine, bool isAttackSuccess)
        {
            attackSender.SendUnit(attackLine, isAttackSuccess);
        }

        public void SpawnField(int lineCount)
        {
            gameFieldSpawner.SpawnField(lineCount);
        }
    }
}