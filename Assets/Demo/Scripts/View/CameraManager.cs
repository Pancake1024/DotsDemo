using PFF.Common;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class CameraManager : SingletonMono<CameraManager>
    {
        [SerializeField]
        private Camera _FollowCamera;
        
        public Camera FollowCamera => _FollowCamera;
        
    }
}