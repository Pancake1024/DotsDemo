using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class SpawnUnitSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<SpawnUnitConfig>();
        }

        protected override void OnUpdate()
        {
            this.Enabled = false;

            EntityCommandBuffer ecb = new EntityCommandBuffer(WorldUpdateAllocator);
            
            Entities.WithoutBurst().ForEach((in SpawnUnitConfig spawnUnitCfg, in UnitConfigManaged unitConfigManaged) =>
            {
                Entity entity = ecb.Instantiate(spawnUnitCfg.PlayerPrefab);
                //初始化位置
                ecb.SetComponent(entity, new LocalTransform()
                {
                    Position = spawnUnitCfg.Position,
                    Rotation = spawnUnitCfg.Rotation,
                    Scale = spawnUnitCfg.Scale,
                });
            
                //初始化属性
                ecb.SetComponent(entity, new UnitAttributeComponent()
                {
                    Health = spawnUnitCfg.Health,
                    UnitType = spawnUnitCfg.UnitType,
                    MoveSpeed = spawnUnitCfg.MoveSpeed,
                    RotateSpeed = spawnUnitCfg.RotateSpeed,
                });
            
                //桥接GameObject
                var playerAnimatedComp = new UnitGameObjectComponent();
                var go = UnityEngine.Object.Instantiate(unitConfigManaged.UnitPrefabGO);
                playerAnimatedComp.GO = go;
                playerAnimatedComp.Animator = go.GetComponentInChildren<Animator>();
                ecb.AddComponent(entity, playerAnimatedComp);
                ecb.RemoveComponent<MaterialMeshInfo>(entity);
                
                //初始化技能
                ecb.AddBuffer<UnitSkillIdElement>(entity);
                var buffer = ecb.SetBuffer<UnitSkillIdElement>(entity);
            
                buffer.Add(new UnitSkillIdElement() { Slot = 0,ID = 101 });
                buffer.Add(new UnitSkillIdElement() { Slot = 1,ID = 102 });
                buffer.Add(new UnitSkillIdElement() { Slot = 2,ID = 103 });
                buffer.Add(new UnitSkillIdElement() { Slot = 3,ID = 104 });
                buffer.Add(new UnitSkillIdElement() { Slot = 4,ID = 105 });
            }).Run();
            
            ecb.Playback(EntityManager);
        }
    }
}