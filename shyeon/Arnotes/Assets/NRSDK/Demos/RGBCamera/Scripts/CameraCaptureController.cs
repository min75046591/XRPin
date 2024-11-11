using System;
using UnityEngine;
using UnityEngine.UI;
namespace NRKernal.NRExamples
{
    public class CameraCaptureController : MonoBehaviour
    {
        /// <summary> The capture image. </summary>
        public RawImage CaptureImage;

        /// <summary> Gets or sets the RGB camera texture. </summary>
        /// <value> The RGB camera texture. </value>
        private NRRGBCamTexture RGBCamTexture { get; set; }

        void Start()
        {
            if (!NRDevice.Subsystem.IsFeatureSupported(NRSupportedFeature.NR_FEATURE_RGB_CAMERA))
                throw new Exception("RGBCamera is not supported on current glasses.");

            RGBCamTexture = new NRRGBCamTexture();
            CaptureImage.texture = RGBCamTexture.GetTexture();
            RGBCamTexture.Play();
        }

        /// <summary> Executes the 'destroy' action. </summary>
        void OnDestroy()
        {
            RGBCamTexture?.Stop();
            RGBCamTexture = null;
        }

        protected void Validate(ProjectionValidater validator)
        {
            if (RGBCamTexture != null)
            {
                validator.ProjectPointFunc = RGBCamTexture.RGBCamera.NativeRGBCamera.ProjectPoint;
                validator.UnProjectPointFunc = RGBCamTexture.RGBCamera.NativeRGBCamera.UnProjectPoint;
                validator.StartValidate("rgb_project.csv", "rgb_unproject.csv", "rgb_test_results.csv");
                NRDebugger.Info($"[RGBCameraProjection] Validate Finish");
            }
        }
    }
}
