using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitAuto2IdleAuthoring : MonoBehaviour
    {
        public class Baker : Baker<UnitAuto2IdleAuthoring>
        {
            public override void Bake(UnitAuto2IdleAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitAuto2IdleAnimData());
            }
        }
    }
    
    public struct UnitAuto2IdleAnimData : IComponentData
    {
        public float Time;
    }
}