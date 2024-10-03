using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpinnerAnimator : MonoBehaviour
{
    [SerializeField]
    private float spinSpeedUpDuration = 1f;   // Duration for speed-up phase
    [SerializeField]
    private float spinNoSpeedLossDuration = 4f;    // Total time for the constant speed phase
    [SerializeField]
    private float spinSlowDownDuration = 10f;   // Duration for slow-down phase
    [SerializeField]
    private int rotationsSpeedUp = 2;   // Number of full rotations during speed-up
    [SerializeField]
    private int rotationsNoSpeedLoss = 10; // Number of full rotations at constant speed
    [SerializeField]
    private int rotationsSlowDown = 4;  // Number of full rotations during slow-down

    private float totalDuration;  // Total duration of the spin animation
    private float elapsedTime;    // Time counter for tracking the animation progress
    private bool isSpinning;      // Flag to indicate if the spin is in progress

    public void SpinWheels(GameObject[] wheelObjects, float finalAngle)
    {
        // Calculate the total duration of the animation
        totalDuration = spinSpeedUpDuration + spinNoSpeedLossDuration + spinSlowDownDuration;
        elapsedTime = 0f;  // Reset the elapsed time
        isSpinning = true;  // Set the spinning flag to true

        for (int i = 0; i < wheelObjects.Length; i++)
        {
            Transform wheelTransform = wheelObjects[i].transform;
            //wheelTransform.rotation = Quaternion.Euler(0, 0, 0);
            // Reset the rotation to make sure it starts from 0

            // Calculate the relative rotation amounts for each phase
            float totalSpeedUpRotation = 360f * rotationsSpeedUp;     // Speed-up rotations
            float totalConstantSpeedRotation = 360f * rotationsNoSpeedLoss; // Constant speed rotations
            float totalSlowDownRotation = 360f * rotationsSlowDown + finalAngle; // Slow-down rotations and final stop angle

            // Create a DOTween sequence for the spin animation
            Sequence spinSequence = DOTween.Sequence();

            // Step 1: Start with an ease-in acceleration to gain speed
            spinSequence.Append(wheelTransform.DORotate(new Vector3(0, 0, totalSpeedUpRotation), spinSpeedUpDuration, RotateMode.FastBeyond360)
                                .SetEase(Ease.InSine));

            // Step 2: Spin for a few rotations at constant speed (additive to the previous rotation)
            spinSequence.Append(wheelTransform.DORotate(new Vector3(0, 0, totalConstantSpeedRotation), spinNoSpeedLossDuration, RotateMode.FastBeyond360)
                                .SetEase(Ease.Linear));

            // Step 3: Ease out and stop at the final angle (additive to the previous rotation)
            spinSequence.Append(wheelTransform.DORotate(new Vector3(0, 0, totalSlowDownRotation), spinSlowDownDuration, RotateMode.FastBeyond360)
                                .SetEase(Ease.OutQuint));

            // Start the sequence
            spinSequence.Play();
        }

        // Start a coroutine to update the elapsed time
        StartCoroutine(UpdateElapsedTime());
    }

    private IEnumerator UpdateElapsedTime()
    {
        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }
        isSpinning = false; // Set spinning flag to false when done
    }

    public bool CheckAnimation()
    {
        return isSpinning; // Check if the total duration is exceeded
    }
}
