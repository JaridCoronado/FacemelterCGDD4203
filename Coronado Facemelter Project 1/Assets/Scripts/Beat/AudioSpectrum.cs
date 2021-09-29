using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    private float[] _audioSpectrum;
    // Start is called before the first frame update
    void Start()
    {
        _audioSpectrum = new float[128];
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(_audioSpectrum, 0, FFTWindow.BlackmanHarris);
        if (_audioSpectrum != null && _audioSpectrum.Length > 0)
        {
            SpectrumValue = _audioSpectrum[0] * 100;
            
        }

    }
    public static float SpectrumValue { get; private set; }
}
