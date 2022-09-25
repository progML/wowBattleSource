using UnityEngine;

namespace Meta
{
    public class PopUpSendTest : MonoBehaviour
    {
        [SerializeField] private PopUpMessageChannel popUpMessageChannel;

        public string Title = "Lorem ipsum";
        public string Text =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna" +
            " aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco ";


        [ContextMenu("SendPopUp")]
        public void SendPopUp()
        {
            popUpMessageChannel.SpawnMessage(Title, Text);
        }
    }
}