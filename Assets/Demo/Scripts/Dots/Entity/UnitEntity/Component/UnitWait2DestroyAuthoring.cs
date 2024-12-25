using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitWait2DestroyAuthoring : MonoBehaviour
    {
        public class  Baker : Baker<UnitWait2DestroyAuthoring>
        {
            public override void Bake(UnitWait2DestroyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitWait2DestroyData());
            }
        }
    }

    public struct UnitWait2DestroyData : IComponentData
    {
        public bool IsWait;
        public float Time;
    }
}