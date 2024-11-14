// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using GameVanilla.Core;

namespace GameVanilla.Game.UI
{
    /// <summary>
    /// This class manages the in-game progress bar.
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        public Image progressBarImage;
        
        public Sprite idleSprite;     // Sprite for idle expression
        public Sprite happySprite;    // Sprite for happy expression
        public Sprite delightedSprite; // Sprite for delighted expression

        [HideInInspector]
        public int star1;
        [HideInInspector]
        public int star2;
        [HideInInspector]
        public int star3;

        public GameObject characterAvatar;
        private Image avatarImage;
        
        private bool star1Achieved;
        private bool star2Achieved;
        private bool star3Achieved;

        /// <summary>
        /// Unity's Start method.
        /// </summary>
        private void Start()
        {
            Debug.Log($"ProgressBar Start - characterAvatar: {characterAvatar}");
    
            if (characterAvatar != null)
            {
                // Get the Image component from the Character GameObject, not from the Mask
                avatarImage = characterAvatar.GetComponent<Image>();
                if (avatarImage == null)
                {
                    // If not found directly, try to find it in children but exclude Mask
                    avatarImage = characterAvatar.GetComponentsInChildren<Image>()
                        .FirstOrDefault(img => img.gameObject.name != "Mask");
                }
        
                Debug.Log($"Got avatarImage component: {avatarImage?.gameObject.name}");
        
                if (avatarImage == null)
                {
                    Debug.LogError("Could not find Image component in characterAvatar or its children!");
                }
                else
                {
                    avatarImage.sprite = idleSprite;
                    Debug.Log($"Set initial sprite: {idleSprite.name}");
                }
            }
            else
            {
                Debug.LogError("characterAvatar reference is null!");
            }

            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }
        }

        /// <summary>
        /// Sets the data for the progress bar.
        /// </summary>
        /// <param name="score1">The score to reach the first star.</param>
        /// <param name="score2">The score to reach the second star.</param>
        /// <param name="score3">The score to reach the third star.</param>
        public void Fill(int score1, int score2, int score3)
        {
            Debug.Log($"Fill called with scores: {score1}, {score2}, {score3}");
            
            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0;
            }

            star1 = score1;
            star2 = score2;
            star3 = score3;

            Debug.Log($"Stars set to: {star1}, {star2}, {star3}");

            // Reset achievement flags
            star1Achieved = false;
            star2Achieved = false;
            star3Achieved = false;

            // Set initial avatar sprite
            if (avatarImage != null && idleSprite != null)
            {
                Debug.Log("Setting initial idle sprite");
                avatarImage.sprite = idleSprite;
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.PlaySound("StarProgressBar");
                }
            }
            else
            {
                Debug.LogError($"Avatar Image or Idle Sprite is null. avatarImage: {avatarImage}, idleSprite: {idleSprite}");
            }

            UpdateProgressBar(0);
        }

        /// <summary>
        /// Updates the progress bar with the specified score.
        /// </summary>
        /// <param name="score">The current score.</param>
        public void UpdateProgressBar(int score)
        {
            Debug.Log($"UpdateProgressBar called with score: {score}");
            
            if (progressBarImage != null)
            {
                float fillAmount = GetProgressValue(score) / 100.0f;
                Debug.Log($"Setting fill amount to: {fillAmount}");
                progressBarImage.fillAmount = fillAmount;
            }

            if (avatarImage != null)
            {
                Debug.Log($"Current score: {score}, Star thresholds: {star1}/{star2}/{star3}");
                Debug.Log($"Current avatar state - star1Achieved: {star1Achieved}, star2Achieved: {star2Achieved}, star3Achieved: {star3Achieved}");
                
                if (score >= star1 && !star1Achieved && idleSprite != null)
                {
                    Debug.Log("Setting idle sprite");
                    star1Achieved = true;
                    avatarImage.sprite = idleSprite;
                    if (SoundManager.instance != null)
                    {
                        SoundManager.instance.PlaySound("StarProgressBar");
                    }
                }
                if (score >= star2 && !star2Achieved && happySprite != null)
                {
                    Debug.Log("Setting happy sprite");
                    star2Achieved = true;
                    avatarImage.sprite = happySprite;
                    if (SoundManager.instance != null)
                    {
                        SoundManager.instance.PlaySound("StarProgressBar");
                    }
                }
                if (score >= star3 && !star3Achieved && delightedSprite != null)
                {
                    Debug.Log("Setting delighted sprite");
                    star3Achieved = true;
                    avatarImage.sprite = delightedSprite;
                    if (SoundManager.instance != null)
                    {
                        SoundManager.instance.PlaySound("StarProgressBar");
                    }
                }
                
                if (avatarImage.sprite != null)
                {
                    Debug.Log($"Current sprite: {avatarImage.sprite.name}");
                }
                else
                {
                    Debug.LogError("Current sprite is null!");
                }
            }
            else
            {
                Debug.LogError("Avatar Image is null!");
            }
        }

        /// <summary>
        /// Returns the progress of the bar at the specified value.
        /// </summary>
        /// <param name="value">The value to use as a reference for the progress.</param>
        /// <returns></returns>
        private int GetProgressValue(int value)
        {
            const int oldMin = 0;
            var oldMax = star3;
            const int newMin = 0;
            const int newMax = 100;
            var oldRange = oldMax - oldMin;
            const int newRange = newMax - newMin;
            var newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
            return newValue;
        }
    }
}
