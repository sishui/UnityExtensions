#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityExtensions.Editor;
using System;

namespace UnityExtensions
{
    public partial class CardinalPathWithRotation
    {
        protected override Type floatingWindowType => typeof(CardinalPathWithRotationFloatingWindow);


        class CardinalPathWithRotationFloatingWindow : CardinalPathFloatingWindow<CardinalPathWithRotation>
        {
            protected override bool disableRotateTool => false;


            protected override void OnRotateToolWindowGUI(CardinalPathWithRotation path)
            {
                EditorGUILayout.LabelField("Rotation", EditorStyles.centeredGreyMiniLabel);

                using (var scope = new ChangeCheckScope(path))
                {
                    using (new LabelWidthScope(EditorGUIUtility.singleLineHeight))
                    {
                        Vector3 eulerAngles = default;
                        bool lookTangent = true;
                        eulerAngles = path.GetNodeRatation(selectedNode, Space.Self).eulerAngles;
                        lookTangent = path.IsNodeLookTangent(selectedNode);

                        eulerAngles.x = EditorGUILayout.FloatField("X", eulerAngles.x);
                        eulerAngles.y = EditorGUILayout.FloatField("Y", eulerAngles.y);
                        eulerAngles.z = EditorGUILayout.FloatField("Z", eulerAngles.z);
                        lookTangent = EditorGUIKit.IndentedToggleButton("Look Tangent", lookTangent);

                        if (scope.changed)
                        {
                            path.SetNodeRatation(selectedNode, Quaternion.Euler(eulerAngles), Space.Self);
                            path.SetNodeLookTangent(selectedNode, lookTangent);
                        }
                    }
                }
            }


            protected override void OnRotateToolSceneGUI(CardinalPathWithRotation path)
            {
                int count = path.nodeCount;
                for (int i = 0; i < count; i++)
                {
                    var position = path.GetNodePosition(i);
                    var rotation = path.GetNodeRatation(i);

                    float size = HandleUtility.GetHandleSize(position);

                    Handles.color = new Color(0.6f, 1f, 0.3f);
                    Handles.ArrowHandleCap(0, position, rotation, size, EventType.Repaint);
                    HandlesKit.DrawAALine(position, position + rotation * Vector3.up * size);

                    if (selectedNode == i)
                    {
                        using (var scope = new ChangeCheckScope(path))
                        {
                            if (path.IsNodeLookTangent(i))
                            {
                                rotation = Handles.Disc(rotation, position, rotation * Vector3.forward, size, false, 0.01f);
                            }
                            else
                            {
                                rotation = Handles.RotationHandle(rotation, position);
                            }
                            if (scope.changed) path.SetNodeRatation(i, rotation);
                        }
                    }
                    else
                    {
                        Handles.color = capNormalColor;
                        if (Handles.Button(position, Quaternion.identity, size * capSize, size * capSize, Handles.DotHandleCap))
                        {
                            selectedNode = i;
                        }
                    }
                }
            }
        }
    }

} // namespace UnityExtensions

#endif