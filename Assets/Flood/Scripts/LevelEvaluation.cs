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

        public int TabSpace = 2;

        private const string PrefixConnected = "Connected Pipes\t";
        private const string PrefixDropped = "Dropped Pipes\t";
        private const string PrefixPoints = "Points\t\t\t";
        private const string PrefixOpenPipes = "Open Pipes\t\t";
        private const string PrefixMissingsEnds = "Missing Ends\t";


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
                Finish();
            }
            DrawText();
        }

        private void DrawText()
        {
            var space = new String('\t', TabSpace);
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"{PrefixConnected}{space}{Connected}");
            strBuilder.AppendLine($"{PrefixDropped}{space}{Dropped}");
            strBuilder.AppendLine($"{PrefixPoints}{space}{Points}");
            strBuilder.AppendLine();
            strBuilder.AppendLine($"{PrefixOpenPipes}{space}{_result.OpenEnds.Count}");
            strBuilder.AppendLine($"{PrefixMissingsEnds}{space}{_result.MissingEnds.Count}");
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
            StatsTitle.text = "Finished";
            Points += (int)_clock.RemainingTime;
            _evaluated = true;

            DrawText();
        }

        public void Evaluate()
        {
            var results = _pipes.Select(p => p.EvaluatePipes()).ToList();
            _result = new EvaluationResult
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