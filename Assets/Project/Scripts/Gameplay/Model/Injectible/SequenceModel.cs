namespace ReGaSLZR.Gameplay.Model
{

    using NaughtyAttributes;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class SequenceModel : MonoInstaller,
        ISequence.ISetter, ISequence.IGetter
    {

        #region Private Fields

        private readonly ReactiveProperty<Unit> rSequencedUnit
            = new ReactiveProperty<Unit>();

        [SerializeField]
        [ReadOnly]
        private List<Unit> sequencedUnits = new List<Unit>();

        private int sequencedIndex;

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
            rSequencedUnit.Value = null;
            sequencedIndex = 0;
        }

        #endregion

        #region Setter

        public void AddUnitForSequence(Unit unitHolder)
        {
            sequencedUnits.Add(unitHolder);
        }

        public void OrganizeSequence()
        {
             //TODO

            sequencedIndex = 0;
            rSequencedUnit.Value = sequencedUnits[sequencedIndex];
        }

        public void FinishSequence(Unit unitHolder)
        {
            if (unitHolder == null || rSequencedUnit.Value == null ||
                (unitHolder.GetInstanceID() != rSequencedUnit.Value.GetInstanceID()))
            {
                return;
            }
            
            sequencedIndex = (sequencedIndex >= (sequencedUnits.Count - 1)) ?
                0 : (sequencedIndex + 1);
            rSequencedUnit.Value = sequencedUnits[sequencedIndex];
        }

        #endregion

        #region Getter

        public IReadOnlyReactiveProperty<Unit> GetSequencedUnit()
        {
            return rSequencedUnit;
        }

        #endregion

    }

}