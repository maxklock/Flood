using UnityEngine;

namespace Flood
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Flood.Utils;

    [RequireComponent(typeof(PlaceableObject))]
    public class StartPipe : Pipe
    {
        public class OpenPipeResult
        {
            public Pipe Pipe { get; set; }
            public PipeEnd Direction { get; set; }
        }

        // Use this for initialization
        private void Start()
        {
            GridManager.Instance.SetCell(GetComponent<PlaceableObject>(), transform.position, GridPositionState.REAL_WORLD);
        }

        // Update is called once per frame
        private void Update()
        {
            if (AxisToButtonUtil.Instance.IsDown("XBOX_RIGHT_BUMPER"))
            {
                var openEnds = EvaluatePipes();
                foreach (var ends in openEnds)
                {
                    ends.Pipe.GetComponentInChildren<Renderer>().material.color = Color.yellow;
                }
            }
        }

        private static Vector3 PipeEndToVector3(PipeEnd end)
        {
            switch (end)
            {
                case PipeEnd.Top:
                    return Vector3.up;
                case PipeEnd.Bottom:
                    return Vector3.down;
                case PipeEnd.Left:
                    return Vector3.left;
                case PipeEnd.Right:
                    return Vector3.right;
                case PipeEnd.Front:
                    return Vector3.forward;
                case PipeEnd.Back:
                    return Vector3.back;
                default:
                    throw new ArgumentOutOfRangeException(nameof(end), end, null);
            }
        }

        private IEnumerable<OpenPipeResult> EvaluatePipes()
        {
            var pipesProcess = new Queue<Pipe>();
            var pipesDone = new List<Pipe>();
            var pipesOpen = new List<OpenPipeResult>();

            pipesProcess.Enqueue(this);

            while (pipesProcess.Any())
            {
                var currentPipe = pipesProcess.Dequeue();
                pipesDone.Add(currentPipe);

                foreach (var end in currentPipe.Ends)
                {
                    var obj = GridManager.Instance.GetCell(GridManager.Instance.WorldToGrid(currentPipe.transform.position) + PipeEndToVector3(end), GridPositionState.GRID_CELL);
                    var pipe = obj?.GetComponent<Pipe>();
                    if (pipe == null)
                    {
                        if (pipesOpen.All(o => o.Pipe != currentPipe))
                            pipesOpen.Add(new OpenPipeResult
                            {
                                Pipe = currentPipe,
                                Direction = end
                            });
                        continue;
                    }

                    if (pipesDone.Contains(pipe))
                        continue;
                    pipesProcess.Enqueue(pipe);
                }
                
            }

            return pipesOpen;
        }
    }
}