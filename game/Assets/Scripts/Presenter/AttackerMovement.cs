using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AttackerMovement : MonoBehaviour
{
    public UnitMovementFinishedEvent UnitMovementFinished = new UnitMovementFinishedEvent();

    [Header("Settings")]
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float movementDuration;


    public void StartMovement(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(Movement(startPoint, endPoint, movementDuration, movementCurve));
    }

    private IEnumerator Movement(Vector3 startPoint, Vector3 endPoint, float duration, AnimationCurve animationCurve)
    {
        var direction = endPoint - startPoint;
        transform.rotation = Quaternion.LookRotation(direction);

        var timer = 0f;
        var t = 0f;
        while (timer <= duration)
        {
            t = timer / duration;
            transform.position = Vector3.Lerp(startPoint, endPoint, animationCurve.Evaluate(t));
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint;
        UnitMovementFinished.Invoke(new UnitMovementFinishedEventArgs(this));
        yield break;
    }
}


[Serializable]
public class UnitMovementFinishedEvent : UnityEvent<UnitMovementFinishedEventArgs>
{
}

[Serializable]
public class UnitMovementFinishedEventArgs
{
    public AttackerMovement attackerUnit;

    public UnitMovementFinishedEventArgs(AttackerMovement attackerUnit)
    {
        this.attackerUnit = attackerUnit;
    }
}