﻿namespace ReGaSLZR.Gameplay.View
{
    
    using Base;
    using Enum;
    using Model;
    using Util;

    using NaughtyAttributes;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class LevelOutcomeView : BaseReactiveMonoBehaviour
    {

        [SerializeField]
        [Required]
        private GameObject parentView;

        [SerializeField]
        [Required]
        private TextMeshProUGUI textTeamWinner;

        [SerializeField]
        [Required]
        private GameObject parentPlayerWinDesign;

        [SerializeField]
        [Required]
        private GameObject parentEnemyWinDesign;

        [Inject]
        private readonly ITeam.IEnemyGetter iEnemy;

        [Inject]
        private readonly ITeam.IPlayerGetter iPlayer;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

        [Inject]
        private readonly ThemeColors iThemeColors;

        [Inject]
        private readonly ILevel.IGetter iLevel;

        protected override void RegisterObservables()
        {
            iLevel.GetState()
                .Where(state => state == LevelState.Ended)
                .Subscribe(_ => {
                    LogUtil.PrintInfo(GetType(), "On Level End");

                    iLevelSetter.SetLog("Game Over!");
                    var isPlayerWinner = (iEnemy.GetStatus().Value == TeamStatus.WipedOut);

                    textTeamWinner.text = isPlayerWinner
                        ? Team.Player.ToString() : Team.Enemy.ToString();
                    textTeamWinner.color = isPlayerWinner 
                        ? iThemeColors.PlayerUnitBG : iThemeColors.EnemyUnitBG;

                    parentPlayerWinDesign.SetActive(isPlayerWinner);
                    parentEnemyWinDesign.SetActive(!isPlayerWinner);
                    parentView.SetActive(true);
                })
                .AddTo(disposablesBasic);
        }

        private void Start()
        {
            parentView.SetActive(false);
        }

    }

}