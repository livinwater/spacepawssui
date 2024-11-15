// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.Scenes;

namespace GameVanilla.Game.Popups
{
    /// <summary>
    /// This class contains the logic associated to the settings popup.
    /// </summary>
    public class SettingsPopup : Popup
    {
#pragma warning disable 649
        [SerializeField]
        private Slider musicSlider;

        [SerializeField]
        private AnimatedButton resetProgressButton;

        [SerializeField]
        private Image resetProgressImage;

        [SerializeField]
        private Sprite resetProgressDisabledSprite;
#pragma warning restore 649

        private int currentMusic;

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(musicSlider);
            Assert.IsNotNull(resetProgressButton);
            Assert.IsNotNull(resetProgressImage);
            Assert.IsNotNull(resetProgressDisabledSprite);
        }

        /// <summary>
        /// Unity's Start method.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            musicSlider.value = PlayerPrefs.GetInt("music_enabled");
        }

        /// <summary>
        /// Called when the close button is pressed.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            Close();
        }

        /// <summary>
        /// Called when the save button is pressed.
        /// </summary>
        public void OnSaveButtonPressed()
        {
            PlayerPrefs.SetInt("music_enabled", currentMusic);
            SoundManager.instance.SetMusicEnabled(currentMusic == 1);
            var homeScene = parentScene as HomeScene;
            if (homeScene != null)
            {
                homeScene.UpdateButtons();
            }
            Close();
        }

        /// <summary>
        /// Called when the reset progress button is pressed.
        /// </summary>
        public void OnResetProgressButtonPressed()
        {
            PuzzleMatchManager.instance.lastSelectedLevel = 0;
            PlayerPrefs.SetInt("next_level", 0);
            for (var i = 1; i <= 30; i++)
            {
                PlayerPrefs.DeleteKey(string.Format("level_stars_{0}", i));
            }
            resetProgressImage.sprite = resetProgressDisabledSprite;
            resetProgressButton.interactable = false;
        }

        /// <summary>
        /// Called when the help button is pressed.
        /// </summary>
        public void OnHelpButtonPressed()
        {
            parentScene.OpenPopup<AlertPopup>("Popups/AlertPopup", popup =>
            {
                popup.SetTitle("Help");
                popup.SetText("Do you need help?");
            }, false);
        }

        /// <summary>
        /// Called when the info button is pressed.
        /// </summary>
        public void OnInfoButtonPressed()
        {
            parentScene.OpenPopup<AlertPopup>("Popups/AlertPopup", popup =>
            {
                popup.SetTitle("About");
                popup.SetText("Created by gamevanilla.\n Copyright (C) 2018.");
            }, false);
        }

        /// <summary>
        /// Called when the music slider value is changed.
        /// </summary>
        public void OnMusicSliderValueChanged()
        {
            currentMusic = (int)musicSlider.value;
        }
    }
}
