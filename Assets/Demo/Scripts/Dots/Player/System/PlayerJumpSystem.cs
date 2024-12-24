using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerJumpSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<PlayerJumpComponent>();
            RequireForUpdate<PlayerOnGroundComponent>();
            RequireForUpdate<PlayerStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerJumpJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct PlayerJumpJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform, 
            ref PlayerJumpComponent jumpComp, 
            ref PlayerStateComponent stateComp,
            in PlayerInputComponent input,
            in PlayerOnGroundComponent onGroundComp,
            in PlayerMoveComponent moveComp)
        {
            var state = stateComp.State;
            //玩家按下跳跃键，且在地面上，且没有在跳跃
            if (input.IsJump && onGroundComp.IsOnGround && StateUtility.CanJump(stateComp))
            {
                StateUtility.TryAddState(ref stateComp, PlayerState.Jumping);
                // stateComp.State = PlayerState.Jumping;
                jumpComp.JumpSpeed = math.sqrt(2 * jumpComp.JumpHeight * jumpComp.Gravity);
                jumpComp.StartMovement = input.Movement;
            }
            
            if (stateComp.State == PlayerState.Jumping)
            {
                var translation =
                    (jumpComp.StartMovement * moveComp.MoveSpeed + jumpComp.JumpSpeed * math.up()) *
                    deltaTime;
                localTransform.Position += translation;
                jumpComp.JumpSpeed -= jumpComp.Gravity * deltaTime;
                    
                if (jumpComp.JumpSpeed < 0)
                {
                    if (onGroundComp.IsOnGround)
                    {
                        jumpComp.Time = 0;
                        StateUtility.TryAddState(ref stateComp, PlayerState.Jump_End);
                    }
                }
            }

            if (stateComp.State == PlayerState.Jump_End)
            {
                jumpComp.Time += deltaTime;
                if (jumpComp.Time >= jumpComp.JumpEndDuration)
                {
                    StateUtility.TryAddState(ref stateComp, PlayerState.Idle);
                }
            }
        }
    }
}