using UnityEngine;

namespace eDriven.TextureUtilities
{
    public class TextureDescriptor
    {
        /// <summary>
        /// The material index (optional)
        /// </summary>
        public int Index;

        /// <summary>
        /// The path to load the texture from
        /// </summary>
        public string Path;

        /// <summary>
        /// Target texture width
        /// </summary>
        public int Width;

        /// <summary>
        /// Target texture height
        /// </summary>
        public int Height;

        /// <summary>
        /// Compression
        /// </summary>
        public bool Compressed;

        /// <summary>
        /// Mipmaps generation
        /// </summary>
        public bool DoGenerateMipMaps;

        /// <summary>
        /// Returned texture
        /// </summary>
        public Texture2D Texture;

        public TextureDescriptor(int index, string path, bool compressed, bool doGenerateMipMaps)
        {
            Index = index;
            Path = path;
            Compressed = compressed;
            DoGenerateMipMaps = doGenerateMipMaps;
        }

        public TextureDescriptor(int index, string path, int width, int height, bool doCompressTexture, bool doGenerateMipMaps)
            : this(index, path, doCompressTexture, doGenerateMipMaps)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return string.Format("Index: {0}, TexturePath: {1}, DoCompressTexture: {2}, DoGenerateMipMaps: {3}", Index, Path, Compressed, DoGenerateMipMaps);
        }
    }
}