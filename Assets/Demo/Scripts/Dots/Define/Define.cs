namespace Pancake.ECSDemo
{
    public enum UnitAnimState
    {
        None,
        Idle,
        Move,
        Jumping,
        Jump_End,
        Attack,
        BeHit,
        Die,
    }

    public enum UnitState
    {       
        None = 0,
        Idle = 1 << 0,
        Move = 1 << 1 ,
        Jumping = 1 << 2,
        Jump_End = 1 << 3,
        BeHit = 1 << 4,
        CastSkill = 1 << 5,
        CastSkillEnd = 1 << 6,
        Stunned = 1 << 7,
        Invincible = 1 << 8,
        Die = 1 << 9,
    }
    
    public enum UnitType
    {
        None,
        Player,
        Monster,
    }
}