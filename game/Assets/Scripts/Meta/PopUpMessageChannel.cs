using System.Linq;
using UnityEngine;

namespace Meta
{
    [CreateAssetMenu(fileName = "PopUpMessageChannel", menuName = "PopUpMessageChannel", order = 0)]
    public class PopUpMessageChannel : ScriptableObject
    {
        [SerializeField] private PopUpMessage PopUpMessagePrefab;
        [SerializeField] private PopUpContainer PopUpContainerPrefab;

        public void SpawnMessage(string title, string text)
        {
            var popUpContainer = FindObjectOfType<PopUpContainer>();

            if (!popUpContainer)
            {
                popUpContainer = Instantiate(PopUpContainerPrefab);
            }

            PopUpMessage popUpMessage = Instantiate(PopUpMessagePrefab, popUpContainer.transform);
            popUpMessage.SetUp(title, text);
        }
    }
}