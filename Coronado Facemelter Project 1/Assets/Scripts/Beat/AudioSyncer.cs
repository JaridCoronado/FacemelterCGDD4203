using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float _bias; // determing what spectrum value going to trigger a beat time
    public float _timeStep; // minimum interval between each beat time
    public float _timeToBeat;
    public float restSmoothTime;

    private float _previousAudioValue;
    private float _currentAudioValue;
    private float _timer;

    protected bool _isBeat;
    // Start is called before the first frame update
    public virtual void OnBeat()
    {
        
        _timer = 0;
        _isBeat = true;
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        _previousAudioValue = _currentAudioValue;
        _currentAudioValue = AudioSpectrum.SpectrumValue;
        if (_previousAudioValue > _bias && _currentAudioValue <= _bias)
        {
            if (_timer > _timeStep)
                OnBeat();
        }
        if (_previousAudioValue <= _bias && _currentAudioValue > _bias)
        {
            if (_timer > _timeStep)
                OnBeat();
        }
        _timer += Time.deltaTime;
    }
    private void Update()
    {
        OnUpdate();
    }
}
