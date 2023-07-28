using UnityEditor;
using UnityEngine;

namespace PoseAdjuster
{
    [RequireComponent(typeof(Animator))]
    [ExecuteInEditMode]
    public class AnimationPoseAdjuster : MonoBehaviour
    {
        Animator animator;
        [Tooltip("The animation clip used for pose adjustment.")]
        public AnimationClip animationClip;
        [Tooltip("The time at which the pose should be adjusted within the animation clip.")]
        public float poseTime;

        [Tooltip("Enables/disables animation root motion.")]
        public bool enableRootMotion = false;
        bool adjustPose;
        Vector3 originalHipsPosition;
        Vector3 originalNonHumanHipsPosition;
        [Tooltip("If you have a non-humanoid rig, specify the Spine, Hips, or GameObject Transform here to disable root motion. Otherwise, leave this field empty.")]
        [SerializeField] Transform nonHumanHipsOrSpineTransform;

        private void Update()
        {
            if (adjustPose)
            {
                AdjustPose();

                // Mark scene as dirty to apply changes
                EditorUtility.SetDirty(gameObject);

                // Reset the adjustPose flag
                adjustPose = false;
            }
        }
        // This method is used to set the pose time and immediately perform the pose adjustment.
        public void SetPoseTimeAndSetPose(float time)
        {
            poseTime = time;
            AdjustPose();
        }

        public void AdjustPose()
        {
            if (animationClip == null)
            {
                return;
            }
            animator = GetComponent<Animator>();
            // Store the current state of the animator
            AnimatorControllerParameter[] parameters = animator.parameters;
            bool[] parameterValues = new bool[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = animator.GetBool(parameters[i].name);
            }

            // Disable the animator to prevent animation updates
            animator.enabled = false;

            Transform hipsTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
            // Store the original root position
            if (hipsTransform != null)
            {
                originalHipsPosition = hipsTransform.position;
            }

            if (nonHumanHipsOrSpineTransform != null)
            {
                originalNonHumanHipsPosition = nonHumanHipsOrSpineTransform.position;
            }



            // Set the pose time

            animationClip.SampleAnimation(gameObject, poseTime);

            // Restore the previous state of the animator
            for (int i = 0; i < parameters.Length; i++)
            {
                animator.SetBool(parameters[i].name, parameterValues[i]);
            }

            if (enableRootMotion)
            {
                return;
            }
            // Reset the root position and rotation to the original values
            if (hipsTransform != null && nonHumanHipsOrSpineTransform ==null)
            {
                hipsTransform.position = new Vector3(originalHipsPosition.x, hipsTransform.position.y, originalHipsPosition.z); ;
            }
            if (nonHumanHipsOrSpineTransform != null)
            {
                nonHumanHipsOrSpineTransform.position = new Vector3(originalNonHumanHipsPosition.x,
                                                                    nonHumanHipsOrSpineTransform.position.y,
                                                                    originalNonHumanHipsPosition.z); 
            }



            //Enable animator if required
            // animator.enabled = true;
        }
    }
}
