using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class PlayerStateAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerStateAuthoring>
        {
            public override void Bake(PlayerStateAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerStateComponent()
                {
                    State = PlayerState.Idle
                });
            }
        }
    }

    public struct PlayerStateComponent : IComponentData
    {
        public PlayerState State;
    }
}