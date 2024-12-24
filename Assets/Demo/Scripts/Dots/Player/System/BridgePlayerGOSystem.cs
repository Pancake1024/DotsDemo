using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class BridgePlayerGOSystem : SystemBase
    {
        private bool _IsInit;
        
        protected override void OnCreate()
        {
            RequireForUpdate<SpawnPlayerConfig>();
            RequireForUpdate<PlayerInputComponent>();
        }

        protected override void OnUpdate()
        {
            if (!_IsInit)
            {
                _IsInit = true;
            
                var configEntity = SystemAPI.GetSingletonEntity<SpawnPlayerConfig>();
                var playerConfigManaged = EntityManager.GetComponentObject<PlayerConfigManaged>(configEntity);
                var ecb = new EntityCommandBuffer(WorldUpdateAllocator);
            
                foreach (var (localTransform,entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerInputComponent>().WithEntityAccess())
                {
                    var playerAnimatedComp = new PlayerGameObjectComponent();
                    var go = GameObject.Instantiate(playerConfigManaged.AnimatedPrefabGO);
                    playerAnimatedComp.AnimatedGO = go;
                    playerAnimatedComp.Animator = go.GetComponentInChildren<Animator>();
                    ecb.AddComponent(entity, playerAnimatedComp);
            
                    ecb.RemoveComponent<MaterialMeshInfo>(entity);
                }
                
                ecb.Playback(EntityManager);
            }

            Entities.WithoutBurst().ForEach((in PlayerGameObjectComponent objComp,in LocalTransform localTransform) =>
            {
                objComp.AnimatedGO.transform.localPosition = (Vector3)localTransform.Position;
                objComp.AnimatedGO.transform.rotation = localTransform.Rotation;
            }).Run();
        }
    }
}