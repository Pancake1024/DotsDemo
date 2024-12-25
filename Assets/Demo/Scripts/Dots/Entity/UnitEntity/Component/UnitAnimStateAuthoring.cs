using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitAnimStateAuthoring : MonoBehaviour
    {
        public class Baker : Baker<UnitAnimStateAuthoring>
        {
            public override void Bake(UnitAnimStateAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new UnitAnimStateComponent() { AnimState = UnitAnimState.Idle });
            }
        }
    }

    public struct UnitAnimStateComponent : IComponentData
    {
        public UnitAnimState AnimState;
    }
}