using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Letter : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private LayoutElement _layoutElement;
    
    private const float animationTime = 2;
    private static float t;
    private static bool timerEnable = true; //true - можно начинать, false - таймер ещё действует.
    public static bool EnableForAnimation => timerEnable;
    
    /// <summary>
    /// Кортеж из нужных для shuffle данных. Координаты + буква.
    /// </summary>
    public Tuple<Vector3, string> SwapData{
        get { return new Tuple<Vector3, string>(transform.position, Value); }
    }

    /// <summary>
    /// Текущая буква ячейки
    /// </summary>
    public string Value
    {
        get
        {
            if (_text != null)
            {
                return _text.text;
            }
            else
            {
                return "";
            }
        }
        set
        {
            if (value.Length > 1)
                Debug.LogError("Неправильная длина string: " + value);

            if (_text != null)
            {
                _text.text = value;
            }
        }
    }

    /// <summary>
    /// Вспомогательный метод для анимации передвижения буквы, фактически меняющий буквы местами.
    /// </summary>
    public void Swap(Tuple<Vector3, string> swapData)
    {
        StartCoroutine(Timer());
        StartCoroutine(ShuffleAnimation(swapData.Item1, swapData.Item2));
    }

    static IEnumerator Timer()
    {
        if (timerEnable)
        {
            timerEnable = false;
            t = 0;
            while (t < animationTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            timerEnable = true;
        }
    }

    /// <summary>
    /// Сама анимация перемещения буквы во время shuffle.
    /// </summary>
    IEnumerator ShuffleAnimation(Vector3 target, string newLetter)
    {
        _layoutElement.ignoreLayout = true;
        Vector3 StartPos = transform.position;
        while (t < animationTime)
        {
            transform.position = Vector3.Lerp(StartPos, target, t / animationTime);
            yield return null;;
        }
        _layoutElement.ignoreLayout = false;
        Value = newLetter;
    }

    /// <summary>
    /// Вспомогательный метод для последнего элемента при нечетном количестве букв.
    /// </summary>
    public void OddCell()
    {
        StartCoroutine(OddTimer());
    }

    IEnumerator OddTimer()
    {
        _layoutElement.ignoreLayout = true;
        while (t < animationTime)
        {
            yield return null;
        }
        _layoutElement.ignoreLayout = false;
    }
}
