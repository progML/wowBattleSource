using UnityEngine;

namespace UI
{
    public class WindowPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        public void Show()
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }
    }
}