using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [Header("-- SETUP --")]
    private static List<Car> _freeCars = new List<Car>();

    public static Car GetTheNextFreeCar()
    {
        if (_freeCars.Count == 0 || _freeCars == null)
            return null;
        else
            return _freeCars[0];
    }

    public static void AddFreeCar(Car car) 
    {
        if (!_freeCars.Contains(car))
            _freeCars.Add(car);
    }

    public static void RemoveFreeCar(Car car)
    {
        if (_freeCars.Contains(car))
            _freeCars.Remove(car);
    }
}
