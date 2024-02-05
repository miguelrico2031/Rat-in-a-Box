using UnityEngine;
using UnityEngine.Tilemaps;


public interface IPathfinder
{
    public TileBase GetNextTile(TileBase currentTile);
}