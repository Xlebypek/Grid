using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Скрипт висит на Canvas'e.
public class GameMenu : MonoBehaviour
{
    private const string LetterDict = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private int width = 1;
    [SerializeField] private InputField _widthInputField;

    private int height = 1;
    [SerializeField] private InputField _heightInputField;

    [SerializeField] private GameObject _letterPrefab;

    [SerializeField] private RectTransform _letterParent;
    [SerializeField] private GridLayoutGroup _letterParentLayoutGroup;
    #region Buttons

    public void LetterShuffle()
    {
        if (Letter.EnableForAnimation)
        {
            List<Letter> letters = new List<Letter>(_letterParent.GetComponentsInChildren<Letter>());

            while (letters.Count > 1)
            {
                var a = letters[Random.Range(0, letters.Count)];
                letters.Remove(a);
                var b = letters[Random.Range(0, letters.Count)];
                letters.Remove(b);

                a.Swap(b.SwapData);
                b.Swap(a.SwapData);
            }
            
            if (letters.Count == 1)
            {
                var odd = letters[Random.Range(0, letters.Count)];
                odd.OddCell();
                letters.Remove(odd);
            }
        }
    }

    public void Generate()
    {
        //Проверка на Shuffle
        if (!Letter.EnableForAnimation)
            return;
        //Площадь
        int P = width * height; 
        if (P < 1)
        {
            Debug.LogError($"Error size, width = {width}; height = {height}");
            return;
        }
        
        _letterParentLayoutGroup.constraintCount = width;
        _letterParentLayoutGroup.cellSize = CellSize();

        //Разница в количестве существуемых и желаемых букв.
        int diffLetterNum = _letterParent.childCount - P;
        
        if (diffLetterNum > 0)
        {
            //Удаляем лишние буквы с поля.
            for (int i = 0; i < diffLetterNum; i++)
            {
                Destroy(_letterParent.GetChild(i).gameObject);
            }
        }
        else
        {
            //Добавляем недостающие буквы.
            for (int i = 0; i < Mathf.Abs(diffLetterNum); i++)
            {
                Instantiate(_letterPrefab, _letterParent);
            }
        }
        
        //Меняем буквы на всех ячейках.
        foreach (var letter in _letterParent.GetComponentsInChildren<Letter>())
        {
            letter.Value = RandomLetter();
        }
    }

    /// <summary>
    /// Функция для высчитывания размера ячейки в LayoutGroup.
    /// </summary>
    /// <returns></returns>
    private Vector2 CellSize()
    {
        int largerSide = Mathf.Max(width, height);
        float lesserSideLayoutGroup = Mathf.Min(_letterParent.rect.width, _letterParent.rect.height);
        float size = _letterParent.rect.width / largerSide;
        return new Vector2(size, size);
    }

    #endregion /Buttons

    #region InputFields

    public void WidthChange(string value)
    {
        int _IntValue = Int32.Parse(value);
        if ((value.Length < 1) || _IntValue < 1)
        {
            _widthInputField.text = "1";
            return;
        }
        width = _IntValue;
    }
    
    public void HeightChange(string value)
    {
        int _IntValue = Int32.Parse(value);
        if ((value.Length < 1) || _IntValue < 1)
        {
            _heightInputField.text = "1";
            return;
        }
        height = _IntValue;
    }

    #endregion /InputFields
    
    /// <summary>
    /// Возвращает случайную букву английского алфавита.
    /// </summary>
    /// <returns> cast char to string from LetterDict </returns>
    private string RandomLetter()
    {
        return LetterDict[Random.Range(0, LetterDict.Length)].ToString();
    }
}