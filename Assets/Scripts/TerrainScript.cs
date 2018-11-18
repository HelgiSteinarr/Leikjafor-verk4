using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour {

    public Terrain terrain;

    private int heightMapWidth;
    private int heightMapHeight;
    private float[,] originalHeights;

    private void Start()
    {
        heightMapWidth = terrain.terrainData.heightmapWidth;
        heightMapHeight = terrain.terrainData.heightmapHeight;
        originalHeights = terrain.terrainData.GetHeights(
                 0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
    }

    private void OnDestroy()
    {
        terrain.terrainData.SetHeights(0, 0, this.originalHeights);
    }

    public void CreateHole(Vector3 position, float holeHeight = 0.005f, int size = 50)
    {
        position.x += 5;
        Vector3 tempCoord = (position - terrain.gameObject.transform.position);
        Vector3 coord = new Vector3(
            tempCoord.x / terrain.terrainData.size.x,
            tempCoord.y / terrain.terrainData.size.y,
            tempCoord.z / terrain.terrainData.size.z
        );

        Vector2Int positionInTerrain = PositionInTerrain(coord);

        int offset = size / 2;

        object coroutineParams = new object[] { holeHeight, size, positionInTerrain, offset };

        StartCoroutine("UpdateHole", coroutineParams);
    }

    private IEnumerator UpdateHole(object[] parameters)
    {
        // Params
        float holeHeight = (float) parameters[0];
        int size = (int) parameters[1];
        Vector2Int positionInTerrain = (Vector2Int)parameters[2]; //new Vector2Int((int)parameters[2], (int)parameters[3]);
        int offset = (int) parameters[3];

        //
        float[,] heights = terrain.terrainData.GetHeights(positionInTerrain.x - offset, positionInTerrain.y - offset, size, size);
        float currentHeight = 0.2f;
        while (currentHeight > holeHeight)
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    if (!InCircle(x, y, size)) continue;
                    heights[x, y] = currentHeight;
                }

            currentHeight -= Time.deltaTime / 5;

            terrain.terrainData.SetHeights(positionInTerrain.x - offset, positionInTerrain.y - offset, heights);

            yield return new WaitForSeconds(0.1f);
        }
    }


    //

    private Vector2Int PositionInTerrain(Vector3 coord)
        => new Vector2Int((int)(coord.x * heightMapWidth), (int)(coord.z * heightMapHeight));

    private bool InCircle(int x, int y, int size)
    {
        Vector2Int[] corners = new Vector2Int[4] { new Vector2Int(0, 0), new Vector2Int(0, size), new Vector2Int(size, size), new Vector2Int(size, 0) };
        float distanceToNearestCorner = -1;
        foreach (Vector2Int corner in corners)
        {
            float newDistance = Vector2Int.Distance(new Vector2Int(x, y), corner); // Mathf.Abs((x+y) / 2 - (corner.x+corner.y)/2);
            if (newDistance < distanceToNearestCorner || distanceToNearestCorner == -1)
            {
                distanceToNearestCorner = newDistance;
            }
        }
        return distanceToNearestCorner > 10.0f;
    }
}
