namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Model;
    using Util;

    using System.Collections;
    using UniRx;
    using UniRx.Triggers;

    using UnityEngine;
    using Zenject;

    public class PlayerAction : BaseAction
    {

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ITile.IGetter iTileGetter;

        protected override void OnAct()
        {}

        private void Start()
        {
            iLevelGetter.GetSelectedUnit()
                .Where(unit => unit != null)
                .Where(unit => unitController.GetIsActive().Value)
                .Where(unit => unitController.GetIsActionAllowed().Value)
                .Subscribe(unit =>
                {
                    StartCoroutine(CorOnMove(unit));
                })
                .AddTo(disposablesTerminal);
        }

        private IEnumerator CorOnMove(Model.Unit unit)
        {
            if (iTileGetter.IsTileOnCrossRange(
                unitController.Unit.currentTile, unit.currentTile))
            {
                if (unit.Data.Team == Enum.Team.Player)
                {
                    LogUtil.PrintInfo(GetType(), $"CorOnMove(): " +
                        $"Cannot hit allies.");
                }
                else { 
                    unit.Data.Damage(unitController.Unit.Data.StatAttack);
                }
            }

            yield return null;
            FinishAct();
        }

    }

}