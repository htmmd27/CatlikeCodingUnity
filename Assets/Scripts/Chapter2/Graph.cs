using System;
using UnityEngine;

public class Graph : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;
    [SerializeField, Range(10, 200)]
    int resolution = 10;
    // [SerializeField, Range(0, 2)]
    // int function;
    [SerializeField]
    FunctionLibrary.FunctionName function;
    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;
    float duration;
    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode;
    Transform[] points;
    bool transitioning;

    FunctionLibrary.FunctionName transitionFunction;
    void Awake () {
        float step = 2f / resolution;
        //var position = Vector3.zero;
        var scale = Vector3.one * step;
        points = new Transform[resolution  * resolution];
        for (int i = 0; i < points.Length; i++){
            // if (x == resolution) {
            //     x = 0;
            //     z += 1;
            // }
            Transform point = Instantiate(pointPrefab);
            points[i] = point;
            // position.x = (x + 0.5f) * step - 1f;
            // position.z = (z + 0.5f) * step - 1f;
            //position.y = position.x * position.x * position.x;
            //point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (transitioning) 
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration) 
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            //function = FunctionLibrary.GetNextFunctionName(function);
            PickNextFunction();
        }
        if (transitioning) {
            UpdateFunctionTransition();
        }
        else {
            UpdateFunction();
        }
        
        // for (int i = 0; i < points.Length; i++)
        // {
        //     Transform point = points[i];
        //     Vector3 position = point.localPosition;
        //     // if (function == 0) {
        //     //     position.y = FunctionLibrary.Wave(position.x, Time.time);
        //     // }else if (function == 1) {
        //     //     position.y = FunctionLibrary.MultiWave(position.x, Time.time);
        //     // }
        //     // else {
        //     //     position.y = FunctionLibrary.Ripple(position.x, Time.time);
        //     // }
        //     position.y = f(position.x, position.z, Time.time);
        //     point.localPosition = position;
        // }
    }
    void PickNextFunction () {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
    private void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            //float v = (z + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);
        }
    }
    
    void UpdateFunctionTransition () {
        FunctionLibrary.Function
            from = FunctionLibrary.GetFunction(transitionFunction),
            to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(
                u, v, time, from, to, progress
            );
        }
    }
}
