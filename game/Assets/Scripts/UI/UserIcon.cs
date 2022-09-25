using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UserIcon : MonoBehaviour
    {
        [SerializeField] private Image winnerIcon;

        public void UpdateIcon(Sprite sprite)
        {
            winnerIcon.sprite = sprite;
        }
    }
}