namespace ReGaSLZR.Gameplay.Model
{

    using Util;

    using UniRx;
    using UnityEngine;

    [System.Serializable]
    public class UnitData
    {

        public const int MAX_ATTACK = 10;
        public const int MAX_SPEED = 10;
        public const int MAX_HP = 10;

        #region Inspector Fields

        [SerializeField]
        private string displayName;

        [Header("Stats")]

        [SerializeField]
        [Range(0, MAX_HP)]
        private int statMaxHp;
        [SerializeField]
        [Range(1, MAX_SPEED)]
        private int statSpeed;
        [SerializeField]
        [Range(1, MAX_ATTACK)]
        private int statAttack;

        [Header("Per Turn Stats")]

        [SerializeField]
        [Range(0, 5)]
        private int movementPerTurn;
        [SerializeField]
        [Range(0, 5)]
        private int skillUsesPerTurn;

        #endregion

        private readonly ReactiveProperty<int> rCurrentHp
            = new ReactiveProperty<int>(1);

        #region Accessors

        public string DisplayName => displayName;

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