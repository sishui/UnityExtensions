﻿#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityExtensions.Editor;
using System;

namespace UnityExtensions
{
    public partial class CardinalPathWithRotation
    {
        [SerializeField]
        bool _previewRotation = default;


        protected override Type floatingWindowType => typeof(CardinalPathWithRotationFloatingWindow);


        protected override void DrawLocalGizmos(ref Matrix4x4 matrix)
        {
            base.DrawLocalGizmos(ref matrix);

            if (_previewRotation)
            {
                ValidateSamples();

                int segmentCount = this.segmentCount;
                Location loc;
                Quaternion rot;
                Vector3 pos;

                Handles.color = previewRotationColor;

                for (loc.index = 0; loc.index < segmentCount; loc.index++)
                {
                    int count = Mathf.CeilToInt(node(loc.index).length);

                    for (int t = 0; t <= count; t++)
                    {
                        if (t == count && loc.index != segmentCount - 1) continue;

                        loc.time = (float)t / count;
                        rot = GetRotation(loc, Space.Self);
                        pos = GetPoint(loc, Space.Self);

                        Handles.ArrowHandleCap(0, pos, rot, 0.75f, EventType.Repaint);
                        HandlesKit.DrawAALine(pos, pos + rot * Vector3.up);
                    }
                }
            }
        }


        [CustomEditor(typeof(CardinalPathWithRotation))]
        new class Editor : Path.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                var path = target as CardinalPathWithRotation;

                using (var scope = new ChangeCheckScope(path))
                {
                    bool value = EditorGUIKit.IndentedToggleButton("Preview Rotation", path._previewRotation);
                    if (scope.changed) path._previewRotation = value;
                }
            }

        } // class Editor


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

                    Handles.color = handlesRotationColor;
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


        [ContextMenu("Convert to 'Cardinal Path'")]
        void Convert()
        {
            var path = Undo.AddComponent<CardinalPath>(gameObject);
            Path<CardinalNode>.Copy(path, this);
            Undo.DestroyObjectImmediate(this);
        }
    }

} // namespace UnityExtensions

#endif