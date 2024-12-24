using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class SpawnPlayerAuthoring : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public GameObject PlayerAnimatedGO;
        
        public class Baker : Baker<SpawnPlayerAuthoring>
        {
            public override void Bake(SpawnPlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SpawnPlayerConfig()
                {
                    PlayerPrefab = GetEntity(authoring.PlayerPrefab,TransformUsageFlags.Dynamic),
                    Position = new float3(0,0,1),
                    Rotation = quaternion.identity,
                    Scale = 1f,
                });
                var playerCfManaged = new PlayerConfigManaged();
                playerCfManaged.AnimatedPrefabGO = authoring.PlayerAnimatedGO;
                AddComponentObject(entity, playerCfManaged);
            }
        }
    }
    
    public struct SpawnPlayerConfig : IComponentData
    {
        public Entity PlayerPrefab;
        public float3 Position;
        public quaternion Rotation;
        public float Scale;
    }
}