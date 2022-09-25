using System;
using Presenter;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class LineSelector : MonoBehaviour
    {
        [SerializeField] private float Offset;
        [SerializeField] private GameObject selectArrow;

        [SerializeField] private AttackLinesContainer linesContainer;

        public LineSelectedEvent lineSelected = new LineSelectedEvent();

        private Camera _camera;


        private bool _showSelectArrow;
        public bool ShowSelectArrow
        {
            get { return _showSelectArrow; }
            set
            {
                if (_showSelectArrow && !value) selectArrow.SetActive(false);
                else if (!_showSelectArrow && value) selectArrow.SetActive(true);

                _showSelectArrow = value;
            }
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (ShowSelectArrow)
            {
                if (RayCastFindAttackLine(out var line))
                {
                    selectArrow.transform.position = line.WallView.transform.position + Vector3.up * Offset;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (RayCastFindAttackLine(out var line))
                {
                    lineSelected.Invoke(new LineSelectedEventArgs(line, linesContainer[line]));
                }
            }
        }

        private bool RayCastFindAttackLine(out AttackLine attackLine)
        {
            attackLine = null;
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
                return (hit.transform.gameObject.TryGetComponent(out attackLine));

            return false;
        }
    }

    [Serializable]
    public class LineSelectedEvent : UnityEvent<LineSelectedEventArgs>
    {
    }

    [Serializable]
    public class LineSelectedEventArgs
    {
        public AttackLine AttackLine;
        public int LineNum;

        public LineSelectedEventArgs(AttackLine attackLine, int lineNum)
        {
            AttackLine = attackLine;
            LineNum = lineNum;
        }
    }
}