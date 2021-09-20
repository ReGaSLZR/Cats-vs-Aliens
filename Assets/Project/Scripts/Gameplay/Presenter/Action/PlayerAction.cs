namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Model;
    using Util;

    using System.Collections;
    using UniRx;
    using Zenject;

    public class PlayerAction : BaseAction
    {

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ITile.IGetter iTileGetter;


        protected override void OnAct(){}

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

        private IEnumerator CorOnAct(Model.Unit unit)
        {
            if (iTileGetter.IsTileOnCrossRange(
                unitController.Unit.currentTile, unit.currentTile))
            {
                if (unit.Data.Team == Enum.Team.Player)
                {
                    LogUtil.PrintInfo(GetType(), $"CorOnMove(): " +
                        $"Cannot hit allies.");
                    iLevelSetter.SetLog($"Player Unit {unitController.Unit.Data.DisplayName} tried to hit ally {unit.Data.DisplayName}. Invalid act. Auto-skipping.");
                }
                else
                {
                    iLevelSetter.SetLog($"Player Unit {unitController.Unit.Data.DisplayName} attacked Enemy Unit {unit.Data.DisplayName} with {unitController.Unit.Data.StatAttack} damage.");
                    unit.Data.Damage(unitController.Unit.Data.StatAttack);
                }
            }
            else if(!unit.currentTile.Position.Equals(unitController.Unit.currentTile.Position))
            {
                iLevelSetter.SetLog($"Target {unit.Data.DisplayName} is not in range of Player Unit {unitController.Unit.Data.DisplayName}. Auto-skipping act.");
            }

            yield return null;
            FinishAct();
        }

    }

}