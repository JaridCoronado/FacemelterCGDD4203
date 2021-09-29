using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioSyncScale : AudioSyncer
{
    public Vector3 beatScale;
    public Vector3 restScale;
    public Color start;
    public Color end;
    private IEnumerator MoveToScale(Vector3 _target)
    {
        Vector3 _curr = transform.localScale;
        Vector3 _initial = _curr;
        Color _currColor = gameObject.GetComponent<Image>().color;
        Color _initalColor = end;
        float _timer = 0;
        while (_curr != _target)
        {
            _curr = Vector3.Lerp(_initial, _target, _timer / _timeToBeat);
            _currColor = Color.Lerp(_initalColor, end, _timer / _timeToBeat);
            _timer += Time.deltaTime;
            transform.localScale = _curr;
            gameObject.GetComponent<Image>().color = _currColor;
            yield return null;
        }
        _isBeat = false;
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_isBeat) return;
        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
        gameObject.GetComponent<Image>().color = Color.Lerp(gameObject.GetComponent<Image>().color, start, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();
        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }
}
