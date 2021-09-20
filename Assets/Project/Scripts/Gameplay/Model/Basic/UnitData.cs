namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using Util;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;

    [System.Serializable]
    public class UnitData
    {

        #region Constant Fields

        public const int MAX_ATTACK = 10;
        public const int MAX_SPEED = 10;
        public const int MAX_HP = 10;

        #endregion

        #region Inspector Fields

        [SerializeField]
        private string displayName;

        [SerializeField]
        [Required]
        [ShowAssetPreview]
        private Texture icon;

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

        [Header("AI Settings")]

        [SerializeField]
        private MoveOptionAI aiMove = MoveOptionAI.Stationary;

        [SerializeField]
        private float aIMoveDelay;

        [SerializeField]
        private ActOptionAI aiActOption = ActOptionAI.NoAction;

        [SerializeField]
        private float aIActDelay;

        #endregion

        #region Private Fields

        private readonly ReactiveProperty<int> rCurrentHp
            = new ReactiveProperty<int>(1);

        private Team team;

        #endregion

        #region Accessors

        public string DisplayName => displayName;
        public Texture Icon => icon;

        public int StatMaxHp => statMaxHp;

        public int StatSpeed => statSpeed;

        public int StatAttack => statAttack;

        public MoveOptionAI AIMove => aiMove;
        public ActOptionAI AIActOption => aiActOption;
        public float AIMoveDelay => aIMoveDelay;
        public float AIActDelay => aIActDelay;
        public Team Team => team;

        #endregion

        #region Class Implementation

        public void Init(Team team)
        {
            this.team = team;
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