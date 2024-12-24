using Unity.Entities;
using UnityEngine;
using PFF.Common;
using Unity.Mathematics;

namespace Pancake.ECSDemo
{
    public partial class PlayerEditorOrPCInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<CameraComponent>();
        }

        protected override void OnUpdate()
        {
            var cameraEntity = SystemAPI.GetSingletonEntity<FollowCameraTag>();
            var cameraComponent = EntityManager.GetComponentData<CameraComponent>(cameraEntity);
            
            if (Utils.GetPlatformType() == PlatfromType.Editor || Utils.GetPlatformType() == PlatfromType.PC)
            {
                Entities.ForEach((ref PlayerInputComponent input) =>
                {
                    // Movement
                    var inputX = Input.GetAxis("Horizontal");
                    var inputY = Input.GetAxis("Vertical");
                    var movement = new float3(inputX, 0, inputY);
                    var forward = cameraComponent.Forward;
                    var right = cameraComponent.Right;
                    forward.y = 0;
                    right.y = 0;
                    movement = forward * movement.z + right * movement.x;
                    input.Movement = math.lengthsq(movement) > 0 ? math.normalize(movement) : float3.zero;
                    
                    // Jump
                    input.IsJump = Input.GetButtonDown("Jump");
                    
                    // Skill
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        input.UsedSkillSlot = 0;
                    }else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        input.UsedSkillSlot = 1;
                    }else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        input.UsedSkillSlot = 2;
                    }else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        input.UsedSkillSlot = 3;
                    }else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        input.UsedSkillSlot = 4;
                    }
                    else
                    {
                        input.UsedSkillSlot = -1;
                    }
                }).Run();
            }
        }
    }
}