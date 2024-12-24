using Unity.Entities;

namespace Pancake.ECSDemo
{
    public class PlayerRotateAuthoring : UnityEngine.MonoBehaviour
    {
        public class Baker : Baker<PlayerRotateAuthoring>
        {
            public override void Bake(PlayerRotateAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerRotateComponent(){RotateSpeed = 4});
            }
        }       
    }
    
    public struct PlayerRotateComponent : IComponentData
    {
        public float RotateSpeed;
    }
}