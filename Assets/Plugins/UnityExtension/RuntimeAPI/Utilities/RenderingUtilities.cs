using UnityEngine;

namespace UnityExtension
{
    /// <summary>
    /// Unity Rendering 实用工具
    /// </summary>
    public partial struct Utilities
    {
        static Texture2D _whiteTexture;
        static Texture2D _blackTexture;
        static Cubemap _whiteCubemap;
        static Cubemap _blackCubemap;
        static Mesh _quadMesh;


        public static Texture2D whiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }


        public static Texture2D blackTexture
        {
            get
            {
                if (_blackTexture == null)
                {
                    _blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    _blackTexture.SetPixel(0, 0, Color.black);
                    _blackTexture.Apply();
                }

                return _blackTexture;
            }
        }


        public static Cubemap whiteCubemap
        {
            get
            {
                if (_whiteCubemap == null)
                {
                    _whiteCubemap = new Cubemap(1, TextureFormat.ARGB32, false);
                    _whiteCubemap.SetPixel(CubemapFace.NegativeX, 0, 0, Color.white);
                    _whiteCubemap.SetPixel(CubemapFace.NegativeY, 0, 0, Color.white);
                    _whiteCubemap.SetPixel(CubemapFace.NegativeZ, 0, 0, Color.white);
                    _whiteCubemap.SetPixel(CubemapFace.PositiveX, 0, 0, Color.white);
                    _whiteCubemap.SetPixel(CubemapFace.PositiveY, 0, 0, Color.white);
                    _whiteCubemap.SetPixel(CubemapFace.PositiveZ, 0, 0, Color.white);
                    _whiteCubemap.Apply();
                }
                return _whiteCubemap;
            }
        }


        public static Cubemap blackCubemap
        {
            get
            {
                if (_blackCubemap == null)
                {
                    _blackCubemap = new Cubemap(1, TextureFormat.ARGB32, false);
                    _blackCubemap.SetPixel(CubemapFace.NegativeX, 0, 0, Color.black);
                    _blackCubemap.SetPixel(CubemapFace.NegativeY, 0, 0, Color.black);
                    _blackCubemap.SetPixel(CubemapFace.NegativeZ, 0, 0, Color.black);
                    _blackCubemap.SetPixel(CubemapFace.PositiveX, 0, 0, Color.black);
                    _blackCubemap.SetPixel(CubemapFace.PositiveY, 0, 0, Color.black);
                    _blackCubemap.SetPixel(CubemapFace.PositiveZ, 0, 0, Color.black);
                    _blackCubemap.Apply();
                }
                return _blackCubemap;
            }
        }


        public static Mesh quadMesh
        {
            get
            {
                if (!_quadMesh)
                {
                    var vertices = new[]
                    {
                        new Vector3(-0.5f, -0.5f, 0f),
                        new Vector3(0.5f,  0.5f, 0f),
                        new Vector3(0.5f, -0.5f, 0f),
                        new Vector3(-0.5f,  0.5f, 0f)
                    };

                    var uvs = new[]
                    {
                        new Vector2(0f, 0f),
                        new Vector2(1f, 1f),
                        new Vector2(1f, 0f),
                        new Vector2(0f, 1f)
                    };

                    var indices = new[] { 0, 1, 2, 1, 0, 3 };

                    _quadMesh = new Mesh
                    {
                        vertices = vertices,
                        uv = uvs,
                        triangles = indices
                    };

                    _quadMesh.RecalculateNormals();
                    _quadMesh.RecalculateBounds();
                }

                return _quadMesh;
            }
        }

    } // struct Utilities

} // namespace UnityExtension