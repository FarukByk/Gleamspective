using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSpawner : MonoBehaviour
{
    public Tilemap tilemap;        // Hedef Tilemap
    public Tile targetTile;        // Spawn edilecek tile türü
    public GameObject prefab;      // Yerleştirilecek prefab

    void Start()
    {

        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int cellPosition = new Vector3Int(pos.x, pos.y, 0);
            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile == targetTile) // Eğer belirli bir Tile ise
            {
                Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);
                Instantiate(prefab, worldPosition, Quaternion.identity);
                tilemap.SetTile(cellPosition, null);
            }
        }
    }
}