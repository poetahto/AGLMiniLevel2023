using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    public static class MassParenting
    {
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

        [MenuItem("GameObject/Fix")]
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
