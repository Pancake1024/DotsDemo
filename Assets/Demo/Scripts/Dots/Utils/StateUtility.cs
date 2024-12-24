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
        private static Dictionary<PlayerState, PlayerState[]> _State2StatesDict =
            new Dictionary<PlayerState, PlayerState[]>()
            {
                { PlayerState.Idle ,new [] { PlayerState.Move ,PlayerState.Jumping,PlayerState.BeHit,PlayerState.CastSkill,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.Move ,new [] { PlayerState.Idle ,PlayerState.Jumping,PlayerState.BeHit,PlayerState.CastSkill,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.Jumping ,new [] { PlayerState.Jump_End,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.Jump_End ,new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Jumping,PlayerState.BeHit,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.CastSkill ,new [] { PlayerState.Idle,PlayerState.CastSkillEnd,PlayerState.Move,PlayerState.Jumping,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.CastSkillEnd ,new [] { PlayerState.Idle, PlayerState.Move,PlayerState.CastSkill,PlayerState.Stunned,PlayerState.BeHit,PlayerState.Invincible}},
                { PlayerState.BeHit ,new [] { PlayerState.Idle,PlayerState.Move,PlayerState.Jumping,PlayerState.Stunned,PlayerState.Invincible}},
                { PlayerState.Stunned, new PlayerState[] {}},
                { PlayerState.Invincible, new [] { PlayerState.Idle, PlayerState.Move, PlayerState.Jumping, PlayerState.Jump_End,PlayerState.BeHit ,PlayerState.CastSkill,PlayerState.CastSkillEnd}},
            };
        
        /// <summary>
        /// 互斥状态字典
        /// </summary>
        private static Dictionary<PlayerState, PlayerState[]> _State2MutuallyExclusiveDict =
            new Dictionary<PlayerState, PlayerState[]>()
            {
                { PlayerState.Idle ,new [] { PlayerState.Move , PlayerState.Jumping, PlayerState.Jump_End, PlayerState.BeHit,PlayerState.CastSkill}},
                { PlayerState.Move ,new [] { PlayerState.Idle , PlayerState.Jumping, PlayerState.Jump_End, PlayerState.BeHit,PlayerState.CastSkill}},
                { PlayerState.Jumping ,new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Jump_End,PlayerState.BeHit}},
                { PlayerState.Jump_End ,new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Jumping,PlayerState.BeHit}},
                { PlayerState.BeHit, new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Jumping, PlayerState.Jump_End}},
                { PlayerState.Stunned, new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Jumping, PlayerState.BeHit}},
                { PlayerState.Invincible, new PlayerState[] { }},
                { PlayerState.CastSkill, new [] { PlayerState.Idle ,PlayerState.Move, PlayerState.Jumping, PlayerState.Jump_End, PlayerState.BeHit}},
                { PlayerState.CastSkillEnd, new [] { PlayerState.Idle ,PlayerState.Move,PlayerState.Move,PlayerState.Jumping, PlayerState.Jump_End,PlayerState.CastSkill}},
            };
        
        public static bool TryAddState(ref PlayerStateComponent stateComp, PlayerState newState)
        {
            if ((stateComp.State & newState) != 0) return false;

            //遍历所有单个状态
            foreach (PlayerState singleState in Enum.GetValues(typeof(PlayerState)))
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

        public static bool CanMove(PlayerStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & PlayerState.Idle) != 0 ||
                   (state & PlayerState.Move) != 0 ||
                   (state & PlayerState.Jump_End) != 0 ||
                   (state & PlayerState.CastSkillEnd) != 0
                   ;
        }

        public static bool CanRotate(PlayerStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & PlayerState.Idle) != 0 ||
                   (state & PlayerState.Move) != 0 ||
                   (state & PlayerState.Jump_End) != 0 ||
                   (state & PlayerState.CastSkillEnd) != 0
                ;
        }

        public static bool CanJump(PlayerStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & PlayerState.Idle) != 0 ||
                   (state & PlayerState.Move) != 0 ||
                   (state & PlayerState.Jump_End) != 0 ||
                   (state & PlayerState.CastSkillEnd) != 0
                ;
        }

        public static bool CanCastActiveSkill(PlayerStateComponent stateComp)
        {
            var state = stateComp.State;
            return (state & PlayerState.Idle) != 0 ||
                   (state & PlayerState.Move) != 0 ||
                   (state & PlayerState.Jump_End) != 0 ||
                   (state & PlayerState.CastSkillEnd) != 0
                ;
        }

        public static bool CanCastPassiveSkill(PlayerStateComponent stateComp)
        {
            var state = stateComp.State;

            return true;
        }
    }
}