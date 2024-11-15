using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.Scenes;

namespace GameVanilla.Game.Popups
{
    public class SettingsPopup : Popup
    {
        #pragma warning disable 649
        [SerializeField]
        private AnimatedButton resetProgressButton;

        [SerializeField]
        private Image resetProgressImage;

        [SerializeField]
        private Sprite resetProgressDisabledSprite;
        #pragma warning restore 649

        private int currentMusic; // Add this if you removed musicSlider
        
        protected override void Awake()
        {
            base.Awake();
            // Verify these components exist in Unity Inspector
            Assert.IsNotNull(resetProgressButton, "Reset Progress Button is missing");
            Assert.IsNotNull(resetProgressImage, "Reset Progress Image is missing");
            Assert.IsNotNull(resetProgressDisabledSprite, "Reset Progress Disabled Sprite is missing");
        }

        protected override void Start()
        {
            base.Start();
            currentMusic = PlayerPrefs.GetInt("music_enabled");
        }

        public void OnCloseButtonPressed()
        {
            Close();
        }

        public void OnSaveButtonPressed()
        {
            PlayerPrefs.SetInt("music_enabled", currentMusic);
            SoundManager.instance.SetMusicEnabled(currentMusic == 1);
            if (parentScene is HomeScene homeScene)
            {
                homeScene.UpdateButtons();
            }
            Close();
        }

        public void OnResetProgressButtonPressed()
        {
            PuzzleMatchManager.instance.lastSelectedLevel = 0;
            PlayerPrefs.SetInt("next_level", 0);
            for (var i = 1; i <= 30; i++)
            {
                PlayerPrefs.DeleteKey($"level_stars_{i}");
            }
            resetProgressImage.sprite = resetProgressDisabledSprite;
            resetProgressButton.interactable = false;
        }
    }
}