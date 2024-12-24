using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerAnimStateAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAnimStateAuthoring>
        {
            public override void Bake(PlayerAnimStateAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerAnimStateComponent() { AnimState = PlayerAnimState.Idle });
            }
        }
    }

    public struct PlayerAnimStateComponent : IComponentData
    {
        public PlayerAnimState AnimState;
    }
}