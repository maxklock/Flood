using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Flood
{
    public class LevelEvaluation : MonoBehaviour
    {
        private StartPipe[] _pipes;
        private VisualClock _clock;

        private bool _evaluated;

        // Use this for initialization
        void Start()
        {
            _pipes = FindObjectsOfType<StartPipe>();
            _clock = FindObjectOfType<VisualClock>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_evaluated)
            {
                return;
            }

            if (_clock.RemainingTime < 0)
            {
                Evaluate();
            }
        }

        public void Evaluate()
        {
            var results = _pipes.Select(p => p.EvaluatePipes()).ToList();
            var result = new
            {
                OpenEnds = results.Select(r => r.OpenPipes).Aggregate(
                    new List<StartPipe.OpenPipeResult>(),
                    (res, list) =>
                    {
                        res.AddRange(list);
                        return res;
                    }),
                MissingEnds = results.Select(r => r.MissingEndPipes).Aggregate(
                    new List<EndPipe>(),
                    (res, list) =>
                    {
                        res.AddRange(list);
                        return res;
                    }),
            };

            Debug.Log($"You have {result.OpenEnds.Count} open pipes and {result.MissingEnds.Count} ends are missing.");

            _evaluated = true;
        }
    }
}