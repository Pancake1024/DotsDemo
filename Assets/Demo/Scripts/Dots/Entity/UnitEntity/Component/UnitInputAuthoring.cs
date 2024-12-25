using Unity.Entities;
using Unity.Mathematics;

namespace Pancake.ECSDemo
{
    public class UnitInputAuthoring : UnityEngine.MonoBehaviour
    {
        public class Baker : Baker<UnitInputAuthoring>
        {
            public override void Bake(UnitInputAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new UnitInputComponent()
                {
                    UsedSkillSlot = -1
                });
            }
        }       
    }
    
    public struct UnitInputComponent : IComponentData
    {
        public float InputX;
        public float InputY;
        public float3 Movement;
        public bool IsJump;
        public int UsedSkillSlot;
    }
}