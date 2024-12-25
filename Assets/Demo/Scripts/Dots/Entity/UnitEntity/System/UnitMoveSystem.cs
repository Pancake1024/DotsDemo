using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitAttributeComponent>();
            RequireForUpdate<UnitStateComponent>();
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<UnitOnGroundComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new UnitMoveJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct UnitMoveJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref LocalTransform localTransform,
            ref UnitStateComponent stateComp,
            in UnitInputComponent input, 
            in UnitAttributeComponent attributeComp,
            in UnitAnimStateComponent animStateComp,
            in UnitOnGroundComponent onGroundComp)
        {
            // if (attributeComp.UnitType == UnitType.Monster)
            // {
            //     Debug.LogError(stateComp.State + " " + animStateComp.AnimState);
            // }
            
            if (!onGroundComp.IsOnGround || 
                !StateUtility.CanMove(stateComp))
            {
                return;
            }

            float3 translation = attributeComp.MoveSpeed * input.Movement * deltaTime;
            localTransform = localTransform.Translate(translation);
            bool isMoving = math.lengthsq(translation) > 0;
            var state = isMoving ? UnitState.Move : UnitState.Idle;
            StateUtility.TryAddState(ref stateComp, state);
        }
    }
}