using System;
using UnityEngine;
using UnityEngine.Events;

namespace Presenter
{
    [CreateAssetMenu(fileName = "AttackViewFinishedChannel", menuName = "Presenter/AttackViewFinishedChannel",
        order = 0)]
    public class AttackViewFinishedChannel : ScriptableObject
    {
        public AttackViewFinishedEvent attackViewFinished = new AttackViewFinishedEvent();

        public void Invoke()
        {
            attackViewFinished.Invoke(new AttackViewFinishedEventArgs());
        }
    }

    [Serializable]
    public class AttackViewFinishedEvent : UnityEvent<AttackViewFinishedEventArgs>
    {
    }

    [Serializable]
    public class AttackViewFinishedEventArgs
    {
    }
}