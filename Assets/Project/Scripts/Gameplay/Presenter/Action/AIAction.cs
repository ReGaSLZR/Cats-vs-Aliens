namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Enum;
    using Model;
    using Util;

    using UnityEngine;
    using Zenject;
    using System.Collections;

    public class AIAction : BaseAction
    {

        [Inject]
        private readonly ITile.IGetter iTileGetter;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        protected override void OnAct()
        {
            StopAllCoroutines();
            StartCoroutine(CorAct());
        }

        private IEnumerator CorAct()
        {
            yield return new WaitForSeconds(unitController.Unit.Data.AIActDelay);

            switch (unitController.Unit.Data.AIActOption)
            {
                case ActOptionAI.AttackIfHasTarget:
                    {
                        TryAttackIfHasTargetInRange();
                        break;
                    }
                case ActOptionAI.NoAction:
                default:
                    {
                        LogUtil.PrintInfo(GetType(), "NoAction");
                        iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogInfo)}>Pacifist <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}> {unitController.Unit.Data.DisplayName}</color> took no action.</color>");
                        FinishAct();
                        break;
                    }
            }
        }

        private void TryAttackIfHasTargetInRange() 
        {
            LogUtil.PrintInfo(GetType(), "TryAttackIfHasTargetInRange()");
            var tilesInRange = iTileGetter.GetTilesOnCrossRange(unitController.Unit.currentTile);

            foreach (var tile in tilesInRange)
            { 
                if (tile != null && tile.isOccupied)
                {
                    var unit = iSequenceGetter.GetUnitAtTile(tile);
                    if (unit != null && (unit.Data.Team == Team.Player)
                        && unit.Data.GetCurrentHp().Value > 0)
                    {
                        unit.Data.Damage(unitController.Unit.Data.StatAttack);
                        iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}>{unitController.Unit.Data.DisplayName}</color> attacked <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>{unit.Data.DisplayName}</color> with <color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogCritical)}>{unitController.Unit.Data.StatAttack} damage.</color>");
                        FinishAct();
                        return;
                    }
                }
            }

            iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogInvalid)}><color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}>{unitController.Unit.Data.DisplayName}</color> had no targets in close range. Skipping act.</color>");
            FinishAct();
        }

    }

}