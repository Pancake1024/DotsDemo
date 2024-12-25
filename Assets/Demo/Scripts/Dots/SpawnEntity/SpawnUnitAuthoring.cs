using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class SpawnUnitAuthoring : MonoBehaviour
    {
        public GameObject UnitPrefab;
        public GameObject UnitAnimatedGO;
        public UnitType UnitType;
        public int Health;
        public Vector3 BornPosition;
        public Vector3 BornRotation;
        public float MoveSpeed = 4;
        public float RotateSpeed = 4;
        
        public class Baker : Baker<SpawnUnitAuthoring>
        {
            public override void Bake(SpawnUnitAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SpawnUnitConfig()
                {
                    PlayerPrefab = GetEntity(authoring.UnitPrefab,TransformUsageFlags.Dynamic),
                    Position = authoring.BornPosition,
                    Rotation = quaternion.Euler(authoring.BornRotation * Mathf.Deg2Rad),
                    Scale = 1f,
                    UnitType = authoring.UnitType,
                    Health = authoring.Health,
                    RotateSpeed = authoring.RotateSpeed,
                    MoveSpeed = authoring.MoveSpeed,
                });
                var playerCfManaged = new UnitConfigManaged();
                playerCfManaged.UnitPrefabGO = authoring.UnitAnimatedGO;
                AddComponentObject(entity, playerCfManaged);
            }
        }
    }
    
    public struct SpawnUnitConfig : IComponentData
    {
        public Entity PlayerPrefab;
        public float3 Position;
        public quaternion Rotation;
        public float Scale;
        public UnitType UnitType;
        public int Health;
        public float MoveSpeed;
        public float RotateSpeed;
    }
}