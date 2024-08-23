using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomMath
{
    [Serializable]
    public class MyTransform : MonoBehaviour, IEnumerable
    {
        #region Variables

        private MY4X4 matrixTRS;

        [SerializeField] private Vec3 localPosition;
        [SerializeField] private Vec3 rotationEulers;
        [SerializeField] private MyQuaternion localRotation;
        [SerializeField] private Vec3 scale;
        [SerializeField] private MyTransform parent;

        private Vec3 _worldPosition;
        private MyQuaternion _worldRotation;
        private Vec3 _lossyScale;
        private List<MyTransform> _children = new List<MyTransform>();

        #endregion

        #region Constructors

        protected MyTransform()
        {
            localPosition = Vec3.Zero;
            localRotation = MyQuaternion.identity;
            scale = Vec3.One;
            matrixTRS = MY4X4.TRS(localPosition, localRotation, scale);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Matrix that transforms a point from world space into local space (Read Only).
        /// </summary>
        public MY4X4 worldToLocalMatrix { get; }

        /// <summary>
        ///   Matrix that transforms a point from local space into world space (Read Only).
        /// </summary>
        public MY4X4 localToWorldMatrix
        {
            get
            {
                if (parent == null)
                    return matrixTRS;

                return parent.localToWorldMatrix * matrixTRS;
            }
        }

        /// <summary>
        ///   The world space position of the MyTransform.
        /// </summary>
        public Vec3 Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///   Position of the MyTransform relative to the parent MyTransform.
        /// </summary>
        public Vec3 LocalPosition
        {
            get { return localPosition; }
            set { localPosition = value; }
        }

        /// <summary>
        ///   The rotation as Euler angles in degrees.
        /// </summary>
        public Vec3 eulerAngles
        {
            get { throw new NotImplementedException(); }
            set { SetPositionAndRotation(localPosition, MyQuaternion.Euler(value.x, value.y, value.z)); }
        }

        /// <summary>
        ///   The rotation as Euler angles in degrees relative to the parent MyTransform's rotation.
        /// </summary>
        public Vec3 localEulerAngles
        {
            get { return matrixTRS.rotation.eulerAngles; }
            set { SetLocalPositionAndRotation(localPosition, MyQuaternion.Euler(value.x, value.y, value.z)); }
        }

        /// <summary>
        ///   The red axis of the MyTransform in world space.
        /// </summary>
        public Vec3 right { get; set; }

        /// <summary>
        ///   The green axis of the MyTransform in world space.
        /// </summary>
        public Vec3 up { get; set; }

        /// <summary>
        ///   Returns a normalized vector representing the blue axis of the MyTransform in world space.
        /// </summary>
        public Vec3 forward { get; set; }

        /// <summary>
        ///   A MyMyQuaternion that stores the rotation of the MyTransform in world space.
        /// </summary>
        public MyQuaternion Rotation
        {
            get { return localToWorldMatrix.rotation; }
            set
            {
                //Should set local rotation in a certain way that the global rotation matches when multiplying with all parents
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   The rotation of the MyTransform relative to the MyTransform rotation of the parent.
        /// </summary>
        public MyQuaternion LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
                rotationEulers = new Vec3(localRotation.eulerAngles);
                matrixTRS.SetTRS(localPosition, localRotation, localScale);
            }
        }

        /// <summary>
        ///   The scale of the MyTransform relative to the GameObjects parent.
        /// </summary>
        public Vec3 localScale
        {
            get { return new Vec3(matrixTRS.lossyScale); }
            set
            {
                scale = value;
                matrixTRS.SetTRS(localPosition, localRotation, scale);
            }
        }

        /// <summary>
        ///   The global scale of the object (Read Only).
        /// </summary>
        public Vec3 lossyScale
        {
            get { return localToWorldMatrix.lossyScale; }
        }

        /// <summary>
        ///   The parent of the MyTransform.
        /// </summary>
        public MyTransform Parent
        {
            get { return parent; }
            set { SetParent(value); }
        }

        /// <summary>
        ///   Returns the topmost MyTransform in the hierarchy.
        /// </summary>
        public MyTransform root { get; }

        /// <summary>
        ///   The number of children the parent MyTransform has.
        /// </summary>
        public int childCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        ///   The MyTransform capacity of the MyTransform's hierarchy data structure.
        /// </summary>
        public int hierarchyCapacity { get; set; }

        /// <summary>
        ///   The number of MyTransforms in the MyTransform's hierarchy data structure.
        /// </summary>
        public int hierarchyCount { get; }

        #endregion

        #region Functions

        private void OnValidate()
        {
            localRotation = MyQuaternion.Euler(rotationEulers);

            matrixTRS.SetTRS(localPosition, localRotation, scale);
            _worldPosition = localToWorldMatrix.GetPosition();
            _worldRotation = Rotation;
            _lossyScale = lossyScale;

            transform.SetLocalPositionAndRotation(localPosition, localRotation.toQuaternion);
            transform.localScale = scale;
        }

        #region Hierarchy

        /// <summary>
        ///   Set the parent of the MyTransform.
        /// </summary>
        public void SetParent(MyTransform newParent)
        {
            parent.RemoveChild(this);
            parent = newParent;
            parent.AddChild(this);

            //Translate to match local position relative to parent
        }

        /// <summary>
        ///   Set the parent of the MyTransform.
        /// </summary>
        /// <param name="newParent">The parent MyTransform to use.</param>
        /// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
        public void SetParent(MyTransform newParent, bool worldPositionStays)
        {
            parent.RemoveChild(this);
            parent = newParent;
            parent.AddChild(this);

            //Update local position without moving world position
        }

        public void RemoveChild(MyTransform child)
        {
            _children.Remove(child);
        }

        public void AddChild(MyTransform child)
        {
            _children.Add(child);
        }

        /// <summary>
        ///   Unparents all children.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void DetachChildren();

        /// <summary>
        ///   Move the MyTransform to the start of the local MyTransform list.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetAsFirstSibling();

        /// <summary>
        ///   Move the MyTransform to the end of the local MyTransform list.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetAsLastSibling();

        /// <summary>
        ///   Sets the sibling index.
        /// </summary>
        /// <param name="index">Index to set.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetSiblingIndex(int index);

        /// <summary>
        ///   Gets the sibling index.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int GetSiblingIndex();

        /// <summary>
        ///   Finds a child by name n and returns it.
        /// </summary>
        /// <param name="n">Name of child to be found.</param>
        /// <returns>
        ///   The found child MyTransform. Null if child with matching name isn't found.
        /// </returns>
        public MyTransform Find(string n)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        ///   Sets the world space position and rotation of the MyTransform component.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void SetPositionAndRotation(Vec3 position, MyQuaternion rotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Sets the position and rotation of the MyTransform component in local space (i.e. relative to its parent MyTransform).
        /// </summary>
        /// <param name="localPosition"></param>
        /// <param name="localRotation"></param>
        public void SetLocalPositionAndRotation(Vec3 localPosition, MyQuaternion localRotation)
        {
            throw new NotImplementedException();
        }

        public void GetPositionAndRotation(out Vec3 position, out MyQuaternion rotation)
        {
            MY4X4 transformedMatrix = localToWorldMatrix;
            position = transformedMatrix.GetPosition();
            rotation = transformedMatrix.rotation;
        }

        public void GetLocalPositionAndRotation(out Vec3 localPosition, out MyQuaternion localRotation)
        {
            localPosition = matrixTRS.GetPosition();
            localRotation = matrixTRS.rotation;
        }

        #region Translates

        /// <summary>
        ///   Moves the MyTransform in the direction and distance of translation.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="relativeTo"></param>
        public void Translate(Vec3 translation, [DefaultValue("Space.Self")] Space relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Moves the MyTransform in the direction and distance of translation.
        /// </summary>
        /// <param name="translation"></param>
        public void Translate(Vec3 translation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Moves the MyTransform by x along the x axis, y along the y axis, and z along the z axis.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="relativeTo"></param>
        public void Translate(float x, float y, float z, [DefaultValue("Space.Self")] Space relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Moves the MyTransform by x along the x axis, y along the y axis, and z along the z axis.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Translate(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Moves the MyTransform in the direction and distance of translation.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="relativeTo"></param>
        public void Translate(Vec3 translation, MyTransform relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Moves the MyTransform by x along the x axis, y along the y axis, and z along the z axis.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="relativeTo"></param>
        public void Translate(float x, float y, float z, MyTransform relativeTo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Rotates

        /// <summary>
        ///   Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).
        /// </summary>
        /// <param name="eulers">The rotation to apply in euler angles.</param>
        /// <param name="relativeTo">Determines whether to rotate the GameObject either locally to  the GameObject or relative to the Scene in world space.</param>
        public void Rotate(Vec3 eulers, [DefaultValue("Space.Self")] Space relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).
        /// </summary>
        /// <param name="eulers">The rotation to apply in euler angles.</param>
        public void Rotate(Vec3 eulers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The implementation of this method applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).
        /// </summary>
        /// <param name="xAngle">Degrees to rotate the GameObject around the X axis.</param>
        /// <param name="yAngle">Degrees to rotate the GameObject around the Y axis.</param>
        /// <param name="zAngle">Degrees to rotate the GameObject around the Z axis.</param>
        /// <param name="relativeTo">Determines whether to rotate the GameObject either locally to the GameObject or relative to the Scene in world space.</param>
        public void Rotate(float xAngle, float yAngle, float zAngle, [DefaultValue("Space.Self")] Space relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The implementation of this method applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).
        /// </summary>
        /// <param name="xAngle">Degrees to rotate the GameObject around the X axis.</param>
        /// <param name="yAngle">Degrees to rotate the GameObject around the Y axis.</param>
        /// <param name="zAngle">Degrees to rotate the GameObject around the Z axis.</param>
        public void Rotate(float xAngle, float yAngle, float zAngle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the object around the given axis by the number of degrees defined by the given angle.
        /// </summary>
        /// <param name="axis">The axis to apply rotation to.</param>
        /// <param name="angle">The degrees of rotation to apply.</param>
        /// <param name="relativeTo">Determines whether to rotate the GameObject either locally to the GameObject or relative to the Scene in world space.</param>
        public void Rotate(Vec3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the object around the given axis by the number of degrees defined by the given angle.
        /// </summary>
        /// <param name="axis">The axis to apply rotation to.</param>
        /// <param name="angle">The degrees of rotation to apply.</param>
        public void Rotate(Vec3 axis, float angle)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        ///   Rotates the MyTransform about axis passing through point in world coordinates by angle degrees.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        public void RotateAround(Vec3 point, Vec3 axis, float angle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the MyTransform so the forward vector points at target's current position.
        /// </summary>
        /// <param name="target">Object to point towards.</param>
        /// <param name="worldUp">Vector specifying the upward direction.</param>
        public void LookAt(MyTransform target, [DefaultValue("Vec3.up")] Vec3 worldUp)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the MyTransform so the forward vector points at target's current position.
        /// </summary>
        /// <param name="target">Object to point towards.</param>
        public void LookAt(MyTransform target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the MyTransform so the forward vector points at worldPosition.
        /// </summary>
        /// <param name="worldPosition">Point to look at.</param>
        /// <param name="worldUp">Vector specifying the upward direction.</param>
        public void LookAt(Vec3 worldPosition, [DefaultValue("Vec3.up")] Vec3 worldUp)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Rotates the MyTransform so the forward vector points at worldPosition.
        /// </summary>
        /// <param name="worldPosition">Point to look at.</param>
        public void LookAt(Vec3 worldPosition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        public Vec3 MyTransformDirection(Vec3 direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms direction x, y, z from local space to world space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 MyTransformDirection(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void MyTransformDirections(ReadOnlySpan<Vec3> directions, Span<Vec3> myTransformedDirections)
        {
            throw new NotImplementedException();
        }

        public void MyTransformDirections(Span<Vec3> directions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms a direction from world space to local space. The opposite of MyTransform.MyTransformDirection.
        /// </summary>
        /// <param name="direction"></param>
        public Vec3 InverseMyTransformDirection(Vec3 direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms the direction x, y, z from world space to local space. The opposite of MyTransform.MyTransformDirection.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 InverseMyTransformDirection(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformDirections(ReadOnlySpan<Vec3> directions, Span<Vec3> myTransformedDirections)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformDirections(Span<Vec3> directions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms vector from local space to world space.
        /// </summary>
        /// <param name="vector"></param>
        public Vec3 MyTransformVector(Vec3 vector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms vector x, y, z from local space to world space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 MyTransformVector(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void MyTransformVectors(ReadOnlySpan<Vec3> vectors, Span<Vec3> myTransformedVectors)
        {
            throw new NotImplementedException();
        }

        public void MyTransformVectors(Span<Vec3> vectors)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms a vector from world space to local space. The opposite of MyTransform.MyTransformVector.
        /// </summary>
        /// <param name="vector"></param>
        public Vec3 InverseMyTransformVector(Vec3 vector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms the vector x, y, z from world space to local space. The opposite of MyTransform.MyTransformVector.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 InverseMyTransformVector(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformVectors(ReadOnlySpan<Vec3> vectors, Span<Vec3> myTransformedVectors)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformVectors(Span<Vec3> vectors)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms position from local space to world space.
        /// </summary>
        /// <param name="position"></param>
        public Vec3 MyTransformPoint(Vec3 position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms the position x, y, z from local space to world space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 MyTransformPoint(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void MyTransformPoints(ReadOnlySpan<Vec3> positions, Span<Vec3> myTransformedPositions)
        {
            throw new NotImplementedException();
        }

        public void MyTransformPoints(Span<Vec3> positions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms position from world space to local space.
        /// </summary>
        /// <param name="position"></param>
        public Vec3 InverseMyTransformPoint(Vec3 position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   MyTransforms the position x, y, z from world space to local space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3 InverseMyTransformPoint(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformPoints(ReadOnlySpan<Vec3> positions, Span<Vec3> myTransformedPositions)
        {
            throw new NotImplementedException();
        }

        public void InverseMyTransformPoints(Span<Vec3> positions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Is this MyTransform a child of parent?
        /// </summary>
        /// <param name="parent"></param>
        public bool IsChildOf(MyTransform parent)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internals

        /// <summary>
        ///   Has the MyTransform changed since the last time the flag was set to 'false'?
        /// </summary>
        public extern bool hasChanged
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Returns a MyTransform child by index.
        /// </summary>
        /// <param name="index">Index of the child MyTransform to return. Must be smaller than MyTransform.childCount.</param>
        /// <returns>
        ///   MyTransform child by index.
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MyTransform GetChild(int index);

        #endregion
    }
}