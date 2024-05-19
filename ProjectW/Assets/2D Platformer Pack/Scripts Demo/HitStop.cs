using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{

    public float stopDuration;
    public float stopDurationStrong;

    public void Stop()
    {
        Time.timeScale = 0f;

        StartCoroutine(Duration());

    }

    public void HardStop()
    {
        Time.timeScale = 0f;

        StartCoroutine(DurationStrong());

    }

    IEnumerator Duration()
    {
        yield return new WaitForSecondsRealtime(stopDuration);
        Time.timeScale = 1f;
    }

    IEnumerator DurationStrong()
    {
        yield return new WaitForSecondsRealtime(stopDurationStrong);
        Time.timeScale = 1f;
    }

}
