using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Scripts.External.Converters;
using App.Scripts.External.Extensions.MathExtensions;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Tools.Editor.LevelEditor
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
        
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [OnValueChanged("CreateGrid")]
        public int2 GridSize = new(3, 5);
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        public int2 OffsetBetweenCells = new(0, 0);
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        public int HorizontalOffset = 0;
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        public int TopOffset = 0;
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        public int HealthCount = 3;

        private PresetsData PresetsData;
        private EntityProvider EntitiesProvider;
        private string _pathToDirectoryLevels;

        private List<string> PresetNames => PresetsData.PresetItems.Select(x => x.Key).ToList();

        private static void OpenWindow()
        {
            GetWindow<LevelGeometry>().Show();
        }

        [OnInspectorInit]
        private void CreateGrid()
        {
            _pathToDirectoryLevels = Path.Combine(Application.dataPath, "Resources", "Levels");
            CurrentEntityCellData = new();
            PresetsData = GetPresets();
            BrushingPresetName = PresetsData.PresetItems.First().Key;
            EntitiesProvider = Resources.Load<EntityProvider>($"Configs/Entities/EntitiesProvider_ResolveProblem");

            Grid = Grid.CopyWithNewSize(GridSize.x, GridSize.y);
        }

        private PresetsData GetPresets()
        {
            var path = "Assets/App/Scripts/Tools/Editor/LevelEditor/Presets.json";

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
                    CurrentEntityCellData.HealthPoints = entityStage.MaxHealthCounter;
                }
            }
        }

        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [Button("Save Level")]
        private void SaveLevel()
        {
            string path = EditorUtility.SaveFilePanel("Save Level", _pathToDirectoryLevels, "level.json", "json");

            if (path.Length != 0)
            {
                LevelData levelData = new();
                levelData.GridSize = GridSize;
                levelData.Grid = Grid;
                levelData.HorizontalOffset = HorizontalOffset;
                levelData.TopOffset = TopOffset;
                levelData.OffsetBetweenCells = OffsetBetweenCells;
                levelData.HealthCount = HealthCount;
                
                var json = JsonConvert.SerializeObject(levelData, Formatting.Indented, new Int2Converter());
                
                File.WriteAllText(path, json);
                AssetDatabase.ImportAsset("Assets" + path.Split("Assets")[0]);
                AssetDatabase.Refresh();
            }
        }
        
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [Button("Load Level")]
        private void LoadLevel()
        {
            var path = EditorUtility.OpenFilePanel("Open Level", _pathToDirectoryLevels, "json");
            if (path.Length != 0)
            {
                var json = File.ReadAllText(path);

                var data = JsonConvert.DeserializeObject<LevelData>(json);

                GridSize = data.GridSize;
                CreateGrid();
                
                OffsetBetweenCells = data.OffsetBetweenCells;
                HorizontalOffset   = data.HorizontalOffset;
                TopOffset          = data.TopOffset;
                HealthCount        = data.HealthCount == 0 ? 3 : data.HealthCount;

                Grid = data.Grid;
            }
        }
    }
}