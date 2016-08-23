using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SleepFade : MonoBehaviour
{
    Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();

        StartCoroutine(Fade());
	}
	
    IEnumerator Fade()
    {
        Vector2 start = new Vector2(0, 0);
        Vector2 end = new Vector2(1f, 1);
        float speed = 1f;
        float rate = 0f;

        while(rate <= 1f)
        {
            rate += speed * Time.smoothDeltaTime;
            Vector2 mid = Vector2.Lerp(start, end, rate);

            Color clr = image.color;
            clr.a = mid.y;
            image.color = clr;

            yield return null;
        }

        speed = 1.7f;
        rate = 0f;
        while (rate <= 1f)
        {
            rate += speed * Time.smoothDeltaTime;
            Vector2 mid = Vector2.Lerp(end, start, rate);

            Color clr = image.color;
            clr.a = mid.y;
            image.color = clr;

            yield return null;
        }

        speed = 1f;
        rate = 0f;
        while (rate <= 1f)
        {
            rate += speed * Time.smoothDeltaTime;
            Vector2 mid = Vector2.Lerp(start, end, rate);

            Color clr = image.color;
            clr.a = mid.y;
            image.color = clr;

            yield return null;
        }

        speed = 1.4f;
        rate = 0f;
        while (rate <= 1f)
        {
            rate += speed * Time.smoothDeltaTime;
            Vector2 mid = Vector2.Lerp(end, start, rate);

            Color clr = image.color;
            clr.a = mid.y;
            image.color = clr;

            yield return null;
        }

        speed = 0.8f;
        rate = 0f;
        while (rate <= 1f)
        {
            rate += speed * Time.smoothDeltaTime;
            Vector2 mid = Vector2.Lerp(start, end, rate);

            Color clr = image.color;
            clr.a = mid.y;
            image.color = clr;

            yield return null;
        }
    }
}
