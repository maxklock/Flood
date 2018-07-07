using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Flood
{
    using System.Text;

    public class LevelEvaluation : MonoBehaviour
    {
        public int Connected = 0;
        public int Dropped = 0;
        public int Points = 0;

        private StartPipe[] _pipes;
        private VisualClock _clock;

        public TextMesh StatsText;

        private bool _evaluated;

        private const string PrefixConnected = "Connected Pipes:\t\t";
        private const string PrefixDropped = "Dropped Pipes:\t\t";
        private const string PrefixPoints = "Points:\t\t\t\t\t\t";
        private const string PrefixOpenPipes = "Open Pipes:\t\t\t\t";
        private const string PrefixMissingsEnds = "Missing Ends:\t\t\t";

        private const string Space = "\t\t\t\t";

        // Use this for initialization
        void Start()
        {
            _pipes = FindObjectsOfType<StartPipe>();
            _clock = FindObjectOfType<VisualClock>();
        }

        // Update is called once per frame
        void Update()
        {
            var result = Evaluate();

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"{PrefixConnected}{Space}{Connected}");
            strBuilder.AppendLine($"{PrefixDropped}{Space}{Dropped}");
            strBuilder.AppendLine($"{PrefixPoints}{Space}{Points}");
            strBuilder.AppendLine();
            strBuilder.AppendLine($"{PrefixOpenPipes}{Space}{result.OpenEnds.Count}");
            strBuilder.AppendLine($"{PrefixMissingsEnds}{Space}{result.MissingEnds.Count}");
            StatsText.text = strBuilder.ToString();

            if (_evaluated)
            {
                return;
            }

            if (_clock.RemainingTime < 0)
            {
                Debug.Log($"You have {result.OpenEnds.Count} open pipes and {result.MissingEnds.Count} ends are missing.");
                _evaluated = true;
            }
        }

        public class EvaluationResult
        {
            public List<StartPipe.OpenPipeResult> OpenEnds { get; set; }
            public List<EndPipe> MissingEnds { get; set; }
        }

        public EvaluationResult Evaluate()
        {
            var results = _pipes.Select(p => p.EvaluatePipes()).ToList();
            return new EvaluationResult
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
        }
    }
}