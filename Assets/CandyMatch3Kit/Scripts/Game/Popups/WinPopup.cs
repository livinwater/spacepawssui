// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


namespace GameVanilla.Game.Popups
{
    /// <summary>
    /// This class contains the logic associated to the popup that is shown when the player wins a game.
    /// </summary>
    public class WinPopup : EndGamePopup
    {
#pragma warning disable 649
        
        [SerializeField] private Image star1;
        [SerializeField] private Image star2;
        [SerializeField] private Image star3;
        // [SerializeField] private GameObject coinsGroup;
        // [SerializeField] private Text coinsText;
        [SerializeField] private Sprite disabledStarSprite;
#pragma warning restore 649

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(star1);
            Assert.IsNotNull(star2);
            Assert.IsNotNull(star3);
            // Assert.IsNotNull(coinsGroup, "coinsGroup is null in WinPopup");
            // Assert.IsNotNull(coinsText, "coinsText is null in WinPopup");
            Assert.IsNotNull(disabledStarSprite);

        }

        public void SetStars(int stars)
        {
            if (stars == 0)
            {
                star1.sprite = disabledStarSprite;
                star2.sprite = disabledStarSprite;
                star3.sprite = disabledStarSprite;
            }
            else if (stars == 1)
            {
                star2.sprite = disabledStarSprite;
                star3.sprite = disabledStarSprite;
            }
            else if (stars == 2)
            {
                star3.sprite = disabledStarSprite;
            }
        }
        
        public void OnNextButtonPressed()
        {
            // Close the popup
            Close();

            // Load the next level
            var currentLevel = PlayerPrefs.GetInt("current_level", 1);
            var nextLevel = currentLevel + 1;
            PlayerPrefs.SetInt("current_level", nextLevel);

            // Reload the GameScene
            SceneManager.LoadScene("GameScene");
        }
        
    }
}
