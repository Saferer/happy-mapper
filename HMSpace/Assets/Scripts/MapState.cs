using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapState
{
    private static int currentLevel = 0;
    private static Vector2 location = new Vector2(-2.1f, .89f);//new Vector2(2.92f, 0.72f);
    private static int numPressed = 0;

    public static int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }
    }

    public static Vector2 Location
    {
        get
        {
            return location;
        }
        set
        {
            location = value;
        }
    }

    public static int NumPressed
    {
        get
        {
            return numPressed;
        }
        set
        {
            numPressed = value;
        }
      }
}
