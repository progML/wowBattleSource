using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Presenter
{
    public class AttackLinesContainer : MonoBehaviour
    {
        [SerializeField] private Transform linesParent;


        private Dictionary<int, AttackLine> lines;

        public AttackLine this[int index] => lines[index];
        public int this[AttackLine line]
        {
            get
            {
                foreach (var keyValuePair in lines)
                    if (keyValuePair.Value == line)
                        return keyValuePair.Key;

                throw new Exception("No such line");
            }
        }

        public Transform LinesParent => linesParent;

        public void AddLine(AttackLine line, int lineNum)
        {
            lines.Add(lineNum, line);
        }

        public void DestroyLines()
        {
            if (lines == null) return;
            foreach (var attackLine in lines) Destroy(attackLine.Value.gameObject);
            lines = new Dictionary<int, AttackLine>();
        }

        private void Awake()
        {
            lines ??= new Dictionary<int, AttackLine>();
        }
    }
}