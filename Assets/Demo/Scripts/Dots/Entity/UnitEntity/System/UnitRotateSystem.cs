using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitRotateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitAttributeComponent>();
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<UnitStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new UnitRotateJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct UnitRotateJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform, 
            in UnitStateComponent stateComp, 
            in UnitAttributeComponent attributeComp,
            in UnitInputComponent input)
        {
            if (!StateUtility.CanRotate(stateComp)) return;
            
            var movement = input.Movement;
            if (input.InputX != 0)
            {
                quaternion targetRotation = quaternion.LookRotation(movement, localTransform.Up());
                localTransform.Rotation = math.slerp(localTransform.Rotation,targetRotation, attributeComp.RotateSpeed  * deltaTime);
            }
        }
    }
}