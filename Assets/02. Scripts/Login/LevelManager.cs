using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class LevelManager : NetworkSceneManagerDefault
{
    SceneRef _currentScene;
    bool _currentSceneOutdated;
    
    static LevelManager _instance;
    public static LevelManager Instance { get; }

    void Awake()
    {
        if (_instance)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    protected override void LateUpdate()
    {
        if (!Runner)
        {
            return;
        }

        if (Runner.CurrentScene != _currentScene)
        {
            _currentSceneOutdated = true;
        }
    }

    public void LoadSceneAsync(SceneRef prevScene, SceneRef newScene)
    {
        SwitchScene(prevScene, newScene, (objects => Debug.Log("Finished")));
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        yield return base.SwitchScene(prevScene, newScene, finished);
    }
}
