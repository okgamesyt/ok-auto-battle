using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour
    where T : Manager<T>
{ 
    public static T Instance;

    protected void Awake()
    {
        Instance = (T)this;
    }
}
