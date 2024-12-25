using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitStateAuthoring : MonoBehaviour
    {
        public class Baker : Baker<UnitStateAuthoring>
        {
            public override void Bake(UnitStateAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitStateComponent()
                {
                    State = UnitState.Idle
                });
            }
        }
    }

    public struct UnitStateComponent : IComponentData
    {
        public UnitState State;
    }
}