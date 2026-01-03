using UnityEngine;

namespace Items.Scripts
{
    [CreateAssetMenu(menuName = "Game/Items/Wearable", fileName = "NewWearable")]
    public class WearableDefinition : ItemDefinition
    {
        [Header("Wearable")]
        [SerializeField] private EquipSlot equipSlot;

        public EquipSlot EquipSlot => equipSlot;
        
        public static Builder NewBuilder() => new Builder();
        
        public sealed class Builder
            : ItemDefinition.Builder<WearableDefinition, Builder>
        {
            private EquipSlot _equipSlot;

            protected override Builder Self() => this;
            protected override WearableDefinition Create()
                => ScriptableObject.CreateInstance<WearableDefinition>();

            public Builder EquipSlot(EquipSlot v) { _equipSlot = v; return this; }

            public override WearableDefinition Build()
            {
                var def = base.Build();
                def.equipSlot = _equipSlot;
                return def;
            }
        }
    }
}