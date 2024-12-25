using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{

    public class UnitGameObjectComponent : IComponentData
    {
        public GameObject GO;
        public Animator Animator;
        public UnitAnimState AnimState = UnitAnimState.None;
    }

    public class UnitConfigManaged : IComponentData
    {
        public GameObject UnitPrefabGO;
    }
    
    public struct UnitSkillIdElement : IBufferElementData
    {
        public int Slot;
        public int ID;
    }
    
    #region Skill Event
    
    public struct DamageEvent : IBufferElementData
    {
        public int Damage;
    }
    
    public struct AnimStateChangeEvent : IBufferElementData
    {
        public UnitAnimState AnimState;
    }
    
    public struct CasterActFinishEvent : IBufferElementData
    {
    }
    
    #endregion
}