using UnityEditor;
using UnityEngine;

namespace AGL.Asteroids.Editor
{
    public static class MassParenting
    {
        [MenuItem("GameObject/Convert To Asteroid Debris")]
        public static void ConvertToAsteroidDebris()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject.TryGetComponent(out MeshFilter mesh))
                {
                    // Create a new parent
                    var parent = new GameObject("Parent");
                    Undo.RegisterCreatedObjectUndo(parent, "Create Parent");
                    parent.transform.SetParent(gameObject.transform.parent);
                    parent.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
                    parent.transform.localScale = gameObject.transform.localScale;
                    gameObject.transform.SetParent(parent.transform, true);
                    gameObject.transform.localScale = Vector3.one;

                    // Give them physics components
                    var meshCollider = parent.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    meshCollider.sharedMesh = mesh.sharedMesh;
                    var rigidbody = parent.AddComponent<Rigidbody>();
                    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    rigidbody.useGravity = false;

                    // Set the physics to the correct layer
                    parent.tag = "Asteroid";
                    parent.layer = LayerMask.NameToLayer("Detail");

                    EditorUtility.SetDirty(gameObject);
                }
            }
        }

        public static void MassParent()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                var parent = new GameObject(gameObject.name);
                Undo.RegisterCreatedObjectUndo(parent, "Create Parent");
                parent.transform.SetParent(gameObject.transform.parent);
                parent.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
                gameObject.transform.SetParent(parent.transform, true);
                EditorUtility.SetDirty(gameObject);
            }
        }

        public static void MeshFix()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                if (gameObject.TryGetComponent(out MeshCollider meshCollider))
                {
                    var meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
                    meshCollider.sharedMesh = meshFilter.sharedMesh;
                    EditorUtility.SetDirty(gameObject);
                }
            }
        }

        public static void ScaleFix()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                var child = gameObject.transform.GetChild(0);
                var scale = child.localScale;
                child.localScale = Vector3.one;
                gameObject.transform.localScale = scale;
                EditorUtility.SetDirty(gameObject);
            }
        }
    }
}
