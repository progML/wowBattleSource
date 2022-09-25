using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meta
{
    [CreateAssetMenu(fileName = "SceneLoaderChannel", menuName = "ScriptableObjects/SceneLoaderChannel", order = 0)]
    public class SceneLoaderChannel : ScriptableObject
    {
        [SerializeField] private SceneNamesConfig sceneNamesConfig;

        public void LoadLoginScene()
        {
            SceneManager.LoadScene(sceneNamesConfig.LoginSceneName);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(sceneNamesConfig.MainMenuSceneName);
        }

        public void LoadCoreGameplay()
        {
            SceneManager.LoadScene(sceneNamesConfig.CoreGameplaySceneName);
        }

        public void LoadMyBase()
        {
            SceneManager.LoadScene(sceneNamesConfig.MyBaseSceneName);
        }
    }
}