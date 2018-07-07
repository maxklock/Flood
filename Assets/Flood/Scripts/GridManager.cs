namespace Flood
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using HoloToolkit.Unity;

    using UnityEngine;

    public class GridManager : Singleton<GridManager>
    {
        public Vector3 GridDimensions = new Vector3(50,50,50);

        public Vector3 GridOffset => new Vector3(GridDimensions.x, 0,GridDimensions.z) / -2;
        public Vector3 WorldOffset => GridOffset * GridScale;
        public float GridScale = 0.25f;

        private PlaceableObject[,,] _mainGrid;

        // Use this for initialization
        void Start () {
            _mainGrid = new PlaceableObject[(int)GridDimensions.x, (int)GridDimensions.y, (int)GridDimensions.z];

        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public bool IsCellFree(Vector3 gridPosition, GridPositionState state)
        {
            return GetCell(gridPosition, state) == null;
        }

        public PlaceableObject GetCell(Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = (gridPosition - WorldOffset) / GridScale;
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

        public void SetCell(PlaceableObject obj, Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = (gridPosition - WorldOffset) / GridScale;
            }

            _mainGrid[(int)(coords.x), (int)(coords.y), (int)(coords.z)] = obj;
            ApplyGridTransform(obj, coords);
        }

        private void ApplyGridTransform(PlaceableObject obj, Vector3 coords)
        {
            obj.transform.position = (new Vector3((int)coords.x, (int)coords.y, (int)coords.z) * GridScale) + WorldOffset + Vector3.one * (GridScale / 2);
            obj.transform.rotation = Quaternion.Euler(ClampToAxis(obj.transform.eulerAngles));
        }

        public bool SetCellTry(PlaceableObject obj, Vector3 gridPosition, GridPositionState state)
        {
            var coords = gridPosition;
            if (state == GridPositionState.REAL_WORLD)
            {
                coords = (gridPosition - WorldOffset) / GridScale;
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