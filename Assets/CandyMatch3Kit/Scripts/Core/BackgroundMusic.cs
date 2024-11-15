using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameVanilla.Core
{
    /// <summary>
    /// This class manages the background music of the game.
    /// </summary>
    public class BackgroundMusic : MonoBehaviour
    {
        private static BackgroundMusic instance;
        private AudioSource audioSource;

        // Singleton instance accessor
        public static BackgroundMusic Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            // Subscribe to scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Unity's Start method.
        /// </summary>
        private void Start()
        {
            var music = PlayerPrefs.GetInt("music_enabled", 1);
            audioSource.mute = music == 0;
            audioSource.Play();
        }

        /// <summary>
        /// Method to change the music clip.
        /// </summary>
        /// <param name="newClip">The new music clip to play.</param>
        public void ChangeMusic(AudioClip newClip)
        {
            if (newClip == null)
            {
                Debug.LogWarning("Attempted to change music to a null clip.");
                return;
            }

            if (audioSource.clip == newClip)
                return; // Already playing this clip

            audioSource.Stop();
            audioSource.clip = newClip;

            var music = PlayerPrefs.GetInt("music_enabled", 1);
            audioSource.mute = music == 0;
            audioSource.Play();
        }

        /// <summary>
        /// Called when a scene is loaded.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                // Load and play the game scene music
                AudioClip gameSceneMusic = Resources.Load<AudioClip>("Music/GameScene");
                ChangeMusic(gameSceneMusic);
            }
            else if (scene.name == "MainMenuScene")
            {
                // Load and play the main menu music
                AudioClip mainMenuMusic = Resources.Load<AudioClip>("Music/MainMenu");
                ChangeMusic(mainMenuMusic);
            }
            // Add more conditions for other scenes if needed
        }

        /// <summary>
        /// Unity's OnDestroy method.
        /// </summary>
        private void OnDestroy()
        {
            // Unsubscribe from the scene loaded event
            if (instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }
    }
}
