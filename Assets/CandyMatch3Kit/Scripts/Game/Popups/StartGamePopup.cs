using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using FullSerializer;
using TMPro;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.UI;

namespace GameVanilla.Game.Popups
{
    /// <summary>
    /// This class contains the logic associated with the popup that is shown before starting a game.
    /// </summary>
    public class StartGamePopup : Popup
    {
        #pragma warning disable 649
        [SerializeField]
        private Text levelText;

        [SerializeField]
        private StakingSliderHandler stakingSliderHandler;

        [SerializeField]
        private TextMeshProUGUI stakingAmountText;

        [SerializeField]
        private Button stakeButton;

        [SerializeField]
        private Button playButton;

        [SerializeField]
        private AnimatedButton closeButton;

        [SerializeField]
        private TextMeshProUGUI statusMessageText;

        private int numLevel;
        private int stakingAmount;
        private bool hasStaked = false;

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(levelText);
            Assert.IsNotNull(stakingSliderHandler);
            Assert.IsNotNull(stakingAmountText);
            Assert.IsNotNull(stakeButton);
            Assert.IsNotNull(playButton);
            Assert.IsNotNull(closeButton);
            Assert.IsNotNull(statusMessageText);
        }
        
        protected new void Start()
        {
            base.Start();
            
            // Set the Play button to be non-interactable initially
            playButton.interactable = false;

            // Assign button listeners
            stakeButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            

            stakeButton.onClick.AddListener(OnStakeButtonPressed);
            playButton.onClick.AddListener(OnPlayButtonPressed);
            closeButton.onClick.AddListener(OnCloseButtonPressed);

            // Initialize status message
            statusMessageText.text = "Please select a staking amount and press Stake.";
            
            // Subscribe to staking amount changes
            stakingSliderHandler.OnStakingAmountChanged += OnStakingAmountChanged;
        }

        public void OnStakeButtonPressed()
        {
            stakingAmount = stakingSliderHandler.GetSelectedStakingAmount();

            if (stakingAmount > 0)
            {
                hasStaked = true;
                playButton.interactable = true;
                stakeButton.interactable = false;
                
                // Get the Text component from the stake button's children
                var buttonText = stakeButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "Staked";
                }

                statusMessageText.text = $"You have staked {stakingAmount} sui. You can now play.";
            }
            else
            {
                statusMessageText.text = "Please select a staking amount greater than zero.";
            }
        }

        private void OnDestroy()
        {
            // Clean up event subscription
            if (stakingSliderHandler != null)
            {
                stakingSliderHandler.OnStakingAmountChanged -= OnStakingAmountChanged;
            }
        }

        /// <summary>
        /// Loads the level data corresponding to the specified level number.
        /// </summary>
        /// <param name="levelNum">The number of the level to load.</param>
        public void LoadLevelData(int levelNum)
        {
            numLevel = levelNum;

            // Add validation
            var serializer = new fsSerializer();
            try 
            {
                var level = FileUtils.LoadJsonFile<Level>(serializer, "Levels/" + numLevel);
                if (level == null)
                {
                    Debug.LogError($"Failed to load level {numLevel} - level data is null");
                    return;
                }
                levelText.text = "Level " + numLevel;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load level {numLevel}: {e.Message}");
                return;
            }
        }
        
        /// <summary>
        /// Called when the play button is pressed.
        /// </summary>
        
        public void OnPlayButtonPressed()
        {
            if (hasStaked)
            {
                // Add validation
                if (numLevel <= 0)
                {
                    Debug.LogError("Invalid level number: " + numLevel);
                    return;
                }

                // Pass stakingAmount to your game logic
                PuzzleMatchManager.instance.lastSelectedLevel = numLevel;
                PuzzleMatchManager.instance.stakingAmount = stakingAmount;
                GetComponent<SceneTransition>().PerformTransition();
            }
            else
            {
                statusMessageText.text = "You must stake before playing.";
            }
        }
        /// <summary>
        /// Called when the close button is pressed.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            Close();
        }

        private void OnStakingAmountChanged(int newAmount)
        {
            if (hasStaked)
            {
                hasStaked = false;
                playButton.interactable = false;
                stakeButton.interactable = true;
                
                // Get the Text component from the stake button's children
                var buttonText = stakeButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "Stake";
                }

                statusMessageText.text = "Staking amount changed. Please stake again.";
            }
        }
    }
}
