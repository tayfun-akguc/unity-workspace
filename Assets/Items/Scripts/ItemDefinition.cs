using UnityEngine;

namespace Items.Scripts
{
    public abstract class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string id; // e.g. "ore_t1", "potion_heal_small"
        [SerializeField] private string displayName;
        
        [Header("Visuals")]
        [SerializeField] private Sprite icon;
        
        [Header("Rules")]
        [SerializeField] private ItemCategory category;
        
        [Min(1)]
        [SerializeField] private int maxStack = 1;
        
        public string Id => id;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public ItemCategory Category => category;
        public int MaxStack => maxStack;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(id)) id = id.Trim();
        }
#endif

        private void Initialize(
            string itemId,
            string itemDisplayName,
            Sprite itemIcon,
            ItemCategory itemCategory,
            int itemMaxStack
        )
        {
            id = itemId;
            displayName = itemDisplayName;
            icon = itemIcon;
            category = itemCategory;
            maxStack = itemMaxStack;
        }
        
        // Nested SuperBuilder
        public abstract class Builder<TDef, TBuilder>
            where TDef : ItemDefinition
            where TBuilder : Builder<TDef, TBuilder>
        {
            private string _id;
            private string _displayName;
            private Sprite _icon;
            private ItemCategory _category;
            private int _maxStack = 1;

            protected abstract TBuilder Self();
            protected abstract TDef Create();

            public TBuilder Id(string v) { _id = v; return Self(); }
            public TBuilder DisplayName(string v) { _displayName = v; return Self(); }
            public TBuilder Icon(Sprite v) { _icon = v; return Self(); }
            public TBuilder Category(ItemCategory v) { _category = v; return Self(); }
            public TBuilder MaxStack(int v) { _maxStack = v; return Self(); }

            public virtual TDef Build()
            {
                var def = Create();
                def.Initialize(_id, _displayName, _icon, _category, _maxStack);
                return def;
            }
        }
    }
    
   
}