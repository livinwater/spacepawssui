// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using System;
using System.Collections;
using System.Globalization;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using GameVanilla.Core;
using GameVanilla.Game.Popups;
using GameVanilla.Game.UI;

namespace GameVanilla.Game.Scenes
{
    /// <summary>
    /// This class contains the logic associated to the home scene.
    /// </summary>
    public class HomeScene : BaseScene
    {
#pragma warning disable 649

        [SerializeField]
        private AnimatedButton musicButton;
        
        [SerializeField] private AudioClip levelSelectMusic; // Add this field

        
#pragma warning restore 649

        private readonly string dateLastPlayedKey = "date_last_played";
        private readonly string dailyBonusDayKey = "daily_bonus_day";
        
        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        private void Awake()
        {
            if (musicButton != null)
            {
                Assert.IsNotNull(musicButton);
            }
        }

        /// <summary>
        /// Unity's Start method.
        /// </summary>
        private void Start()
        {
            // Play level select music
            if (levelSelectMusic != null)
            {
                SoundManager.instance.SetMusicEnabled(levelSelectMusic);
            }
            UpdateButtons();
            //CheckDailyBonus();
        }

        /// <summary>
        /// Checks the daily bonus.
        /// </summary>
        private void CheckDailyBonus()
        {
            StartCoroutine(CheckDailyBonusAsync());
        }

        /// <summary>
        /// Internal coroutine to check the daily bonus.
        /// </summary>
        private IEnumerator CheckDailyBonusAsync()
        {
            yield return new WaitForSeconds(0.5f);

            if (!PlayerPrefs.HasKey(dateLastPlayedKey))
            {
                AwardDailyBonus();
                yield break;
            }
            
            var dateLastPlayedStr = PlayerPrefs.GetString(dateLastPlayedKey);
            var dateLastPlayed = Convert.ToDateTime(dateLastPlayedStr, CultureInfo.InvariantCulture);

            var dateNow = DateTime.Now;
            var diff = dateNow.Subtract(dateLastPlayed);
            if (diff.TotalHours >= 24)
            {
                if (diff.TotalHours < 48)
                {
                    AwardDailyBonus();
                }
                else
                {
                    PlayerPrefs.DeleteKey(dateLastPlayedKey);
                    PlayerPrefs.DeleteKey(dailyBonusDayKey);
                    AwardDailyBonus();
                }
            }
        }

        /// <summary>
        /// Rewards the player with the corresponding daily bonus.
        /// </summary>
        private void AwardDailyBonus()
        {
            var dateToday = DateTime.Today;
            var dateLastPlayedStr = Convert.ToString(dateToday, CultureInfo.InvariantCulture);
            PlayerPrefs.SetString(dateLastPlayedKey, dateLastPlayedStr);

            var dailyBonusDay = PlayerPrefs.GetInt(dailyBonusDayKey);
            OpenPopup<DailyBonusPopup>("Popups/DailyBonusPopup", popup => { popup.SetInfo(dailyBonusDay); });

            var newDailyBonusDay = (dailyBonusDay + 1) % 7;
            PlayerPrefs.SetInt(dailyBonusDayKey, newDailyBonusDay);
        }

        /// <summary>
        /// Called when the settings button is pressed.
        /// </summary>
        public void OnSettingsButtonPressed()
        {
            OpenPopup<SettingsPopup>("Popups/SettingsPopup");
        }

        /// <summary>
        /// Called when the sound button is pressed.
        /// </summary>
        

        /// <summary>
        /// Called when the music button is pressed.
        /// </summary>
        public void OnMusicButtonPressed()
        {
            if (musicButton != null)
            {
                var audioManager = SoundManager.instance;
                audioManager.ToggleMusic();
                musicButton.transform.GetChild(0).GetComponent<SpriteSwapper>().SetEnabled(!audioManager.IsMusicEnabled());
            }
        }

        /// <summary>
        /// Updates the state of the UI buttons according to the values stored in PlayerPrefs.
        /// </summary>
        public void UpdateButtons()
        {
            var music = PlayerPrefs.GetInt("music_enabled");
            musicButton.transform.GetChild(0).GetComponent<SpriteSwapper>().SetEnabled(music == 1);
        }
    }
}
