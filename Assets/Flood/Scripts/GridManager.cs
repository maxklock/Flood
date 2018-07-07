namespace Flood
{
    using System.Collections;
    using System.Collections.Generic;

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
            if (state == GridPositionState.GRID_CELL)
                return (_mainGrid[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] == null);
            else
                return (_mainGrid[(int) (gridPosition.x/GridScale), (int)(gridPosition.y / GridScale), (int)(gridPosition.z / GridScale)] == null);
        }

        public PlaceableObject GetCell(Vector3 gridPosition, GridPositionState state)
        {
            if (state == GridPositionState.GRID_CELL)
                return (_mainGrid[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z]);
            else
                return (_mainGrid[(int)(gridPosition.x / GridScale), (int)(gridPosition.y / GridScale), (int)(gridPosition.z / GridScale)]);
        }

        public void SetCell(PlaceableObject obj)
        {
            var coords = (obj.transform.position - WorldOffset) / GridScale;
            _mainGrid[(int)(coords.x), (int)(coords.y), (int)(coords.z)] = obj;
            obj.transform.position = (new Vector3((int)coords.x, (int)coords.y, (int)coords.z) * GridScale) + WorldOffset + Vector3.one * (GridScale / 2);
            obj.transform.rotation = Quaternion.identity; // ToDo FixMe
        }

        public void SetCell(PlaceableObject obj, Vector3 gridPosition, GridPositionState state)
        {
            if (state == GridPositionState.GRID_CELL)
                _mainGrid[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = obj;
            else
            {
                var coords = gridPosition / GridScale;
                _mainGrid[(int)(coords.x), (int)(coords.y), (int)(coords.z)] = obj;
            }
        }

        public bool SetCellTry(PlaceableObject obj, Vector3 gridPosition, GridPositionState state)
        {
            if (state == GridPositionState.GRID_CELL)
            {
                if (IsCellFree(gridPosition, state))
                {
                    _mainGrid[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = obj;
                    return true;
                }
                else return false;

            }
            else
            {
                if (IsCellFree(gridPosition, state))
                {
                    _mainGrid[(int)(gridPosition.x / GridScale), (int)(gridPosition.y / GridScale), (int)(gridPosition.z / GridScale)] = obj;
                    return true;
                }
                else  return false;
            }
        }



    }

    public enum GridPositionState
    {
        GRID_CELL,
        REAL_WORLD
    }
}