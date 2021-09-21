namespace ReGaSLZR.Gameplay.Model
{

    using Base;
    using Enum;

    using NaughtyAttributes;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UniRx;

    public class Unit : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [ReadOnly]
        public Tile currentTile;

        [SerializeField]
        private UnitData data;

        #endregion

        #region Accessor

        public UnitData Data => data;

        private readonly ReactiveProperty<bool> rOnInit
            = new ReactiveProperty<bool>(false);

        #endregion

        #region Class Implementation

        public void Init(Team team)
        {
            data.Init(team);
            rOnInit.Value = true;
        }

        public IReadOnlyReactiveProperty<bool> GetOnInit()
        {
            return rOnInit;
        }

        #endregion
    }

}