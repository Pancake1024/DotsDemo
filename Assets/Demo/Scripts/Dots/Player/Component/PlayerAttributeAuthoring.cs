using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class PlayerAttributeAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAttributeAuthoring>
        {
            public override void Bake(PlayerAttributeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerAttributeComponent()
                {
                    Health = 100,
                });
            }
        }
    }

    public struct PlayerAttributeComponent : IComponentData
    {
        public int Health;
    }
}