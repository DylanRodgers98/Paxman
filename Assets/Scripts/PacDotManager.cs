using System;
using UnityEngine;

public class PacDotManager : MonoBehaviour
{
    private int _numberOfDots;

    public void IncrementNumberOfDots()
    {
        _numberOfDots++;
    }

    public void DecrementNumberOfDots()
    {
        if (--_numberOfDots == 0)
        {
            // TODO: implement something for when player eats all dots
            Time.timeScale = 0;
            Debug.Log("You ate all the dots!");
        }
    }
}
