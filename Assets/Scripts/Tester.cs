using CustomMath;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private MyTransformVisualizer visualizer;

    [SerializeField] private Transform pivotUnityTransform;

    [SerializeField] private Transform targetUnity;

    [SerializeField] private Vector3 dir;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 eulers;

    [SerializeField] private Vector3 axis;
    [SerializeField] private float angle;

    [SerializeField] private bool shouldRotateAround = false;


    private Vec3 _pivotPos = Vec3.Zero;
    private MyQuaternion _pivotRotation = MyQuaternion.identity;
    private Vec3 _pivotScale = Vec3.Zero;


    [ContextMenu("Test")]
    private void Test()
    {
        // Debug.Log(unityTransform.localToWorldMatrix + "\n" + _transform.localToWorldMatrix);
        // Debug.Log(unityTransform.position);
        // Debug.Log(unityTransform.rotation);
        // Debug.Log(unityTransform.lossyScale);

        // _transform.Rotation = MyQuaternion.Euler(45, 12, 0);
        // Debug.Log(_transform.Rotation.eulerAngles);
        // Debug.Log(_transform.transform.rotation.eulerAngles);

        //unityTransform.SetPositionAndRotation(position, Quaternion.Euler(eulers));
        //_transform.SetPositionAndRotation(new Vec3(position), MyQuaternion.Euler(new Vec3(eulers)));

        _pivotPos = new Vec3(pivotUnityTransform.position);
        _pivotRotation = new MyQuaternion(pivotUnityTransform.rotation);
        _pivotScale = new Vec3(pivotUnityTransform.localScale);

        visualizer.GetTransform(1).Position = new Vec3(0, 0, 0);
        //Works
        // Translate
        // unityTransform.Translate(dir, pivotUnityTransform);
        // visualizer.GetTransform(1).Translate(new Vec3(dir), new MyTransform("Pivot", pivotPos, pivotRotation, pivotScale));

        //Works
        //Rotate based on Eulers
        // unityTransform.Rotate(eulers);
        // visualizer.GetTransform(1).Rotate(new Vec3(eulers));

        //Works
        // //Rotate based on Axis
        // unityTransform.Rotate(axis, angle, Space.Self);
        // visualizer.GetTransform(1).Rotate(new Vec3(axis), angle, Space.Self);

        //RotateAround


        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }

    private void Update()
    {
        _pivotPos = new Vec3(pivotUnityTransform.position);
        _pivotRotation = new MyQuaternion(pivotUnityTransform.rotation);
        _pivotScale = new Vec3(pivotUnityTransform.localScale);

        if (shouldRotateAround)
        {
            unityTransform.RotateAround(pivotUnityTransform.position, axis, angle * Time.deltaTime);
            visualizer.GetTransform(1).RotateAround(_pivotPos, new Vec3(axis), angle * Time.deltaTime);
        }
    }
}