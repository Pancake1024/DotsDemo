using Unity.Entities;
using Unity.Mathematics;

namespace Pancake.ECSDemo
{
    public class PlayerInputAuthoring : UnityEngine.MonoBehaviour
    {
        public class Baker : Baker<PlayerInputAuthoring>
        {
            public override void Bake(PlayerInputAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerInputComponent()
                {
                    UsedSkillSlot = -1
                });
            }
        }       
    }
    
    public struct PlayerInputComponent : IComponentData
    {
        public float3 Movement;
        public bool IsJump;
        public int UsedSkillSlot;
    }
}