using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Code adapted from https://www.youtube.com/watch?v=F-3nxJ2ANXg
public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

   public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        foreach(var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }
        }
        _nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
