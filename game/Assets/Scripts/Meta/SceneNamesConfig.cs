using UnityEngine;

namespace Meta
{
    [CreateAssetMenu(fileName = "SceneNamesConfig", menuName = "SceneNamesConfig", order = 0)]
    public class SceneNamesConfig : ScriptableObject
    {
        public string CoreGameplaySceneName = "CoreGameplayScene";
        public string MyBaseSceneName = "MyBaseScene";
        public string LoginSceneName = "LoginScene";
        public string MainMenuSceneName = "MainMenuScene";
    }
}