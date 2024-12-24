using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class AnimatorSyncSystem : SystemBase
    {
        private PlayerAnimState _CurAnimState = PlayerAnimState.None;

        private Dictionary<PlayerAnimState, string> _State2TriggerName =
            new Dictionary<PlayerAnimState, string>()
            {
                { PlayerAnimState.Idle , "trigger_idle"},
                { PlayerAnimState.Move , "trigger_run"},
                { PlayerAnimState.Jumping , "trigger_jump"},
                { PlayerAnimState.Jump_End , "trigger_jump_end"},
                { PlayerAnimState.Attack , "trigger_attack"},
                { PlayerAnimState.BeHit , "trigger_behit"},
                { PlayerAnimState.Die , "trigger_die"},
            };
        

        protected override void OnCreate()
        {
            RequireForUpdate<PlayerAnimStateComponent>();
            RequireForUpdate<PlayerGameObjectComponent>();
        }

        protected override void OnUpdate()
        {
            Entities.WithoutBurst().ForEach((ref PlayerAnimStateComponent stateComp, in PlayerGameObjectComponent objComp, in Entity entity) =>
            {
                var newState = stateComp.AnimState;
                if (_CurAnimState == newState) return;
                _CurAnimState = newState;
                var triggerName = _State2TriggerName[newState];
                objComp.Animator.SetTrigger(triggerName);
            }).Run();
        }
    }
}