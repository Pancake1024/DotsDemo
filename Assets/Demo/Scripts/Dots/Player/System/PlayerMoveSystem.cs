using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pancake.ECSDemo
{
    public partial class PlayerMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerMoveComponent>();
            RequireForUpdate<PlayerStateComponent>();
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<PlayerOnGroundComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerMoveJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct PlayerMoveJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform,
            ref PlayerStateComponent stateComp,
            in PlayerMoveComponent moveComp,
            in PlayerInputComponent input, 
            in PlayerOnGroundComponent onGroundComp)
        {
            var state = stateComp.State;
            if (!onGroundComp.IsOnGround || 
                !(state == PlayerState.Idle || state == PlayerState.Move || state == PlayerState.Jump_End))
            {
                return;
            }

            float3 translation = moveComp.MoveSpeed * input.Movement * deltaTime;
            localTransform = localTransform.Translate(translation);
            bool isMoving = math.lengthsq(translation) > 0;
            stateComp.State = isMoving ? PlayerState.Move : PlayerState.Idle;
        }
    }
}