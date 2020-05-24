using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPulse : MonoBehaviour
{
    private AudioSource source;
    private Renderer r;
    private float[] samples = new float[512];
    public float peak = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        r = GetComponent<Renderer>();
        //source.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    // Update is called once per frame
    void Update()
    {
        source.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        float largest = 0;
        //int index = 0;
        for (int i = 0; i < samples.Length; ++i)
        {
            if (samples[i] > largest)
            {
                largest = samples[i];
                //index = i;
            }
        }

        if (largest > peak)
            peak = largest;

        if (peak > 0)
        {
            //r.material.color = Color.HSVToRGB(index, 0.55f, 1);
            float s = 1 + (largest / peak) * 0.5f;
            transform.localScale = new Vector3(s, s, s);
        }
    }
}
