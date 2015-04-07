using System.Collections.Generic;

namespace eDriven.TextureUtilities
{
    /// <summary>
    /// The class that sets textures on materials of the renderer
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class TextureArranger
    {
        #region Members

        private readonly Dictionary<int, TextureDescriptor> _textureDescriptors = new Dictionary<int, TextureDescriptor>();
        /// <summary>
        /// Texture descriptors
        /// </summary>
        public Dictionary<int, TextureDescriptor> TextureDescriptors
        {
            get { return _textureDescriptors; }
        }

        private readonly UnityEngine.Renderer _renderer;
        public UnityEngine.Renderer Renderer
        {
            get { return _renderer; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="renderer">A renderer to arrange textures to</param>
        public TextureArranger(UnityEngine.Renderer renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// Is this position occupied?
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasTexture(int index)
        {
            return _textureDescriptors.ContainsKey(index);
        }

        /// <summary>
        /// Adds a texture to the material specified by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="descriptor"></param>
        public void AddTexture(int index, TextureDescriptor descriptor)
        {
            _textureDescriptors.Add(index, descriptor); // add descriptor
            _renderer.materials[index].mainTexture = descriptor.Texture; // set texture
            _renderer.materials[index].mainTexture.name = descriptor.Path; // name the texture
        }
        
    }
}
