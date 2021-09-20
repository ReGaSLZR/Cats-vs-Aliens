namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Model;
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
        {
            //this.UpdateAsObservable()
            //    .Where(_ => Input.GetMouseButtonDown(0))
            //    .Subscribe()
            //    .

            //FinishAct(); //TODO ren
        }

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
            //TODO
            if (iTileGetter.IsTileOnCrossRange(
                unitController.Unit.currentTile, unit.currentTile))
            {
                unit.Data.Damage(unitController.Unit.Data.StatAttack);
            }

            yield return null;
            FinishAct();
        }

    }

}