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
        private GameObject prefab;

        [Header("Stats")]

        [SerializeField]
        private int statMaxHp;
        [SerializeField]
        private int statSpeed;
        [SerializeField]
        private int statAttack;

        #endregion

        #region Private Fields

        private readonly ReactiveProperty<int> rCurrentHp 
            = new ReactiveProperty<int>();

        #endregion

        #region Accessors

        public string DisplayName => displayName;
        public Texture Icon => icon;

        public int StatMaxHp => statMaxHp;

        public int StatSpeed => statSpeed;

        public int StatAttack => statAttack;

        #endregion

        #region Class Implementation

        public void Init()
        {
            rCurrentHp.Value = StatMaxHp;
        }

        public void Spawn(Vector2 position, Transform parent)
        {
            var spawn = Instantiate(prefab, parent);
            spawn.transform.position = new Vector3(position.x, position.y, 0);
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