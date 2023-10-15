using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public bool autoUpdate;
    public DrawMode drawMode;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;
    public Material material;

    [Range(0, 6)]
    public int editorPreviewLOD;

    float[,] falloffMap;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public enum DrawMode
    {
        NoiseMap,
        FalloffMap,
        Mesh
    }

    private void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    private void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(material);
    }

    public int mapChunkSize
    {
        get
        {
            if (terrainData.useFlatShading)
                return 95;
            else
                return 239;
        }
    }

    public void DrawMapInEditor()
    {
        MapData data = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(data.heightMap));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(
                data.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading));
        else if (drawMode == DrawMode.FalloffMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(falloffMap));
    }

    public void RequestMapData(Vector2 centre, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate {
            MapDataThread(centre, callback);
        };
        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 centre, Action<MapData> callback)
    {
        MapData data = GenerateMapData(centre);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, data));
        }
    }

    public void RequestMeshData(MapData mapData, int levelOfDetail, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate {
            MeshDataThread(mapData, levelOfDetail, callback);
        };
        new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int levelOfDetail, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(
            mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail, terrainData.useFlatShading);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private MapData GenerateMapData(Vector2 centre)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.scale, noiseData.octaves, noiseData.persistance,
            noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

        if (terrainData.useFalloffMap)
        {
            if (falloffMap == null)
                falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize + 2);

            for (int y = 0; y < mapChunkSize+2; y++)
            {
                for (int x = 0; x < mapChunkSize+2; x++)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
            }
        }

        return new MapData(noiseMap);
    }

    private void OnValidate()
    {
        if (terrainData != null)
        {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }
        if (noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
}