using Unity.Entities;

namespace Pancake.ECSDemo
{
    public partial class UnitWait2DestroySystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitWait2DestroyData>();
            RequireForUpdate<UnitGameObjectComponent>();
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(WorldUpdateAllocator);

            Entities.WithoutBurst().ForEach((ref UnitWait2DestroyData wait2DestroyComp,
                in UnitGameObjectComponent objComp,
                in Entity entity) =>
            {
                if (wait2DestroyComp.IsWait)
                {
                    wait2DestroyComp.Time -= SystemAPI.Time.DeltaTime;
                    if (wait2DestroyComp.Time <= 0)
                    {
                        wait2DestroyComp.IsWait = false;
                        ecb.DestroyEntity(entity);
                        UnityEngine.GameObject.Destroy(objComp.GO);
                    }
                }
            }).Run();

            ecb.Playback(EntityManager);
        }
    }
}