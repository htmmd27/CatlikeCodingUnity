using System;
using UnityEngine;

public class GPUGraph : MonoBehaviour {
    
    [SerializeField, Range(10, 1000)]
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
    bool transitioning;
    [SerializeField]
    ComputeShader computeShader;
    FunctionLibrary.FunctionName transitionFunction;
    private ComputeBuffer positionsBuffer;
    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;
    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");
    private void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
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
        UpdateFunctionOnGPU();
    }
    void PickNextFunction () {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
    
    void UpdateFunctionOnGPU () {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        
        computeShader.SetBuffer(0, positionsId, positionsBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
        
    }
    
}
