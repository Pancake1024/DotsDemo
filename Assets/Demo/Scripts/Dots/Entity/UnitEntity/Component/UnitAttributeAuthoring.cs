using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitAttributeAuthoring : MonoBehaviour
    {
        public class Baker : Baker<UnitAttributeAuthoring>
        {
            public override void Bake(UnitAttributeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitAttributeComponent());
            }
        }
    }

    public struct UnitAttributeComponent : IComponentData
    {
        public int Health;
        public UnitType UnitType;
        public float MoveSpeed;
        public float RotateSpeed;
    }
}