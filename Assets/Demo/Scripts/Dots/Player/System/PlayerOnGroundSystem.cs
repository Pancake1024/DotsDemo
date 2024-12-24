using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Pancake.ECSDemo
{
    public partial class PlayerOnGroundSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerOnGroundComponent>();
        }
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerOnGroundJob();
            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct PlayerOnGroundJob : IJobEntity
    {
        public void Execute(ref LocalTransform localTransform, ref PlayerOnGroundComponent onGroundComp)
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