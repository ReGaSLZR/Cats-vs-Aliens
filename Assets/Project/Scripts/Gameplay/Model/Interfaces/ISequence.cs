namespace ReGaSLZR.Gameplay.Model
{
    using System.Collections.Generic;
    using UniRx;

    public static class ISequence 
    {

        public interface ISetter
        {
            void AddUnitForSequence(Unit unitHolder);
            void OrganizeSequence();
            void FinishSequence(Unit unitHolder);
        }

        public interface IGetter
        {
            IReadOnlyReactiveProperty<Unit> GetActiveUnit();
            Unit GetUnitAtTile(Tile tile);
            List<Unit> GetUnits();
        }

    }

}