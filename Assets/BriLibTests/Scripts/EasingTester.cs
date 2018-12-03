using UnityEngine;

namespace BriLib.Tests
{
    public class EasingTester : MonoBehaviour
    {
        public Easing.Method EasingMethod;
        public float Duration;
        public bool TestMatrixEasing = false;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private float startTime;
        private float endTime;
        private bool easing = false;
        private bool cameraEasing = false;
        private Matrix4x4 startMatrix;
        private Matrix4x4 endMatrix;
        private Matrix4x4 orthoMatrix;
        private Matrix4x4 perspectiveMatrix;

        private void Start()
        {
            perspectiveMatrix = Camera.main.projectionMatrix;
            orthoMatrix = Matrix4x4.Ortho(-10, 10, -10, 10, 0, 100);
        }
        
        private void Update()
        {
            if (easing)
            {
                transform.position = Easing.Ease(startPoint, endPoint, startTime, endTime, Time.time, EasingMethod);
                easing = Time.time < endTime;
            }
            else if (cameraEasing)
            {
                Camera.main.projectionMatrix = Easing.Ease(startMatrix, endMatrix, startTime, endTime, Time.time, EasingMethod);
                cameraEasing = Time.time < endTime;
            }
            else if (Input.GetMouseButton(0))
            {
                var mouse = Input.mousePosition;
                startPoint = transform.position;
                var diff = Camera.main.transform.position.z - startPoint.z;
                var position = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Mathf.Abs(diff)));
                position.z = transform.position.z;
                endPoint = position;
                startTime = Time.time;
                endTime = startTime + Duration;
                easing = true;
            }
            else if (TestMatrixEasing)
            {
                TestMatrixEasing = false;
                startTime = Time.time;
                endTime = startTime + Duration;
                cameraEasing = true;
                if (Camera.main.projectionMatrix == perspectiveMatrix)
                {
                    startMatrix = perspectiveMatrix;
                    endMatrix = orthoMatrix;
                }
                else
                {
                    startMatrix = orthoMatrix;
                    endMatrix = perspectiveMatrix;
                }
            }
        }
    }
}
