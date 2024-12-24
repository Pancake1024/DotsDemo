using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class PlayerJumpAuthoring : MonoBehaviour 
    {
        public class Baker : Baker<PlayerJumpAuthoring>
        {
            public override void Bake(PlayerJumpAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerJumpComponent()
                {
                    JumpHeight = 5f,
                    Gravity = 9.8f,
                    JumpEndDuration = 0.3f
                });
            }
        }
    }

    public struct PlayerJumpComponent : IComponentData
    {
        public float JumpHeight;
        public float Gravity;
        
        public float JumpSpeed;
        public float3 StartMovement;
        public float JumpEndDuration;
        public float Time;
    }
}