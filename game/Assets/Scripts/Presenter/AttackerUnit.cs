using System;
using System.Collections;
using Presenter;
using UnityEngine;
using UnityEngine.Events;


public class AttackerUnit : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Animator animator;
    [SerializeField] private AttackerMovement attackerMovement;

    public UnitAttackFinishedEvent UnitAttackFinished = new UnitAttackFinishedEvent();

    private static readonly int WallDestroyedVictory = Animator.StringToHash("WallDestroyedVictory");

    public void StartAttack(Vector3 startPoint, Vector3 endPoint)
    {
        attackerMovement.UnitMovementFinished.AddListener(OnArrived);
        attackerMovement.StartMovement(startPoint, endPoint);
    }

    public void AttackResult(AttackResult attackResult)
    {
        if (attackResult.IsAttackSuccess)
        {
            StartCoroutine(OnWallDestroyed());
        }
        else
        {
            StartCoroutine(OnAttackFailed(attackResult));
        }
    }

    private void OnArrived(UnitMovementFinishedEventArgs arg0)
    {
        attackerMovement.UnitMovementFinished.RemoveListener(OnArrived);
        UnitAttackFinished.Invoke(new UnitAttackFinishedEventArgs(this));
    }


    private IEnumerator OnAttackFailed(AttackResult attackResult)
    {
        var position = gameObject.transform.position;
        var effect = Instantiate(attackResult.UnitDeathType.UnitDeathEffect);
        effect.transform.position = position;
        Destroy(gameObject, 0.1f);
        yield break;
    }

    private IEnumerator OnWallDestroyed()
    {
        animator.SetTrigger(WallDestroyedVictory);
        Destroy(gameObject);
        yield break;
    }
}


[Serializable]
public class UnitAttackFinishedEvent : UnityEvent<UnitAttackFinishedEventArgs>
{
}

[Serializable]
public class UnitAttackFinishedEventArgs
{
    public AttackerUnit attackerUnit;

    public UnitAttackFinishedEventArgs(AttackerUnit attackerUnit)
    {
        this.attackerUnit = attackerUnit;
    }
}