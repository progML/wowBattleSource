using System;
using UnityEngine;

namespace Presenter.Misc
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;

        private void Update()
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
    }
}