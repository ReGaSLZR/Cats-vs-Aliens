namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Enum;

    using UnityEngine;

    public class AIAction : BaseAction
    {

        protected override void OnAct()
        {
            switch (unitController.Unit.Data.AIActOption)
            {
                case ActOptionAI.AttackIsHasTarget:
                    {
                        //TODO ren
                        break;
                    }
                case ActOptionAI.NoAction:
                default:
                    {
                        FinishAct();
                        break;
                    }
            }
        }

    }

}