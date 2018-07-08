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
        public EndPipe[] EndPipes;

        public class EvaluationResult
        {
            public List<OpenPipeResult> OpenPipes { get; set; }
            public List<EndPipe> MissingEndPipes { get; set; }
        }

        public class OpenPipeResult
        {
            public Pipe Pipe { get; set; }
            public PipeEnd Direction { get; set; }
        }

        // Use this for initialization
        private void Start()
        {
            GridManager.Instance.SetCell(gameObject, transform.position, GridPositionState.REAL_WORLD);
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

        public EvaluationResult EvaluatePipes()
        {
            var pipesProcess = new Queue<Pipe>();
            var pipesDone = new List<Pipe>();
            var pipesOpen = new List<OpenPipeResult>();
            var pipesEnds = new List<EndPipe>();

            pipesProcess.Enqueue(this);

            while (pipesProcess.Any())
            {
                var currentPipe = pipesProcess.Dequeue();
                pipesDone.Add(currentPipe);

                foreach (var end in currentPipe.Ends)
                {
                    var isEnabled = currentPipe.IsParticleEnabled(end);
                    var obj = GridManager.Instance.GetCell(GridManager.Instance.WorldToGrid(currentPipe.transform.position) + PipeEndToVector3(end), GridPositionState.GRID_CELL);
                    var pipe = obj?.GetComponent<Pipe>();
                    if (pipe == null)
                    {
                        if (!isEnabled)
                        {
                            currentPipe.SetOpenEnd(end);
                        }
                        if (pipesOpen.All(o => o.Pipe != currentPipe))
                            pipesOpen.Add(new OpenPipeResult
                            {
                                Pipe = currentPipe,
                                Direction = end
                            });
                        continue;
                    }

                    if (isEnabled)
                    {
                        currentPipe.SetClosedEnd(end);
                    }

                    if (pipesDone.Contains(pipe))
                        continue;

                    var endPipe = pipe as EndPipe;
                    if (endPipe != null)
                    {
                        pipesEnds.Add(endPipe);
                        continue;
                    }
                    pipesProcess.Enqueue(pipe);
                }
                
            }

            Debug.Log($"{pipesOpen.Count}");

            return new EvaluationResult
            {
                OpenPipes = pipesOpen,
                MissingEndPipes = EndPipes.Where(p => !pipesEnds.Contains(p)).ToList()
            };
        }
    }
}