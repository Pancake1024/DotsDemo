using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public struct FollowCameraTag : IComponentData
    {
        
    }
    
    public class PlayerFollowCameraManaged : IComponentData
    {
        public Camera FollowCamera;
        public float3 Offset;
        public float3 EulerAngle;
    }

    public struct CameraComponent : IComponentData
    {
        public float3 Forward;
        public float3 Right;
    }
    
}