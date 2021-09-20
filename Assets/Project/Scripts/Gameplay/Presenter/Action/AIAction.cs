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
                        FinishAct();
                        return;
                    }
                }
            }

            FinishAct();
        }

    }

}