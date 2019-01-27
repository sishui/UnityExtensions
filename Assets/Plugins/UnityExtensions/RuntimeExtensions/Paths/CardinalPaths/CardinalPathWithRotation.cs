using System;
using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// Cardinal 路径节点 (with Rotation)
    /// </summary>
    [Serializable]
    public class CardinalNodeWithRotation : CardinalNode
    {
        public Quaternion rotation;
        public bool lookTangent;


        public void SetRotation(Path path, int nodeIndex, Quaternion value)
        {
            if (lookTangent)
            {
                var loc = new Path.Location(nodeIndex, 0);
                if (!path.circular && nodeIndex == path.segmentCount) loc.Set(nodeIndex - 1, 1);
                rotation = Quaternion.LookRotation(path.GetTangent(loc, Space.Self), value * Vector3.up);
            }
            else
            {
                rotation = value.normalized;
            }
        }


        public void SetLookTangent(Path path, int nodeIndex, bool value)
        {
            lookTangent = value;
            if (value)
            {
                SetRotation(path, nodeIndex, rotation);
            }
        }
    }


    /// <summary>
    /// 带旋转控制的 Cardinal 路径
    /// </summary>
    [AddComponentMenu("Unity Extensions/Path/Cardinal Path (with Rotation)")]
    public partial class CardinalPathWithRotation : CardinalPath<CardinalNodeWithRotation>
    {
        public override void Reset()
        {
            base.Reset();
            node(0).lookTangent = true;
            node(1).lookTangent = true;
            node(0).rotation = Quaternion.identity;
            node(1).rotation = Quaternion.identity;
        }


        public Quaternion GetNodeRatation(int nodeIndex, Space space = Space.World)
        {
            if (space == Space.Self) return node(nodeIndex).rotation;
            else return TransformRotation(node(nodeIndex).rotation);
        }


        public void SetNodeRatation(int nodeIndex, Quaternion rotation, Space space = Space.World)
        {
            if (space == Space.World) rotation = InverseTransformRotation(rotation);
            node(nodeIndex).SetRotation(this, nodeIndex, rotation);
        }


        public bool IsNodeLookTangent(int nodeIndex)
        {
            return node(nodeIndex).lookTangent;
        }


        public void SetNodeLookTangent(int nodeIndex, bool lookTangent)
        {
            node(nodeIndex).SetLookTangent(this, nodeIndex, lookTangent);
        }


        public override void InsertNode(int nodeIndex)
        {
            base.InsertNode(nodeIndex);

            if (circular || (nodeIndex > 0 && nodeIndex < nodeCount - 1))
            {
                node(nodeIndex).lookTangent = circularNode(nodeIndex - 1).lookTangent;
                node(nodeIndex).SetRotation(this, nodeIndex,
                    Quaternion.SlerpUnclamped(circularNode(nodeIndex - 1).rotation, circularNode(nodeIndex + 1).rotation, 0.5f));
            }
            else
            {
                if (nodeIndex == 0)
                {
                    node(0).lookTangent = node(1).lookTangent;
                    node(nodeIndex).SetRotation(this, nodeIndex, node(1).rotation);
                }
                else
                {
                    node(nodeIndex).lookTangent = node(nodeIndex-1).lookTangent;
                    node(nodeIndex).SetRotation(this, nodeIndex, node(nodeIndex - 1).rotation);
                }
            }
        }


        public override bool RemoveNode(int nodeIndex)
        {
            return base.RemoveNode(nodeIndex);
        }


        protected override void SetCircular(bool circular)
        {
            base.SetCircular(circular);
        }


        public override void SetNodePosition(int nodeIndex, Vector3 position, Space space = Space.World)
        {
            base.SetNodePosition(nodeIndex, position, space);
        }
    }

} // namespace UnityExtensions