using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An initializer for any utility scripts
/// </summary>
public class GameInitializer : MonoBehaviour
{
    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // initializes utility scripts
        ScreenUtils.Initialize();
    }
}
