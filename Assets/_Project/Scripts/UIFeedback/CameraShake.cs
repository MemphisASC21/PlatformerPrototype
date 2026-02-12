using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    /*-------CameraShake--------
     * We can use this for jump but also health damage. 
     * 
     * UPDATE:
     * It is in a coroutine now rather than update. I thought performance wise it would be better to not have it check on update for the
     * shake timer and instead run a coroutine when called.
     */

    //variables
    private Coroutine currentShake;
    [SerializeField] private CameraConfig config;

    Vector3 initialPosition;
    void Awake()
    {
        initialPosition = transform.localPosition;
    }

    //for jump and land shake
    public void PlayLandingShake()
    {
        TriggerShake(config.landDuration, config.landMagnitude);
    }

    //for getting damage or hit shake
    public void PlayDamageShake()
    {
        TriggerShake(config.damageDuration, config.damageMagnitude);
    }

    public void PlayThwompShake()
    {
        TriggerShake(config.thwompHitDuration, config.thwompHitMagnitude);
    }

    private void TriggerShake(float duration, float magnitude)
    {
        if (currentShake != null) StopCoroutine(currentShake);
        currentShake = StartCoroutine(ShakeRoutine(duration, magnitude));
    }


    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;
    }
}
