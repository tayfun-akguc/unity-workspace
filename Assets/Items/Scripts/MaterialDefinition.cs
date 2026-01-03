using UnityEngine;

namespace Items.Scripts
{
    [CreateAssetMenu(menuName = "Game/Items/Material", fileName = "NewMaterial")]
    public class MaterialDefinition : ItemDefinition
    {
        // Keep empty for now.
        // Later you can add crafting tags, tier, refinement info, etc.

        public static Builder NewBuilder() => new Builder();

        public sealed class Builder : Builder<MaterialDefinition, Builder>
        {
            protected override Builder Self() => this;

            protected override MaterialDefinition Create()
                => ScriptableObject.CreateInstance<MaterialDefinition>();

            public override MaterialDefinition Build()
            {
                var def = base.Build();
                return def;
            }
        }
    }
}