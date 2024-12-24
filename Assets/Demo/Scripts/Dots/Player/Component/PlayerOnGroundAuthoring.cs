using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class PlayerOnGroundAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerOnGroundAuthoring>
        {
            public override void Bake(PlayerOnGroundAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerOnGroundComponent());
            }
        }
    }
    
    public struct PlayerOnGroundComponent : IComponentData
    {
        public bool IsOnGround;
    }
}