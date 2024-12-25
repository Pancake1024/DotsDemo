using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public static class StateUtility
    {
        /// <summary>
        /// 状态可以转换的状态字典
        /// </summary>
        private static Dictionary<UnitState, UnitState[]> _State2StatesDict =
            new Dictionary<UnitState, UnitState[]>()
            {
                { UnitState.Idle ,new [] { UnitState.Move ,UnitState.Jumping,UnitState.BeHit,UnitState.CastSkill,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.Move ,new [] { UnitState.Idle ,UnitState.Jumping,UnitState.BeHit,UnitState.CastSkill,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.Jumping ,new [] { UnitState.Jump_End,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.Jump_End ,new [] { UnitState.Idle ,UnitState.Move,UnitState.Jumping,UnitState.BeHit,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.CastSkill ,new [] { UnitState.Idle,UnitState.CastSkillEnd,UnitState.Move,UnitState.Jumping,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.CastSkillEnd ,new [] { UnitState.Idle, UnitState.Move,UnitState.CastSkill,UnitState.Stunned,UnitState.BeHit,UnitState.Invincible,UnitState.Die}},
                { UnitState.BeHit ,new [] { UnitState.Idle,UnitState.Move,UnitState.Jumping,UnitState.Stunned,UnitState.Invincible,UnitState.Die}},
                { UnitState.Stunned, new UnitState[] {UnitState.Die}},
                { UnitState.Invincible, new [] { UnitState.Idle, UnitState.Move, UnitState.Jumping, UnitState.Jump_End,UnitState.BeHit ,UnitState.CastSkill,UnitState.CastSkillEnd,UnitState.Die}},
                { UnitState.Die, new UnitState[] {}},
            };
        
        /// <summary>
        /// 互斥状态字典
        /// </summary>
        private static Dictionary<UnitState, UnitState[]> _State2MutuallyExclusiveDict =
            new Dictionary<UnitState, UnitState[]>()
            {
                { UnitState.Idle ,new [] { UnitState.Move , UnitState.Jumping, UnitState.Jump_End, UnitState.BeHit,UnitState.CastSkill,UnitState.Die}},
                { UnitState.Move ,new [] { UnitState.Idle , UnitState.Jumping, UnitState.Jump_End, UnitState.BeHit,UnitState.CastSkill,UnitState.Die}},
                { UnitState.Jumping ,new [] { UnitState.Idle ,UnitState.Move,UnitState.Jump_End,UnitState.BeHit,UnitState.Die}},
                { UnitState.Jump_End ,new [] { UnitState.Idle ,UnitState.Move,UnitState.Jumping,UnitState.BeHit,UnitState.Die}},
                { UnitState.CastSkill, new [] { UnitState.Idle ,UnitState.Move, UnitState.Jumping, UnitState.Jump_End, UnitState.BeHit,UnitState.Die}},
                { UnitState.CastSkillEnd, new [] { UnitState.Idle ,UnitState.Move,UnitState.Move,UnitState.Jumping, UnitState.Jump_End,UnitState.CastSkill,UnitState.Die}},
                { UnitState.BeHit, new [] { UnitState.Idle ,UnitState.Move,UnitState.Jumping, UnitState.Jump_End,UnitState.Die}},
                { UnitState.Stunned, new [] { UnitState.Idle ,UnitState.Move,UnitState.Jumping, UnitState.BeHit,UnitState.Die}},
                { UnitState.Invincible, new UnitState[] { }},
                { UnitState.Die, new [] { UnitState.Idle, UnitState.Move, UnitState.Jumping, UnitState.Jump_End, UnitState.BeHit, UnitState.CastSkill, UnitState.CastSkillEnd, UnitState.Invincible, UnitState.Stunned}},
            };
        
        public static bool TryAddState(ref UnitStateComponent stateComp, UnitState newState)
        {
            if ((stateComp.State & newState) != 0) return false;

            //遍历所有单个状态
            foreach (UnitState singleState in Enum.GetValues(typeof(UnitState)))
            {
                //当前状态包含了单个状态
                if ((stateComp.State & singleState) != 0)
                {
                    //判断指定单个状态是否可以转换到新状态
                    if (_State2StatesDict.TryGetValue(singleState, out var states))
                    {
                        bool canChangeState = false;
                        foreach (var state in states)
                        {
                            //如果新增的状态，在singleState的可转换数组中
                            if ((newState & state) != 0)
                            {
                                canChangeState = true;
                                break;
                            }
                        }
                        
                        if (canChangeState)
                        {
                            var curState = stateComp.State;
                            Debug.Log($"old curState：{curState}");
                            //判断是否有互斥状态
                            if (_State2MutuallyExclusiveDict.TryGetValue(singleState, out var mutuallyExclusiveStates))
                            {
                                foreach (var state in mutuallyExclusiveStates)
                                {
                                    //如果新增的状态，与互斥状态有交集
                                    if ((newState & state) != 0)
                                    {
                                        //清除互斥状态
                                        curState &= ~singleState;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError($"Can't find mutually exclusive states for {curState} in config");
                            }
                            
                            //添加新状态
                            curState |= newState;
                            stateComp.State = curState;
                            Debug.Log($"new curState：{curState}");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool CanMove(UnitStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & UnitState.Idle) != 0 ||
                   (state & UnitState.Move) != 0 ||
                   (state & UnitState.Jump_End) != 0 ||
                   (state & UnitState.CastSkillEnd) != 0
                   ;
        }

        public static bool CanRotate(UnitStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & UnitState.Idle) != 0 ||
                   (state & UnitState.Move) != 0 ||
                   (state & UnitState.Jump_End) != 0 ||
                   (state & UnitState.CastSkillEnd) != 0
                ;
        }

        public static bool CanJump(UnitStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & UnitState.Idle) != 0 ||
                   (state & UnitState.Move) != 0 ||
                   (state & UnitState.Jump_End) != 0 ||
                   (state & UnitState.CastSkillEnd) != 0
                ;
        }

        public static bool CanCastActiveSkill(UnitStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & UnitState.Idle) != 0 ||
                   (state & UnitState.Move) != 0 ||
                   (state & UnitState.Jump_End) != 0 ||
                   (state & UnitState.CastSkillEnd) != 0
                ;
        }

        public static bool CanCastPassiveSkill(UnitStateComponent stateComp)
        {
            var state = stateComp.State;

            return true;
        }
    }
}