using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSize : MonoBehaviour
{
    [SerializeField] private GameObject _songList;
    [SerializeField] private GameObject _mainmenu;

    private void Start()
    {
        Debug.Log(gameObject.transform.childCount);
        if(this.gameObject.transform.childCount > 0)
        {
            var c = gameObject.GetComponent<Image>().color.a;
            c = 1f;
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((this.transform.GetChild(0).GetComponent<RectTransform>().rect.width * (this.transform.childCount - 2)), 0);
        }
        else
        {
            var _color =gameObject.GetComponent<Image>().color;
            _color.a = 0f;
            gameObject.GetComponent<Image>().color = _color;
        }

    }

    public void Back()
    {
        _songList.SetActive(false);
        _mainmenu.SetActive(true);
    }
}
