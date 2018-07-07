using UnityEngine;

namespace Flood
{
    using System;

    using HoloToolkit.Unity;

    public class Pipe : MonoBehaviour
    {
        public PipeEnd[] Ends;

        public void UpdateEnds(Vector3 euler)
        {
            for (var i = 0; i < Ends.Length; i++)
            {
                for (var z = 1; z < (euler.z + 360) % 360; z += 90)
                {
                    switch (Ends[i])
                    {
                        case PipeEnd.Top:
                            Ends[i] = PipeEnd.Left;
                            break;
                        case PipeEnd.Bottom:
                            Ends[i] = PipeEnd.Right;
                            break;
                        case PipeEnd.Left:
                            Ends[i] = PipeEnd.Bottom;
                            break;
                        case PipeEnd.Right:
                            Ends[i] = PipeEnd.Top;
                            break;
                    }
                }

                for (var x = 1; x < (euler.x + 360) % 360; x += 90)
                {
                    switch (Ends[i])
                    {
                        case PipeEnd.Top:
                            Ends[i] = PipeEnd.Front;
                            break;
                        case PipeEnd.Bottom:
                            Ends[i] = PipeEnd.Back;
                            break;
                        case PipeEnd.Front:
                            Ends[i] = PipeEnd.Bottom;
                            break;
                        case PipeEnd.Back:
                            Ends[i] = PipeEnd.Top;
                            break;
                    }
                }

                for (var y = 1; y < (euler.y + 360) % 360; y += 90)
                {
                    switch (Ends[i])
                    {
                        case PipeEnd.Left:
                            Ends[i] = PipeEnd.Front;
                            break;
                        case PipeEnd.Right:
                            Ends[i] = PipeEnd.Back;
                            break;
                        case PipeEnd.Front:
                            Ends[i] = PipeEnd.Right;
                            break;
                        case PipeEnd.Back:
                            Ends[i] = PipeEnd.Left;
                            break;
                    }
                }
            }
        }

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
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
