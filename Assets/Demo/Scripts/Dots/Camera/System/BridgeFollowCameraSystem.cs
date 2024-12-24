using Unity.Entities;
using Unity.Mathematics;

namespace Pancake.ECSDemo
{
    public partial class BridgeFollowCameraSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (System.Object.ReferenceEquals(CameraManager.Instance.FollowCamera, null)) return;

            this.Enabled = false;

            var ecb = new EntityCommandBuffer(WorldUpdateAllocator);
            var entity = ecb.CreateEntity();
            ecb.AddComponent(entity,new FollowCameraTag());
            ecb.AddComponent(entity, new CameraComponent());
            ecb.AddComponent(entity,new PlayerFollowCameraManaged()
            {
                FollowCamera = CameraManager.Instance.FollowCamera,
                Offset = new float3(-2,2,0),
                EulerAngle = new float3(20,0,0),
            });
            
            ecb.Playback(EntityManager);
        }
    }
}