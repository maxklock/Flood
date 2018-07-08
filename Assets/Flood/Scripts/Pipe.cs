using UnityEngine;

namespace Flood
{
    using System;
    using System.Linq;

    using HoloToolkit.Unity;

    public class Pipe : MonoBehaviour
    {
        public PipeEnd[] Ends;

        protected PipeParticle[] _particles;

        private static PipeEnd RotateEndsZ(float euler, PipeEnd end)
        {
            for (var z = 1; z < (euler + 360) % 360; z += 90)
            {
                switch (end)
                {
                    case PipeEnd.Top:
                        end = PipeEnd.Left;
                        break;
                    case PipeEnd.Bottom:
                        end = PipeEnd.Right;
                        break;
                    case PipeEnd.Left:
                        end = PipeEnd.Bottom;
                        break;
                    case PipeEnd.Right:
                        end = PipeEnd.Top;
                        break;
                }
            }
            return end;
        }

        private static PipeEnd RotateEndsX(float euler, PipeEnd end)
        {
            for (var x = 1; x < (euler + 360) % 360; x += 90)
            {
                switch (end)
                {
                    case PipeEnd.Top:
                        end = PipeEnd.Front;
                        break;
                    case PipeEnd.Bottom:
                        end = PipeEnd.Back;
                        break;
                    case PipeEnd.Front:
                        end = PipeEnd.Bottom;
                        break;
                    case PipeEnd.Back:
                        end = PipeEnd.Top;
                        break;
                }
            }
            return end;
        }

        private static PipeEnd RotateEndsY(float euler, PipeEnd end)
        {
            for (var y = 1; y < (euler + 360) % 360; y += 90)
            {
                switch (end)
                {
                    case PipeEnd.Left:
                        end = PipeEnd.Front;
                        break;
                    case PipeEnd.Right:
                        end = PipeEnd.Back;
                        break;
                    case PipeEnd.Front:
                        end = PipeEnd.Right;
                        break;
                    case PipeEnd.Back:
                        end = PipeEnd.Left;
                        break;
                }
            }
            return end;
        }

        public void UpdateEnds(Vector3 euler)
        {
            for (var i = 0; i < Ends.Length; i++)
            {
                Ends[i] = RotateEndsZ(euler.z, Ends[i]);
                Ends[i] = RotateEndsX(euler.x, Ends[i]);
                Ends[i] = RotateEndsY(euler.y, Ends[i]);
            }

            foreach (var particle in _particles)
            {
                particle.End = RotateEndsZ(euler.z, particle.End);
                particle.End = RotateEndsX(euler.x, particle.End);
                particle.End = RotateEndsY(euler.y, particle.End);
            }
        }

        // Use this for initialization
        void Start()
        {
            _particles = GetComponentsInChildren<PipeParticle>();
        }

        public void SetOpenEnd(PipeEnd end)
        {
            if (_particles == null)
            {
                _particles = GetComponentsInChildren<PipeParticle>();
                if (_particles == null)
                {
                    return;
                }
            }
            foreach (var particle in _particles.Where(p => p.End == end))
            {
                particle.Particle.Play();
            }
        }

        public void SetClosedEnd(PipeEnd end)
        {
            if (_particles == null)
            {
                _particles = GetComponentsInChildren<PipeParticle>();
                if (_particles == null)
                {
                    return;
                }
            }
            foreach (var particle in _particles.Where(p => p.End == end))
            {
                particle.Particle.Stop();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }

    public enum PipeEnd
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }
}