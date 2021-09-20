namespace ReGaSLZR.Gameplay.Model
{

    using NaughtyAttributes;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class SequenceModel : MonoInstaller,
        ISequence.ISetter, ISequence.IGetter
    {

        #region Private Fields

        private readonly ReactiveProperty<Unit> rActiveUnit
            = new ReactiveProperty<Unit>();

        [SerializeField]
        [ReadOnly]
        private List<Unit> sequencedUnits = new List<Unit>();

        private int currentIndex;

        #endregion

        #region Class Overrides

        public override void InstallBindings()
        {
            InitValues();

            Container.Bind<ISequence.IGetter>().FromInstance(this);
            Container.Bind<ISequence.ISetter>().FromInstance(this);
        }

        #endregion

        #region Class Implementation

        private void InitValues()
        {
            rActiveUnit.Value = null;
            currentIndex = 0;
        }

        private void SetNextInSequence()
        {
            bool isUnset = true;
            while (isUnset)
            {
                currentIndex = (currentIndex >= (sequencedUnits.Count - 1)) ?
                        0 : (currentIndex + 1);
                
                if (sequencedUnits[currentIndex].Data.GetCurrentHp().Value > 0)
                {
                    rActiveUnit.Value = sequencedUnits[currentIndex];
                    isUnset = false;
                }
            }
        }

        #endregion

        #region Setter

        public void AddUnitForSequence(Unit unitHolder)
        {
            sequencedUnits.Add(unitHolder);
        }

        public void OrganizeSequence()
        {
            sequencedUnits = sequencedUnits
                .OrderByDescending(unit => unit.Data.StatSpeed)
                .ToList();

            currentIndex = 0;
            rActiveUnit.SetValueAndForceNotify(sequencedUnits[currentIndex]);
        }

        public void FinishSequence(Unit unitHolder)
        {
            if (unitHolder == null || rActiveUnit.Value == null ||
                (unitHolder.GetInstanceID() != rActiveUnit.Value.GetInstanceID()))
            {
                return;
            }

            SetNextInSequence();
        }

        #endregion

        #region Getter

        public IReadOnlyReactiveProperty<Unit> GetActiveUnit()
        {
            return rActiveUnit;
        }

        public Unit GetUnitAtTile(Tile tile)
        {
            if (tile == null)
            {
                return null;
            }

            foreach (var unit in sequencedUnits)
            {
                if (unit != null && unit.currentTile != null &&
                    unit.currentTile.GetInstanceID() == tile.GetInstanceID())
                {
                    return unit;
                }
            }

            return null;
        }

        public List<Unit> GetUnits()
        {
            return sequencedUnits;
        }

        #endregion

    }

}