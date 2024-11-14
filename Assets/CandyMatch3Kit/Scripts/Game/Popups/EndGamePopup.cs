using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.UI;
using FullSerializer;
using GameVanilla.Game.Scenes;


namespace GameVanilla.Game.Popups
{
    /// <summary>
    /// This class contains the logic associated to the popup that is shown when a game ends.
    /// </summary>
    public class EndGamePopup : Popup
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject goalGroup;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Text levelText; // Added
        [SerializeField] private Text scoreOnlyReachedText; // Added

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(titleText, "titleText is null in EndGamePopup");
            Assert.IsNotNull(scoreText, "scoreText is null in EndGamePopup");
            Assert.IsNotNull(goalGroup, "goalGroup is null in EndGamePopup");
            Assert.IsNotNull(goalPrefab, "goalPrefab is null in EndGamePopup");
            Assert.IsNotNull(homeButton, "homeButton is null in EndGamePopup");
            Assert.IsNotNull(retryButton, "retryButton is null in EndGamePopup");
            Assert.IsNotNull(levelText, "levelText is null in EndGamePopup"); // Added
            Assert.IsNotNull(scoreOnlyReachedText, "scoreOnlyReachedText is null in EndGamePopup"); // Added
        }

        public void OnReplayButtonPressed()
        {
            var gameScene = parentScene as GameScene;
            if (gameScene != null)
            {
                var numLives = PlayerPrefs.GetInt("num_lives");
                if (numLives > 0)
                {
                    gameScene.RestartGame();
                    Close();
                }
                else
                {
                    gameScene.OpenPopup<BuyLivesPopup>("Popups/BuyLivesPopup");
                }
            }
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetLevel(int level)
        {
            levelText.text = "Level " + level;
        }

        public void SetGoals(GameObject group)
        {
            var goals = group.GetComponentsInChildren<GoalUiElement>();
            if (goals.Length > 0)
            {
                scoreOnlyReachedText.gameObject.SetActive(false);
                foreach (var goal in goals)
                {
                    var goalObject = Instantiate(goal.gameObject);
                    goalObject.transform.SetParent(goalGroup.transform, false);
                    goalObject.GetComponent<GoalUiElement>().SetCompletedTick(goal.isCompleted);
                }
            }
            else
            {
                scoreOnlyReachedText.gameObject.SetActive(true);
            }
        }
    } // Correctly closes the class
} // Correctly closes the namespace
