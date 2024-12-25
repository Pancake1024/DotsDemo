using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitOnGroundAuthoring : MonoBehaviour
    {
        public class Baker : Baker<UnitOnGroundAuthoring>
        {
            public override void Bake(UnitOnGroundAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitOnGroundComponent());
            }
        }
    }
    
    public struct UnitOnGroundComponent : IComponentData
    {
        public bool IsOnGround;
    }
}