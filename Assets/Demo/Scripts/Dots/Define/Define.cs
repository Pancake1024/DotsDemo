namespace Pancake.ECSDemo
{
    public enum PlayerAnimState
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

    public enum PlayerState
    {        
        Idle,
        Move,
        Jumping,
        Jump_End,
        BeHit,
        CastSkill,
        CastSkillEnd,
    }
}