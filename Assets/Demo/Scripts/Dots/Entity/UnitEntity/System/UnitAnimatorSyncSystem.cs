using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitAnimatorSyncSystem : SystemBase
    {
        private Dictionary<UnitAnimState, string> _State2TriggerName =
            new Dictionary<UnitAnimState, string>()
            {
                { UnitAnimState.Idle , "trigger_idle"},
                { UnitAnimState.Move , "trigger_run"},
                { UnitAnimState.Jumping , "trigger_jump"},
                { UnitAnimState.Jump_End , "trigger_jump_end"},
                { UnitAnimState.Attack , "trigger_attack"},
                { UnitAnimState.BeHit , "trigger_behit"},
                { UnitAnimState.Die , "trigger_die"},
            };
        

        protected override void OnCreate()
        {
            RequireForUpdate<UnitAnimStateComponent>();
            RequireForUpdate<UnitGameObjectComponent>();
        }

        protected override void OnUpdate()
        {
            Entities.WithoutBurst().ForEach((ref UnitAnimStateComponent stateComp, 
                in UnitGameObjectComponent objComp, 
                in Entity entity) =>
            {
                var newState = stateComp.AnimState;
                if (objComp.AnimState == newState) return;
                objComp.AnimState = newState;
                var triggerName = _State2TriggerName[newState];
                objComp.Animator.SetTrigger(triggerName);
            }).Run();
        }
    }
}