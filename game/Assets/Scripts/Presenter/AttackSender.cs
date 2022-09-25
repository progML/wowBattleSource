using System;
using System.Collections;
using System.Collections.Generic;
using Presenter.Misc;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Presenter
{
    [Serializable]
    public class Attack
    {
        public AttackLine attackLine;
        public AttackerUnit attackerUnit;
        public AttackResult attackResult;

        public Attack(AttackLine attackLine, AttackerUnit attackerUnit, AttackResult attackResult)
        {
            this.attackLine = attackLine;
            this.attackerUnit = attackerUnit;
            this.attackResult = attackResult;
        }
    }

    public class AttackSender : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float AttackFinishEventSendDelay = 1f;

        [Header("Links")]
        [SerializeField] private UnitsPrefabsConfig prefabsConfig;
        [SerializeField] private UnitDeathTypes deathTypes;
        [SerializeField] private AttackViewFinishedChannel attackViewFinishedChannel;

        private Dictionary<AttackLine, Attack> _attackResults;


        public void SendUnit(AttackLine attackLine, bool attackSuccess)
        {
            if (_attackResults.ContainsKey(attackLine))
            {
                throw new Exception("Line already under attack");
            }

            attackLine.ResetView();
            var attackerUnitPrefab = prefabsConfig.unitsPrefabs.GetRandom();
            var attackerUnit = Instantiate(attackerUnitPrefab);

            attackerUnit.UnitAttackFinished.AddListener(OnUnitArrived);
            var newAttack = new Attack(attackLine, attackerUnit, GetAttackResult(attackSuccess));

            if (!_attackResults.ContainsKey(attackLine))
            {
                _attackResults.Add(attackLine, newAttack);
            }

            attackLine.SendUnit(attackerUnit);
        }

        private void Awake()
        {
            _attackResults = new Dictionary<AttackLine, Attack>();
        }


        private AttackResult GetAttackResult(bool attackSuccess)
        {
            UnitDeathType unitDeathType = null;
            if (!attackSuccess) unitDeathType = deathTypes.unitDeathTypes.GetRandom();

            var attackResult = new AttackResult(attackSuccess, unitDeathType);

            return attackResult;
        }


        private void OnUnitArrived(UnitAttackFinishedEventArgs args)
        {
            var unit = args.attackerUnit;
            unit.UnitAttackFinished.RemoveListener(OnUnitArrived);
            AttackLine attackLine = null;
            foreach (var valuePair in _attackResults)
            {
                if (valuePair.Value.attackerUnit == unit)
                {
                    attackLine = valuePair.Key;
                    unit.AttackResult(valuePair.Value.attackResult);
                    valuePair.Value.attackLine.AttackResultViewStart(valuePair.Value.attackResult);

                    break;
                }
            }

            if (attackLine) _attackResults.Remove(attackLine);

            StartCoroutine(DelayedAction(() => { attackViewFinishedChannel.Invoke(); }, AttackFinishEventSendDelay));
        }

        private IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}