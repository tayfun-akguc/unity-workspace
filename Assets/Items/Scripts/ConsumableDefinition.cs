using UnityEngine;

namespace Items.Scripts
{
    [CreateAssetMenu(menuName = "Game/Items/Consumable", fileName = "NewConsumable")]
    public class ConsumableDefinition : ItemDefinition
    {
        [Header("Consumable")] [Min(0f)] [SerializeField]
        private float cooldownSeconds = 0f;

        // implement effects later (Heal, Buff, etc.)
        public float CooldownSeconds
        {
            get => cooldownSeconds;
            set => cooldownSeconds = value;
        }

        public static Builder NewBuilder() => new Builder();

        public sealed class Builder : ItemDefinition.Builder<ConsumableDefinition, Builder>
        {
            private float _cooldownSeconds;
            
            protected override Builder Self() => this;

            protected override ConsumableDefinition Create()
                => ScriptableObject.CreateInstance<ConsumableDefinition>();

            public Builder CooldownSeconds(float v) { _cooldownSeconds = v; return this; }

            public override ConsumableDefinition Build()
            {
                var def = base.Build();
                def.CooldownSeconds = _cooldownSeconds;
                return def;
            }
        }
    }
}