using TMPro;
using UnityEngine;

namespace Meta
{
    public class PopUpMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;


        public void SetUp(string title, string text)
        {
            this.title.text = title;
            this.message.text = text;
        }

        public void CloseClick()
        {
            Destroy(gameObject);
        }
    }
}