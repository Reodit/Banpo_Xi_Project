using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;

        Vector3 fromAnglePos = fov.CirclePoint(-1 * fov.ViewAngle * 0.5f);

        Handles.color = new Color(1, 1, 1, 0.2f);

        Handles.DrawWireDisc(fov.transform.position,
            Vector3.up,
            fov.ViewRange);

        Handles.DrawWireArc(fov.transform.position,
            Vector3.up,
            fromAnglePos,
            fov.ViewAngle,
            fov.ViewRange);

        Handles.Label(fov.transform.position + (fov.transform.forward * 2f),
            fov.ViewAngle.ToString());
    }
}
