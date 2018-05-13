/*
 * Dis code was written by 
 * 
 * Void Chicken
 * 
 * Give credit plz
 * 
 * Much tanks
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VoidChicken.Terrain
{
    public class VoidTerrain : MonoBehaviour
    {

        public GameObject terrainBlock;

        public int width = 0;

        public float noiseScale = 0f;
        public float noiseAmplitude = 0f;
        public AnimationCurve noiseRemap;

        private int[] heights;


        public float blockHeight = 0f;



        float GenerateHeight(float x)
        {
            return Mathf.Floor(noiseRemap.Evaluate(Mathf.PerlinNoise(x * noiseAmplitude, 0)) * noiseScale);
        }

        public void Generate()
        {
            heights = new int[width];

            for (int i = 0; i < width; i++)
            {
                int height = (int)GenerateHeight(i);
                for (int j = 0; j < height; j++) {
                    Instantiate(terrainBlock, new Vector3(i, j * blockHeight, 0), Quaternion.identity, transform);
                }
            }
        }

    }
}
