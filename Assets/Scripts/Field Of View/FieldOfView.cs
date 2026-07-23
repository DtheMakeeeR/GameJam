/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _fov;
    [SerializeField] private float _viewDistance;
    [SerializeField] private Vector3 _origin;
    [SerializeField] private int _rayCount = 200;
    [SerializeField] private float _startingAngle;
    private Mesh mesh;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate() {
        float angle = _startingAngle;
        float angleIncrease = _fov / _rayCount;

        Vector3[] vertices = new Vector3[_rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[_rayCount * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= _rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, Utils.GetVectorFromAngle(angle), _viewDistance, _layerMask);
            if (raycastHit2D.collider == null) {
                // No hit
                vertex = _origin + Utils.GetVectorFromAngle(angle) * _viewDistance;
            } else {
                // Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(_origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin) {
        _origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        _startingAngle = Utils.GetAngleFromVectorFloat(aimDirection) + _fov / 2f;
    }

    public void SetFoV(float fov) {
        _fov = fov;
    }

    public void SetViewDistance(float viewDistance) {
        _viewDistance = viewDistance;
    }

}
