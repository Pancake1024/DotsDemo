using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class UnitJumpAuthoring : MonoBehaviour 
    {
        public class Baker : Baker<UnitJumpAuthoring>
        {
            public override void Bake(UnitJumpAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new UnitJumpComponent()
                {
                    JumpHeight = 5f,
                    Gravity = 9.8f,
                    JumpEndDuration = 0.3f
                });
            }
        }
    }

    public struct UnitJumpComponent : IComponentData
    {
        public float JumpHeight;
        public float Gravity;
        
        public float JumpSpeed;
        public float3 StartMovement;
        public float JumpEndDuration;
        public float Time;
    }
}