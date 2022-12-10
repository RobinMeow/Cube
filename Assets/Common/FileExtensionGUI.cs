
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;

// Source: https://gist.github.com/PROGrand/917e4663f161d6f48104f8cd9808cb30

[InitializeOnLoad]
public sealed class FileExtensionGUI
{
    static GUIStyle _labelStyles = null;
    static string _selectedGuid = String.Empty;
    static bool _selectedAssetIsValid = false;
    static Color32 _selectedTextColor = new Color32(255, 147, 64, 255);
    static Color32 _textColor = new Color32(255, 147, 64, 199);

    static readonly Dictionary<string, Color32> _extensionColors = new Dictionary<string, Color32>{
        { ".cs", new Color32(67, 255, 46, 199) }, 
        { ".prefab", new Color32(127, 214, 253, 199) }, // Unitys light blue 
        { ".asset", new Color32(246, 108, 64, 199) }, // Unitys orange 
        { ".asmdef", new Color32(75, 135, 171, 199) }, // asmdef icon blue
        { ".physicsMaterial2D", new Color32(177, 253, 89, 199) },
        { ".physicsMaterial", new Color32(177, 253, 89, 199) },
        { ".wav", new Color32(252,191,7, 199) },
        // ToDo: add more extensionc colors 
        // Unity DarkPurple: new Color32(97, 51, 238, 199);
        // Unity Green: new Color32(20, 211, 104, 199);

//        .unity Silber
//.png, .jpged, .jpg all images one color
//.md, .json, .txt => all text based files in one color
//.mat => blue
    };


    static readonly string EMPTY_ASSET_GUID = Guid.Empty.ToString().Replace("-", "");
    static string _previousGuid = String.Empty;

    static FileExtensionGUI()
    {
        EditorApplication.projectWindowItemOnGUI += HandleOnGUI;
        Selection.selectionChanged += HandleSelectionChanged;
    }

    static void HandleSelectionChanged()
    {
        if (Selection.activeObject != null)
            _selectedAssetIsValid = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out _selectedGuid, out long _);
    }

    static bool IsValidAssetGuid(string assetGuid)
    {
        return !String.IsNullOrEmpty(assetGuid) && assetGuid.Length == EMPTY_ASSET_GUID.Length;
    }

    static void HandleOnGUI(string guid, Rect selectionRect) 
    {
        if (guid == _previousGuid)
            return;
        
        _previousGuid = guid;

        if (!IsValidAssetGuid(guid))
            return;

        EditorWindow projectWindow = GetProjectWindow();

        if (projectWindow == null)
            return;

        bool isSingleColumnView = IsSingleColumnView(projectWindow);

        if (!isSingleColumnView && IsThumbnailsView(projectWindow))
            return;

        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (0 >= path.Length)
            return;

        if (!Path.HasExtension(path)) 
            return;
        
        string fileName = Path.GetFileNameWithoutExtension(path);

        string[] directories = fileName.Split(Path.DirectorySeparatorChar);
        if (directories[^1].Contains("."))
            return;

        string extension = Path.GetExtension(path);

        bool selected = _selectedAssetIsValid && guid == _selectedGuid;

        if (_labelStyles == null)
            _labelStyles = new GUIStyle(EditorStyles.label);

        bool extensionHasOwnColor = TryGetExtensionColor(extension, out Color32 extensionColor);

        if (extensionHasOwnColor && selected)
            extensionColor = new Color32(extensionColor.r, extensionColor.g, extensionColor.b, 255);

        _labelStyles.normal.textColor = extensionHasOwnColor 
            ? extensionColor 
            : GetFallBackTextColor(selected);

        GUIContent fileExtensionContent = new GUIContent(extension);
        Vector2 extensionSize = _labelStyles.CalcSize(fileExtensionContent);
        Vector2 fileNameSize = _labelStyles.CalcSize(new GUIContent(fileName));
        selectionRect.x += fileNameSize.x + (isSingleColumnView ? 15 : 18);
        selectionRect.width = fileNameSize.x + extensionSize.x;

        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
        
        offsetRect.x += 1.5f; // pixels between filename and extension
        offsetRect.y -= 1.0f; // display correction, so extension is even (lineHeight) with filename 

        EditorGUI.LabelField(offsetRect, fileExtensionContent, _labelStyles);
    }

    static Color32 GetFallBackTextColor(bool selected)
    {
        return selected
            ? _selectedTextColor
            : _textColor;
    }

    static bool TryGetExtensionColor(string extension, out Color32 extensionColor)
    {
        return _extensionColors.TryGetValue(extension, out extensionColor);
    }

    static bool IsThumbnailsView(EditorWindow projectWindow)
    {
        object gridSize = projectWindow
            .GetType()
            .GetProperty("listAreaGridSize", BindingFlags.Instance | BindingFlags.Public)
            .GetValue(projectWindow, null);
        
        return (float)gridSize > 16f;
    }

    static bool IsSingleColumnView(EditorWindow projectWindow)
    {
        object columnsCount = projectWindow
            .GetType()
            .GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(projectWindow);

        return (int)columnsCount == 0;
    }

    static EditorWindow GetProjectWindow()
    {
        EditorWindow focusedWindow = EditorWindow.focusedWindow;
        if (focusedWindow != null && IsProjectWindow(focusedWindow))
            return focusedWindow;

        return FindProjectWindow();
    }

    static bool IsProjectWindow(EditorWindow editorWindow)
    {
        return editorWindow.titleContent.text == "Project";
    }

    static EditorWindow FindProjectWindow()
    {
        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (EditorWindow item in windows)
        {
            if (IsProjectWindow(item))
                return item;
        }

        return null;
    }
}
#endif
