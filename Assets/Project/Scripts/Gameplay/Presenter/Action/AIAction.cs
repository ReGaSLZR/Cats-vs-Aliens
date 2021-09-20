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
                        iLevelSetter.SetLog($"Pacifist enemy unit {unitController.Unit.Data.DisplayName} took no action.");
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
                        iLevelSetter.SetLog($"Enemy {unitController.Unit.Data.DisplayName} attacked {unit.Data.DisplayName} with {unitController.Unit.Data.StatAttack} damage.");
                        FinishAct();
                        return;
                    }
                }
            }

            iLevelSetter.SetLog($"Enemy {unitController.Unit.Data.DisplayName} had no targets in close range. Skipping act.");
            FinishAct();
        }

    }

}