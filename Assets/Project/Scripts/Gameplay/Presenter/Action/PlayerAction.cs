namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Model;
    using Util;

    using System.Collections;
    using UniRx;
    using Zenject;
    using UnityEngine;

    public class PlayerAction : BaseAction
    {

        #region Private Fields

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ITile.IGetter iTileGetter;

        #endregion

        protected override void OnAct(){}

        #region Unity Callbacks

        private void Start()
        {
            iLevelGetter.GetSelectedUnit()
                .Where(unit => unit != null)
                .Where(unit => unitController.GetIsActive().Value)
                .Where(unit => unitController.GetIsActionAllowed().Value)
                .Subscribe(unit =>
                {
                    StartCoroutine(CorOnAct(unit));
                })
                .AddTo(disposablesTerminal);
        }

        #endregion

        #region Class Implementation

        private IEnumerator CorOnAct(Model.Unit unit)
        {
            if (iTileGetter.IsTileOnCrossRange(
                unitController.Unit.currentTile, unit.currentTile))
            {
                if (unit.Data.Team == Enum.Team.Player)
                {
                    LogUtil.PrintInfo(GetType(), $"CorOnMove(): " +
                        $"Cannot hit allies.");
                    iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}> {unitController.Unit.Data.DisplayName}</color> tried to hit ally <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>{unit.Data.DisplayName}</color>. <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogInvalid)}>Invalid act. Auto-skipping.</color>");
                }
                else
                {
                    iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>{unitController.Unit.Data.DisplayName}</color> attacked <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}> {unit.Data.DisplayName}</color> with <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogCritical)}>{unitController.Unit.Data.StatAttack} damage.</color>");
                    unit.Data.Damage(unitController.Unit.Data.StatAttack);
                }
            }
            else if(!unit.currentTile.Position.Equals(unitController.Unit.currentTile.Position))
            {
                iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogInvalid)}>Target {unit.Data.DisplayName} is not in range of <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>{unitController.Unit.Data.DisplayName}. </color>Auto-skipping act.</color>");
            }

            yield return null;
            FinishAct();
        }

        #endregion

    }

}