using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

namespace Game.Textures
{
    public class TextureAtlasMapper
    {
        public Texture2D atlas;

        public Texture2D[] textures;

        public Rect[] rects;

        public Dictionary<string, Rect> mappedTextures;

        protected float individualHeight = 32.0f;

        protected float individualWidth = 32.0f;

        public TextureAtlasMapper()
        {
            UnityEngine.Object[] objects = Resources.LoadAll("textures") as UnityEngine.Object[];
            textures = new Texture2D[objects.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                textures[i] = (Texture2D) objects[i];
            }
            
            atlas = new Texture2D(2048, 2048);
            atlas.name = "myatlas";
            atlas.anisoLevel = 16;
            rects = atlas.PackTextures(textures, 0);
            mappedTextures = new Dictionary<string, Rect>();

            for (int i = 0; i < rects.Length; i++)
            {
                mappedTextures[textures[i].name] = rects[i];
            }
        }

        public float GetImageHeight()
        {
            return atlas.height;
        }

        public float GetImageWidth()
        {
            return atlas.width;
        }

        public Vector2[] GetMappedUVs(string textureName)
        {
            if (!mappedTextures.ContainsKey(textureName))
            {
                Debug.Log("missing entry: " + textureName);
            }

                Rect rect = mappedTextures[textureName];
            

            Vector2[] list = new Vector2[4];

            float u1 = rect.x + rect.width;
            float u2 = rect.x;
            float u3 = u2;
            float u4 = u1;

            float v1 = rect.y + rect.height;
            float v2 = v1;
            float v3 = rect.y;
            float v4 = v3;

            list.SetValue(new Vector2(u1, v1), 0);
            list.SetValue(new Vector2(u2, v2), 1);
            list.SetValue(new Vector2(u3, v3), 2);
            list.SetValue(new Vector2(u4, v4), 3);

            return list;
        }
    }
}
