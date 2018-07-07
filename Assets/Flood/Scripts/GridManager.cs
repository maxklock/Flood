namespace Flood
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using HoloToolkit.Unity;

    using UnityEngine;

    public class GridManager : SingleInstance<GridManager>
    {
        public Vector3 GridDimensions = new Vector3(50,50,50);

        public Vector3 GridOffset => new Vector3(GridDimensions.x, 0,GridDimensions.z) / -2;
        public Vector3 WorldOffset => GridOffset * GridScale;
        public float GridScale = 0.25f;

        private GameObject[,,] _mainGrid;

        // Use this for initialization
        void Awake () {
            _mainGrid = new GameObject[(int)GridDimensions.x, (int)GridDimensions.y, (int)GridDimensions.z];

        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public bool IsCellFree(Vector3 gridPosition, GridPositionState state)
        {
            return GetCell(gridPosition, state) == null;
        }

        public GameObject GetCell(Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = WorldToGrid(gridPosition);
            }

            if (IsOutOfRange(coords))
            {
                return null;
            }

            return (_mainGrid[(int)coords.x, (int)coords.y, (int)coords.z]);
        }

        private static Vector3 ClampToAxis(Vector3 euler)
        {
            var vec = Vector3.zero;
            vec.x = Mathf.Round(euler.x / 90) * 90;
            vec.y = Mathf.Round(euler.y / 90) * 90;
            vec.z = Mathf.Round(euler.z / 90) * 90;
            return vec;
        }

        private bool IsOutOfRange(Vector3 coords)
        {
            var ints = new Vector3Int((int)coords.x, (int)coords.y, (int)coords.z);
        
            var x = ints.x < 0 || ints.x >= _mainGrid.GetLength(0);
            var y = ints.y < 0 || ints.y >= _mainGrid.GetLength(1);
            var z = ints.z < 0 || ints.z >= _mainGrid.GetLength(2);

            return x || y || z;
        }
    
        public void SetCell(GameObject obj, Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = WorldToGrid(gridPosition);
            }

            _mainGrid[(int)(coords.x), (int)(coords.y), (int)(coords.z)] = obj;
            ApplyGridTransform(obj, coords);
        }

        public void ApplyGridTransform(GameObject obj, Vector3 coords)
        {
            obj.transform.position = GridToWorld(coords);
            obj.transform.rotation = Quaternion.Euler(ClampToAxis(obj.transform.eulerAngles));
        }

        public Vector3 WorldToGrid(Vector3 pos)
        {
            var tmp = (pos - WorldOffset) / GridScale;
            return new Vector3((int)tmp.x, (int)tmp.y, (int)tmp.z);
        }

        public Vector3 GridToWorld(Vector3 coords)
        {
            return (new Vector3((int)coords.x, (int)coords.y, (int)coords.z) * GridScale) + WorldOffset + Vector3.one * (GridScale / 2);
        }

        public void SetCellByCollider(GameObject obj, params Collider[] colliders)
        {
            for (var x = 0; x <= GridDimensions.x; x++)
            {
                for (var y = 0; y <= GridDimensions.y; y++)
                {
                    for (var z = 0; z <= GridDimensions.z; z++)
                    {
                        var point = GridToWorld(new Vector3(x, y, z));
                        if (colliders.Any(c => c.bounds.Contains(point)))
                        {
                            _mainGrid[x, y, z] = obj;
                        }
                    }
                }
            }
        }

        public bool SetCellTry(GameObject obj, Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = WorldToGrid(gridPosition);
            }

            if (IsOutOfRange(coords))
            {
                return false;
            }

            if (!IsCellFree(gridPosition, state))
            {
                return false;
            }

            _mainGrid[(int)coords.x, (int)coords.y, (int)coords.z] = obj;
            ApplyGridTransform(obj, coords);
            return true;
        }



    }

    public enum GridPositionState
    {
        GRID_CELL,
        REAL_WORLD
    }
}