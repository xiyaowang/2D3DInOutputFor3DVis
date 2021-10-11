using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingHidenCube : MonoBehaviour
{
    protected Mesh mesh;
    public Shader shader;
    public Material material;

    private Vector3 position = Vector3.zero;
    public Vector3 Position {
        get => position;
        set {
            position = value;
            Mvm = Matrix4x4.TRS(Position,Rotation,Sacling);
        }
    }

    private Quaternion rotation = Quaternion.identity;
    public Quaternion Rotation {
        get => rotation;
        set {
            rotation = value;
            Mvm = Matrix4x4.TRS(Position,Rotation,Sacling);
        }
    }


    private Vector3 sacling = Vector3.one*0.3f;
    public Vector3 Sacling {
        get => sacling;
        set {
            sacling = value;
            Mvm = Matrix4x4.TRS(Position,Rotation,Sacling);
        }
    }
    private Matrix4x4 Mvm;


    private void Start() {
        Sacling = new Vector3(0.3f,0.3f,0.002f);
    }

    public void Render(Matrix4x4 mvm, int layer) {
        Graphics.DrawMesh(mesh,mvm*Mvm ,material,layer);
    }

    public void Load() {
        material = new Material(shader);
        mesh = BuildCubeMesh();
        material.SetColor("_Color",new Color(1f,1f,1f,1f));
    }

    protected Mesh BuildCubeMesh() {
        Vector3[] vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f,  0.5f,  -0.5f),
                new Vector3 (-0.5f,  0.5f,  -0.5f),
                new Vector3 (-0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f, -0.5f,  0.5f),
                new Vector3 (-0.5f, -0.5f,  0.5f),
            };
        int[] triangles = new int[] {
                0, 2, 1,
                0, 3, 2,
                2, 3, 4,
                2, 4, 5,
                1, 2, 5,
                1, 5, 6,
                0, 7, 4,
                0, 4, 3,
                5, 4, 7,
                5, 7, 6,
                0, 6, 7,
                0, 1, 6
            };

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
