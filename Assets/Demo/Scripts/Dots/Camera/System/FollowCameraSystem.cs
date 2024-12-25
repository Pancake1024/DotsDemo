using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class FollowCameraSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<FollowCameraTag>();
            RequireForUpdate<PlayerFollowCameraManaged>();
        }

        protected override void OnUpdate()
        {
            var cameraEntity = SystemAPI.GetSingletonEntity<FollowCameraTag>();
            var followCameraManaged = EntityManager.GetComponentObject<PlayerFollowCameraManaged>(cameraEntity);
            var cameraComponent = EntityManager.GetComponentData<CameraComponent>(cameraEntity);

            Entities.WithoutBurst().ForEach(
                (ref LocalTransform localTransform, in UnitInputComponent input ,in UnitAttributeComponent attributeComp) =>
                {
                    if (attributeComp.UnitType != UnitType.Player) return;
                    
                    followCameraManaged.FollowCamera.transform.position =
                        localTransform.Position + followCameraManaged.Offset.x * localTransform.Forward() +
                        new float3(0, followCameraManaged.Offset.y, followCameraManaged.Offset.z);

                    var localRotation = quaternion.Euler(math.radians(followCameraManaged.EulerAngle));
                    var rotation = math.mul(localTransform.Rotation, localRotation);
                    followCameraManaged.FollowCamera.transform.rotation = rotation;
            
                    cameraComponent.Forward = localTransform.Forward();
                    cameraComponent.Right = localTransform.Right();
                    EntityManager.SetComponentData(cameraEntity, cameraComponent);
                }).Run();
        }
    }
}