using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Pancake.ECSDemo
{
    public partial class UnitOnGroundSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitOnGroundComponent>();
        }
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new UnitOnGroundJob();
            job.ScheduleParallel(Dependency).Complete();
        }
    }

    [BurstCompile]
    public partial struct UnitOnGroundJob : IJobEntity
    {
        public void Execute(ref LocalTransform localTransform, ref UnitOnGroundComponent onGroundComp)
        {
            var pos = localTransform.Position;
            onGroundComp.IsOnGround = pos.y <= 0;
            if (pos.y < 0)
            {
                pos.y = 0;
                localTransform.Position = pos;
            }
        }
    }
}