/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUVViewerEditorWindow (version 1.0)
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

        private Mesh mMesh = null;
        private Renderer mRenderer = null;
        private List<SubMesh> mSubMeshes = new List<SubMesh>();
        private SubMesh mSubMesh = null;
        private List<Vector2[]> mUVs = new List<Vector2[]>();
        private Material mMaterial;

        private Color mBackgroundColor = new Color(0, 0, 0, 0.05f);
        private Color mLineColor = new Color(1, 1, 1, 0.4f);
        private Color mFrontFaceColor = new Color(0, 1, 0, 0.4f);
        private Color mBackFaceColor = new Color(1, 0, 0, 0.4f);
        private bool mIsDrawLine = true;
        private bool mIsDrawFrontTriangle = true;
        private bool mIsDrawBackTriangle = true;
        private int mCurrentUVSet = 0;
        private float mZoom = 1f;

        #endregion

        #region ----- GUI Implementation -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] UV Viewer");
            minSize = new Vector2(512, 768);
            
            mMaterial = MyResourceManager.GetMaterialBlendedColor();
        }
        
        /// <summary>
        /// OnSelectionChange.
        /// </summary>
        void OnSelectionChange()
        {
            mMesh = null;
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
                    mMesh = meshFilter.sharedMesh;
                }
                else
                {
                    SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer != null)
                    {
                        mMesh = skinnedMeshRenderer.sharedMesh;
                    }
                }

                if (mMesh == null)
                {
                    return;
                }

                mRenderer = gameObject.GetComponent<Renderer>();
                if (mRenderer == null)
                {
                    return;
                }

                Material[] materials = mRenderer.sharedMaterials;
                mSubMeshes.Clear();
                for (int i = 0, count = mMesh.subMeshCount; i < count; ++i)
                {
                    var subMesh = new SubMesh();
                    subMesh.Material = materials[i % materials.Length];
                    subMesh.Indices = mMesh.GetIndices(i);
                    mSubMeshes.Add(subMesh);
                }
                mSubMesh = mSubMeshes[0];

                mUVs.Clear();
                mUVs.Add(mMesh.uv != null && mMesh.uv.Length > 0 ? mMesh.uv : null);
                mUVs.Add(mMesh.uv2 != null && mMesh.uv2.Length > 0 ? mMesh.uv2 : null);
                mUVs.Add(mMesh.uv3 != null && mMesh.uv3.Length > 0 ? mMesh.uv3 : null);
                mUVs.Add(mMesh.uv4 != null && mMesh.uv4.Length > 0 ? mMesh.uv4 : null);

                mCurrentUVSet = -1;
                for (int i = 0; i < mUVs.Count; i++)
                {
                    if (mUVs[i] != null)
                    {
                        mCurrentUVSet = i;
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
            if (mSubMesh == null)
            {
                GUILayout.Label("Please select a GameObject in the scene with a MeshRenderer");
                return;
            }

            // texture size
            Texture texture = mSubMesh.Material.mainTexture;
            Rect textureArea = new Rect(6, 176, texture != null ? texture.width : 512, texture != null ? texture.height : 512);
            textureArea.width *= mZoom;
            textureArea.height *= mZoom;

            // UI
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUI.color = Color.white;
            GUILayout.Label("Zoom", GUILayout.Width(100));
            mZoom = EditorGUILayout.Slider(mZoom, 0.1f, 3, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Size", GUILayout.Width(100));
            GUILayout.Label(textureArea.width + "x" + textureArea.height, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture", GUILayout.Width(100));
            mBackgroundColor = EditorGUILayout.ColorField(mBackgroundColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Line", GUILayout.Width(100));
            mLineColor = EditorGUILayout.ColorField(mLineColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Front Face", GUILayout.Width(100));
            mFrontFaceColor = EditorGUILayout.ColorField(mFrontFaceColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Back Face", GUILayout.Width(100));
            mBackFaceColor = EditorGUILayout.ColorField(mBackFaceColor, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.color = mIsDrawLine ? Color.cyan : Color.white;
            GUI.contentColor = mIsDrawLine ? Color.black : Color.white;
            mIsDrawLine = GUILayout.Toggle(mIsDrawLine, "Lines", "Button", GUILayout.Width(100));
            GUI.color = mIsDrawFrontTriangle ? Color.cyan : Color.white;
            GUI.contentColor = mIsDrawFrontTriangle ? Color.black : Color.white;
            mIsDrawFrontTriangle = GUILayout.Toggle(mIsDrawFrontTriangle, "Front Triangles", "Button", GUILayout.Width(100));
            GUI.color = mIsDrawBackTriangle ? Color.cyan : Color.white;
            GUI.contentColor = mIsDrawBackTriangle ? Color.black : Color.white;
            mIsDrawBackTriangle = GUILayout.Toggle(mIsDrawBackTriangle, "Back Triangles", "Button", GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            for (int i = 0; i < mUVs.Count; i++)
            {
                if (mUVs[i] == null)
                {
                    GUI.color = Color.gray;
                    GUI.contentColor = Color.white;
                    GUILayout.Label("uv" + i, "Button", GUILayout.Width(74));
                }
                else
                {
                    bool isSelect = mCurrentUVSet == i;
                    GUI.color = isSelect ? Color.cyan : Color.white;
                    GUI.contentColor = isSelect ? Color.black : Color.white;
                    if (GUILayout.Toggle(isSelect, "uv" + i, "Button", GUILayout.Width(74)))
                    {
                        mCurrentUVSet = i;
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
            mMaterial.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            GL.Color(mBackgroundColor);
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
                Graphics.DrawTexture(textureArea, texture, new Rect(0, 0, 1, 1), 0, 0, 0, 0, new Color(1, 1, 1, mBackgroundColor.a), null);
            }
                
            if (mCurrentUVSet >= 0)
            {
                // draw lines
                if (mIsDrawLine)
                {
                    mMaterial.SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Color(mLineColor);
                    for (int i = 0; i < mSubMesh.Indices.Length; i += 3)
                    {
                        var p1 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 0]];
                        var p2 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 1]];
                        var p3 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 2]];
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
                if (mIsDrawFrontTriangle || mIsDrawBackTriangle)
                {
                    mMaterial.SetPass(0);
                    GL.Begin(GL.TRIANGLES);
                    for (int i = 0; i < mSubMesh.Indices.Length; i += 3)
                    {
                        var P1 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 0]];
                        var P2 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 1]];
                        var P3 = mUVs[mCurrentUVSet][mSubMesh.Indices[i + 2]];
                        P1.y = 1f - P1.y;
                        P2.y = 1f - P2.y;
                        P3.y = 1f - P3.y;
                        P1 = Vector2.Scale(P1, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        P2 = Vector2.Scale(P2, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        P3 = Vector2.Scale(P3, new Vector2(textureArea.width, textureArea.height)) + new Vector2(textureArea.x, textureArea.y);
                        if (mIsDrawFrontTriangle)
                        {
                            GL.Color(mFrontFaceColor);
                            GL.Vertex(P1);
                            GL.Vertex(P2);
                            GL.Vertex(P3);
                        }
                        if (mIsDrawBackTriangle)
                        {
                            GL.Color(mBackFaceColor);
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
