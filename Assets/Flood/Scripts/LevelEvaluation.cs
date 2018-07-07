using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Flood
{
    using System.Text;

    using HoloToolkit.Unity;

    public class LevelEvaluation : SingleInstance<LevelEvaluation>
    {
        private EvaluationResult _result;

        public int Connected;
        public int Dropped;
        public int Points;

        private StartPipe[] _pipes;
        private VisualClock _clock;

        public TextMesh StatsText;
        public TextMesh StatsTitle;

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

            if (_evaluated)
            {
                return;
            }

            _result = Evaluate();

            if (_clock.RemainingTime < 0)
            {
                Finish();
            }

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"{PrefixConnected}{Space}{Connected}");
            strBuilder.AppendLine($"{PrefixDropped}{Space}{Dropped}");
            strBuilder.AppendLine($"{PrefixPoints}{Space}{Points}");
            strBuilder.AppendLine();
            strBuilder.AppendLine($"{PrefixOpenPipes}{Space}{_result.OpenEnds.Count}");
            strBuilder.AppendLine($"{PrefixMissingsEnds}{Space}{_result.MissingEnds.Count}");
            StatsText.text = strBuilder.ToString();
        }
        
        public struct EvaluationResult
        {
            public List<StartPipe.OpenPipeResult> OpenEnds;

            public List<EndPipe> MissingEnds;

            public float ReminingTime;
        }

        public void Finish()
        {
            StatsTitle.text += " - Finished";
            _evaluated = true;
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
                ReminingTime = _clock.RemainingTime
            };
        }
    }
}