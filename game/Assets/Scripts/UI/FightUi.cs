using UnityEngine;

namespace UI
{
    public class FightUi : WindowPanel
    {
        [SerializeField] private UserProfilePanel _attackerProfile;
        [SerializeField] private UserProfilePanel _defenderProfile;
        [SerializeField] private CustomBlockBar _attackerAttacksCounter;
        [SerializeField] private CustomBlockBar _defenderHealthCounter;

        public void SetUp(UserUIData attackerData, UserUIData defenderData,
            int attacksMaxCount, int attacksCurCount,
            int defenderHealth, int defenderMaxHealth)
        {
            _attackerProfile.SetUp(attackerData);
            _defenderProfile.SetUp(defenderData);
            _attackerAttacksCounter.SetUp(attacksMaxCount, attacksCurCount);
            _defenderHealthCounter.SetUp(defenderMaxHealth, defenderHealth);
        }


        public void AttackCountReduce()
        {
            _attackerAttacksCounter.RemoveBlock();
        }


        public void DefenderHealthReduce()
        {
            _defenderHealthCounter.RemoveBlock();
        }
    }
}