using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitJumpSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<UnitJumpComponent>();
            RequireForUpdate<UnitOnGroundComponent>();
            RequireForUpdate<UnitStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new UnitJumpJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel(Dependency).Complete();
        }
    }

    [BurstCompile]
    public partial struct UnitJumpJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform, 
            ref UnitJumpComponent jumpComp, 
            ref UnitStateComponent stateComp,
            in UnitInputComponent input,
            in UnitOnGroundComponent onGroundComp,
            in UnitAttributeComponent attributeComp)
        {
            var state = stateComp.State;
            //玩家按下跳跃键，且在地面上，且没有在跳跃
            if (input.IsJump && onGroundComp.IsOnGround && StateUtility.CanJump(stateComp))
            {
                StateUtility.TryAddState(ref stateComp, UnitState.Jumping);
                // stateComp.State = PlayerState.Jumping;
                jumpComp.JumpSpeed = math.sqrt(2 * jumpComp.JumpHeight * jumpComp.Gravity);
                jumpComp.StartMovement = input.Movement;
            }
            
            if (stateComp.State == UnitState.Jumping)
            {
                var translation =
                    (jumpComp.StartMovement * attributeComp.MoveSpeed + jumpComp.JumpSpeed * math.up()) *
                    deltaTime;
                localTransform.Position += translation;
                jumpComp.JumpSpeed -= jumpComp.Gravity * deltaTime;
                    
                if (jumpComp.JumpSpeed < 0)
                {
                    if (onGroundComp.IsOnGround)
                    {
                        jumpComp.Time = 0;
                        StateUtility.TryAddState(ref stateComp, UnitState.Jump_End);
                    }
                }
            }

            if (stateComp.State == UnitState.Jump_End)
            {
                jumpComp.Time += deltaTime;
                if (jumpComp.Time >= jumpComp.JumpEndDuration)
                {
                    StateUtility.TryAddState(ref stateComp, UnitState.Idle);
                }
            }
        }
    }
}