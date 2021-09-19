namespace ReGaSLZR.Gameplay.Model
{

    using Util;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Unit", menuName = "Project/Create New Unit")]
    public class Unit : ScriptableObject
    {

        #region Inspector Fields

        [SerializeField]
        private string displayName;

        [ShowAssetPreview]
        [SerializeField]
        private Texture icon;

        [SerializeField]
        private UnitHolder prefab;

        [Header("Stats")]

        [SerializeField]
        [Range(0, 50)]
        private int statMaxHp;
        [SerializeField]
        [Range(0, 10)]
        private int statSpeed;
        [SerializeField]
        [Range(1, 10)]
        private int statAttack;

        [Header("Per Turn Stats")]

        [SerializeField]
        [Range(0, 5)]
        private int movementPerTurn;
        [SerializeField]
        [Range(0, 5)]
        private int skillUsesPerTurn;

        #endregion

        #region Private Fields

        private readonly ReactiveProperty<int> rCurrentHp
            = new ReactiveProperty<int>();

        #endregion

        #region Accessors

        public string DisplayName => displayName;
        public Texture Icon => icon;

        public UnitHolder Prefab {
                get {
                    prefab.SetUpUnit(this);
                    return prefab;
                }
            }

        public int StatMaxHp => statMaxHp;

        public int StatSpeed => statSpeed;

        public int StatAttack => statAttack;

        #endregion

        #region Class Implementation

        public void Init()
        {
            rCurrentHp.Value = StatMaxHp;
        }

        public void Heal(int healValue)
        {
            if (healValue < 0)
            {
                LogUtil.PrintWarning(GetType(), $"Heal(): healValue of {healValue} is not accepted.");
                return;
            }

            rCurrentHp.Value = Mathf.Min(statMaxHp, rCurrentHp.Value + healValue);
        }

        public void Damage(int damageValue)
        {
            if (damageValue < 0)
            {
                LogUtil.PrintWarning(GetType(), $"Damage(): damageValue of {damageValue} is not accepted.");
                return;
            }

            rCurrentHp.Value = Mathf.Max(0, rCurrentHp.Value - damageValue);
        }

        public IReadOnlyReactiveProperty<int> GetCurrentHp()
        {
            return rCurrentHp;
        }

        #endregion

    }

}