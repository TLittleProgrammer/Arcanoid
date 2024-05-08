using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameScene.Levels.AssetManagement;
using GameScene.Levels.Entities;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Rendering;
using Graph = UnityEditor.Graphs.Graph;
using Random = UnityEngine.Random;

namespace Editor.LevelEditor
{
    public class LevelGeometry : OdinEditorWindow
    {
        //Geometry
        [TabGroup("Editor", "Geometry", SdfIconType.Map, TextColor = "green")]
        [TableMatrix(HorizontalTitle = "Level Grid", DrawElementMethod = "DrawElement", ResizableColumns = false, RowHeight = 32)]
        public int[,] Grid;
        
        [TabGroup("Editor", "Geometry", SdfIconType.Map, TextColor = "green")]
        public EntityCellData CurrentEntityCellData;
        [TabGroup("Editor", "Geometry", SdfIconType.Map, TextColor = "green")]
        [ValueDropdown("PresetNames")]
        public string BrushingPresetName;
        
        private Rect CurrentRectEntityCellData;

        //Parameters
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [OnValueChanged("CreateGrid")]
        public int2 GridSize = new(3, 5);

        private PresetsData PresetsData;
        private EntityProvider EntitiesProvider;

        private List<string> PresetNames => PresetsData.PresetItems.Select(x => x.Key).ToList();
        private bool _brushIsEnabled = false;

        [MenuItem("Tools/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelGeometry>().Show();
        }

        [OnInspectorInit]
        private void CreateGrid()
        {
            CurrentEntityCellData = new();
            PresetsData = GetPresets();
            BrushingPresetName = PresetsData.PresetItems.First().Key;
            EntitiesProvider = Resources.Load<EntityProvider>($"Configs/Entities/EntitiesProvider");

            Grid = new int[GridSize.x, GridSize.y];
        }

        private PresetsData GetPresets()
        {
            var path = "Assets/Scripts/Editor/LevelEditor/Presets.json";

            var json = File.ReadAllText(path);
            
            return JsonConvert.DeserializeObject<PresetsData>(json);
        }

        private int DrawElement(Rect rect, int value)
        {
            value = GetChangeValue(rect, value);
            DrawRect(rect, value);
            TryShowCellData(rect, value);

            return value;
        }

        private int GetChangeValue(Rect rect, int initialValue)
        {
            if (Event.current.type == EventType.MouseDrag && rect.Contains(Event.current.mousePosition))
            {
                PresetItem presetItem = PresetsData.PresetItems[BrushingPresetName];
                initialValue = int.Parse(presetItem.BlockKey);

                GUI.changed = true;
                Event.current.Use();
            }

            return initialValue;
        }

        private void DrawRect(Rect rect, int value)
        {
            string key = value.ToString();
            
            if (EntitiesProvider.EntityStages.ContainsKey(key))
            {
                Texture2D texture = EntitiesProvider.EntityStages[key].Sprite.texture;

                GUI.DrawTexture(rect.Padding(1), texture);
            }
            else
            {
                GUI.DrawTexture(rect.Padding(1), default);
            }
        }

        private void TryShowCellData(Rect rect, int value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                string key = value.ToString();

                if (EntitiesProvider.EntityStages.ContainsKey(key))
                {
                    EntityStage entityStage = EntitiesProvider.EntityStages[key];
                    
                    CurrentEntityCellData.Sprite = entityStage.Sprite;
                    CurrentEntityCellData.HealthPoints = entityStage.HealthCounter;
                }
            }
        }

        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [ShowIf("_brushIsEnabled")]
        [HorizontalGroup("Editor/Split", 0.5f)]
        [Button(SdfIconType.BrushFill, IconAlignment = IconAlignment.LeftOfText, Name = "Brush"), GUIColor(0.4f, 0.8f, 1)]
        private void OnBrushButtonClickedEnabled()
        {
            _brushIsEnabled = false;
        }
        
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [HideIf("_brushIsEnabled")]
        [HorizontalGroup("Editor/Split", 0.5f)]
        [Button(SdfIconType.BrushFill, IconAlignment = IconAlignment.LeftOfText, Name = "Brush")]
        private void OnBrushButtonClickedDisabled()
        {
            _brushIsEnabled = true;
        }
    }
}