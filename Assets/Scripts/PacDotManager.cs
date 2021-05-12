using System;
using System.Collections.Generic;
using UnityEngine;

public class PacDotManager : MonoBehaviour
{
    public static event Action OnAllDotsEaten;
    private IList<GameObject> _dots;
    private int _numActiveDots;

    public void AddDot(GameObject dot)
    {
        _dots.Add(dot);
        _numActiveDots++;
    }

    public void DeactivateDot(GameObject dot)
    {
        dot.SetActive(false);
        if (--_numActiveDots == 0)
        {
            OnAllDotsEaten?.Invoke();
            ReactivateAllDots();
        }
    }

    private void ReactivateAllDots()
    {
        foreach (GameObject dot in _dots)
        {
            dot.SetActive(true);
        }
    }
    
    private void Awake()
    {
        _dots = new List<GameObject>();
    }
}
