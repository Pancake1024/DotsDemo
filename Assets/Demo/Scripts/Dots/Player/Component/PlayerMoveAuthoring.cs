using Unity.Entities;
using Unity.Mathematics;

namespace Pancake.ECSDemo
{
    public class PlayerMoveAuthoring : UnityEngine.MonoBehaviour
    {
        public class Baker : Baker<PlayerMoveAuthoring>
        {
            public override void Bake(PlayerMoveAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerMoveComponent(){MoveSpeed = 2});
            }
        }       
    }
    
    public struct PlayerMoveComponent : IComponentData
    {
        public float MoveSpeed;
    }
}