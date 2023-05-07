/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUVViewerEditorWindow (version 1.1)
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace MyClasses.Tool
{
    public class MyUVViewerEditorWindow : EditorWindow
    {
        #region ----- Internal Class -----
    
        public class SubMesh
        {
            public Material Material;
            public int[] Indices = null;
        }

        #endregion

        #region ----- Variable -----

        private Mesh _mesh = null;
        private Renderer _renderer = null;
        private List<SubMesh> _subMeshes = new List<SubMesh>();
        private SubMesh _subMesh = null;
        private List<Vector2[]> _uvs = new List<Vector2[]>();
        private Material _material;

        private Color _backgroundColor = new Color(0, 0, 0, 0.05f);
        private Color _lineColor = new Color(1, 1, 1, 0.4f);
        private Color _frontFaceColor = new Color(0, 1, 0, 0.4f);
        private Color _backFaceColor = new Color(1, 0, 0, 0.4f);
        private bool _isDrawLine = true;
        private bool _isDrawFrontTriangle = true;
        private bool _isDrawBackTriangle = true;
        private int _curUVSet = 0;
        private float _zoom = 1f;

        #endregion

        #region ----- EditorWindow Implementation -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] UV Viewer");
            minSize = new Vector2(512, 768);
            
            _material = MyResourceManager.GetMaterialBlendedColor();
        }
        
        /// <summary>
        /// OnSelectionChange.
        /// </summary>
        void OnSelectionChange()
        {
            _mesh = null;
            try
            {
                GameObject gameObject = Selection.activeGameObject;
                if (gameObject == null)
                {
                    return;
                }

                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    _mesh = meshFilter.sharedMesh;
                }
                else
                {
                    SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer != null)
                    {
                        _mesh = skinnedMeshRenderer.sharedMesh;
                    }
                }

                if (_mesh == null)
                {
                    return;
                }

                _renderer = gameObject.GetComponent<Renderer>();
                if (_renderer == null)
                {
                    return;
                }

                Material[] materials = _renderer.sharedMaterials;
                _subMeshes.Clear();
                for (int i = 0, count = _mesh.subMeshCount; i < count; ++i)
                {
                    var subMesh = new SubMesh();
                    subMesh.Material = materials[i % materials.Length];
                    subMesh.Indices = _mesh.GetIndices(i);
                    _subMeshes.Add(subMesh);
                }
                _subMesh = _subMeshes[0];

                _uvs.Clear();
                _uvs.Add(_mesh.uv != null && _mesh.uv.Length > 0 ? _mesh.uv : null);
                _uvs.Add(_mesh.uv2 != null && _mesh.uv2.Length > 0 ? _mesh.uv2 : null);
                _uvs.Add(_mesh.uv3 != null && _mesh.uv3.Length > 0 ? _mesh.uv3 : null);
                _uvs.Add(_mesh.uv4 != null && _mesh.uv4.Length > 0 ? _mesh.uv4 : null);

                _curUVSet = -1;
                for (int i = 0; i < _uvs.Count; i++)
                {
                    if (_uvs[i] != null)
                    {
                        _curUVSet = i;
                        break;
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            if (_subMesh == null)
            {
                GUILayout.Label("Please select a GameObject in the scene with a MeshRenderer");
                return;
            }

            // texture size
            Texture texture = _subMesh.Material.mainTexture;
            Rect textureArea = new Rect(6, 176, texture != null ? texture.width : 512, texture != null ? texture.height : 512);
            textureArea.width *= _zoom;
            textureArea.height *= _zoom;

            // UI
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUI.color = Color.white;
            GUILayout.Label("Zoom", GUILayout.Width(100));
            _zoom = EditorGUILayout.Slider(_zoom, 0.1f, 3, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Size", GUILayout.Width(100));
            GUILayout.Label(textureArea.width + "x" + textureArea.height, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture", GUILayout.Width(100));
            _backgroundColor = EditorGUILayout.ColorField(_backgroundColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Line", GUILayout.Width(100));
            _lineColor = EditorGUILayout.ColorField(_lineColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Front Face", GUILayout.Width(100));
            _frontFaceColor = EditorGUILayout.ColorField(_frontFaceColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Back Face", GUILayout.Width(100));
            _backFaceColor = EditorGUILayout.ColorField(_backFaceColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.color = _isDrawLine ? Color.cyan : Color.white;
            GUI.contentColor = _isDrawLine ? Color.black : Color.white;
            _isDrawLine = GUILayout.Toggle(_isDrawLine, "Lines", "Button", GUILayout.Width(100));
            GUI.color = _isDrawFrontTriangle ? Color.cyan : Color.white;
            GUI.contentColor = _isDrawFrontTriangle ? Color.black : Color.white;
            _isDrawFrontTriangle = GUILayout.Toggle(_isDrawFrontTriangle, "Front Triangles", "Button", GUILayout.Width(100));
            GUI.color = _isDrawBackTriangle ? Color.cyan : Color.white;
            GUI.contentColor = _isDrawBackTriangle ? Color.black : Color.white;
            _isDrawBackTriangle = GUILayout.Toggle(_isDrawBackTriangle, "Back Triangles", "Button", GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            for (int i = 0; i < _uvs.Count; i++)
            {
                if (_uvs[i] == null)
                {
                    GUI.color = Color.gray;
                    GUI.contentColor = Color.white;
                    GUILayout.Label("uv" + i, "Button", GUILayout.Width(74));
                }
                else
                {
                    bool isSelect = _curUVSet == i;
                    GUI.color = isSelect ? Color.cyan : Color.white;
                    GUI.contentColor = isSelect ? Color.black : Color.white;
                    if (GUILayout.Toggle(isSelect, "uv" + i, "Button", GUILayout.Width(74)))
                    {
                        _curUVSet = i;
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // draw background
            Vector2 topLeft = new Vector2(textureArea.x, textureArea.y);
            Vector2 topRight = new Vector2(textureArea.x + textureArea.width, textureArea.y);
            Vector2 botLeft = new Vector2(textureArea.x, textureArea.y + textureArea.height);
            Vector2 botRight = new Vector2(textureArea.x + textureArea.width, textureArea.y + textureArea.height);
            _material.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            GL.Color(_backgroundColor);
            GL.Vertex(topLeft);
            GL.Vertex(topRight);
            GL.Vertex(botLeft);
            GL.Vertex(topRight);
            GL.Vertex(botRight);
            GL.Vertex(botLeft);
            GL.End();

            // draw texture
            if (texture != null)
            {
                Graphics.DrawTexture(textureArea, texture, new Rect(0, 0, 1, 1), 0, 0, 0, 0, new Color(1, 1, 1, _backgroundColor.a), null);
            }
                
            if (_curUVSet >= 0)
            {
                // draw lines
                if (_isDrawLine)
                {
                    _material.SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Color(_lineColor);
                    for (int i = 0; i < _subMesh.Indices.Length; i += 3)
                    {
                        var p1 = _uvs[_curUVSet][_subMesh.Indices[i + 0]];
                        var p2 = _uvs[_curUVSet][_subMesh.Indices[i + 1]];
                        var p3 = _uvs[_curUVSet][_subMesh.Indices[i + 2]];
                        p1.y = 1f - p1.y;
                        p2.y = 1f - p2.y;
                        p3.y = 1f - p3.y;
                        p1 = Vector2.Scale(p1, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        p2 = Vector2.Scale(p2, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        p3 = Vector2.Scale(p3, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        GL.Vertex(p1);
                        GL.Vertex(p2);
                        GL.Vertex(p2);
                        GL.Vertex(p3);
                        GL.Vertex(p3);
                        GL.Vertex(p1);
                    }
                    GL.End();
                }

                // draw triangles
                if (_isDrawFrontTriangle || _isDrawBackTriangle)
                {
                    _material.SetPass(0);
                    GL.Begin(GL.TRIANGLES);
                    for (int i = 0; i < _subMesh.Indices.Length; i += 3)
                    {
                        var P1 = _uvs[_curUVSet][_subMesh.Indices[i + 0]];
                        var P2 = _uvs[_curUVSet][_subMesh.Indices[i + 1]];
                        var P3 = _uvs[_curUVSet][_subMesh.Indices[i + 2]];
                        P1.y = 1f - P1.y;
                        P2.y = 1f - P2.y;
                        P3.y = 1f - P3.y;
                        P1 = Vector2.Scale(P1, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        P2 = Vector2.Scale(P2, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        P3 = Vector2.Scale(P3, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        if (_isDrawFrontTriangle)
                        {
                            GL.Color(_frontFaceColor);
                            GL.Vertex(P1);
                            GL.Vertex(P2);
                            GL.Vertex(P3);
                        }
                        if (_isDrawBackTriangle)
                        {
                            GL.Color(_backFaceColor);
                            GL.Vertex(P2);
                            GL.Vertex(P1);
                            GL.Vertex(P3);
                        }
                    }
                    GL.End();
                }
            }
        }

        #endregion
    }
}