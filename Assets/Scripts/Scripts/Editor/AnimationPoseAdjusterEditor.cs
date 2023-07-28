// This script is an editor script for the AnimationPoseAdjuster component. It provides a custom inspector for the component in the Unity Editor.

using UnityEditor;

namespace PoseAdjuster
{
    [CustomEditor(typeof(AnimationPoseAdjuster))]
    public class AnimationPoseAdjusterEditor : Editor
    {
        private AnimationPoseAdjuster poseAdjustment;
        private bool adjustPoseNextFrame = false;
        private float previousPoseTime;

        // Called when the editor script is enabled
        private void OnEnable()
        {
            poseAdjustment = (AnimationPoseAdjuster)target;
            previousPoseTime = poseAdjustment.poseTime;
        }

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (poseAdjustment.animationClip == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            // Display a slider to adjust the pose time
            poseAdjustment.poseTime = EditorGUILayout.Slider("Pose Time", poseAdjustment.poseTime, 0f, poseAdjustment.animationClip.length);

            if (EditorGUI.EndChangeCheck() && poseAdjustment.poseTime != previousPoseTime)
            {
                // If the pose time has changed, set the flag to adjust the pose on the next frame
                adjustPoseNextFrame = true;
                previousPoseTime = poseAdjustment.poseTime;
            }

            EditorGUILayout.Space();

            if (adjustPoseNextFrame)
            {
                // Repaint the Scene view to update the pose adjustment
                SceneView.RepaintAll();

                // Subscribe to the EditorApplication.update event to perform the pose adjustment on the next frame
                EditorApplication.update += PerformPoseAdjustment;
            }
        }

        // Perform the pose adjustment
        private void PerformPoseAdjustment()
        {
            poseAdjustment.AdjustPose();

            // Reset the flag and unsubscribe from the EditorApplication.update event
            adjustPoseNextFrame = false;
            EditorApplication.update -= PerformPoseAdjustment;
        }
    }
}