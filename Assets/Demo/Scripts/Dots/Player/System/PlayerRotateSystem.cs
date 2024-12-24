using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerRotateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerRotateComponent>();
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<PlayerStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerRotateJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct PlayerRotateJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform, 
            in PlayerRotateComponent rotateComp, 
            in PlayerStateComponent stateComp, 
            in PlayerInputComponent input)
        {
            if (!StateUtility.CanRotate(stateComp)) return;
            
            var movement = input.Movement;
            if (input.InputX != 0)
            {
                quaternion targetRotation = quaternion.LookRotation(movement, localTransform.Up());
                localTransform.Rotation = math.slerp(localTransform.Rotation,targetRotation, rotateComp.RotateSpeed  * deltaTime);
            }
        }
    }
}