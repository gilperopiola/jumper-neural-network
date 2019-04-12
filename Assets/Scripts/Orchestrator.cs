using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour {
    public Population population;

    void Start() {
        ConfigManager.Init("Assets/config.json");

        population = new Population();

        CameraManager.SetCamera(Camera.main);
        CameraManager.SetZoom(450);

        Debug.Log(ConfigManager.config.projectName + " started | debug = " + ConfigManager.config.debugMode);
    }

    void Update() {
        population.Advance();
    }
}
