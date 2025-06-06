using UnityEngine;
using UnityEngine.UI;

namespace StupidTemplate.Mods
{
    internal static class CameraController
    {
        private static Camera spectatorCamera;
        private static GameObject cameraObject;
        private static RenderTexture spectatorTexture;

        private static GameObject uiCanvasObject;
        private static RawImage rawImage;

        private static Transform target;

        private static readonly Vector3 offset = new Vector3(0, 2f, -3f);

        public static void SetTarget(GameObject newTarget)
        {
            if (newTarget == null)
            {
                Debug.LogWarning("CameraController: Tried to set null target.");
                return;
            }

            target = newTarget.transform;

            if (spectatorCamera == null)
            {
                Debug.Log("CameraController: Creating Spectator Camera...");

                // Create the spectator camera GameObject
                cameraObject = new GameObject("SpectatorCamera");
                spectatorCamera = cameraObject.AddComponent<Camera>();
                spectatorCamera.clearFlags = CameraClearFlags.Skybox;
                spectatorCamera.fieldOfView = 60f;

                // IMPORTANT: Disable VR rendering so it only shows on desktop screen
                spectatorCamera.stereoTargetEye = StereoTargetEyeMask.None;

                // Create and assign render texture
                spectatorTexture = new RenderTexture(Screen.width, Screen.height, 24);
                spectatorCamera.targetTexture = spectatorTexture;

                Object.DontDestroyOnLoad(cameraObject);

                // Add follower script
                cameraObject.AddComponent<SpectatorFollow>().SetTarget(target);

                // Create UI Canvas to show the camera feed
                CreateSpectatorCanvas();
            }

            cameraObject.GetComponent<SpectatorFollow>().SetTarget(target);
        }

        private static void CreateSpectatorCanvas()
        {
            uiCanvasObject = new GameObject("SpectatorCanvas");
            Canvas canvas = uiCanvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            Object.DontDestroyOnLoad(uiCanvasObject);

            var scaler = uiCanvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            GameObject rawImageObject = new GameObject("SpectatorView");
            rawImageObject.transform.SetParent(uiCanvasObject.transform, false);

            rawImage = rawImageObject.AddComponent<RawImage>();
            rawImage.texture = spectatorTexture;

            // Use unlit material to avoid transparency or color issues
            rawImage.material = new Material(Shader.Find("Unlit/Texture"));

            RectTransform rt = rawImage.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    internal class SpectatorFollow : MonoBehaviour
    {
        private Transform followTarget;
        private readonly Vector3 offset = new Vector3(0, 2f, -3f);
        private readonly float followSpeed = 5f;

        public void SetTarget(Transform target)
        {
            followTarget = target;
        }

        void LateUpdate()
        {
            if (followTarget == null) return;

            Vector3 desiredPosition = followTarget.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            Vector3 lookAt = followTarget.position + Vector3.up;
            Quaternion desiredRotation = Quaternion.LookRotation(lookAt - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, followSpeed * Time.deltaTime);
        }
    }
}
