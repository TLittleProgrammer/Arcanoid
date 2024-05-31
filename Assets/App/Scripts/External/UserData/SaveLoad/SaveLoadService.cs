using System.IO;
using App.Scripts.External.Converters;
using Newtonsoft.Json;
using UnityEngine;
using Path = System.IO.Path;

namespace App.Scripts.External.UserData.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public void Save(ISavable savable)
        {
            string text = JsonConvert.SerializeObject(savable, Formatting.Indented, new Int2Converter());

            string pathToFile = Path.Combine(Application.persistentDataPath, savable.FileName);

            if (!CreateFileIfTheFileDoesNotExists(pathToFile))
            {
                File.Delete(pathToFile);
            }
            
            File.WriteAllText(pathToFile, text);
        }

        public void Load<TSavable>(ref TSavable savable) where TSavable : ISavable
        {
            string pathToFile = GetPath(savable.FileName);

            CreateFileIfTheFileDoesNotExists(pathToFile);

            savable = JsonConvert.DeserializeObject<TSavable>(File.ReadAllText(pathToFile));
        }

        public bool Exists(string name)
        {
            string pathToFile = GetPath(name);
            
            return File.Exists(pathToFile);
        }

        public void Delete(string name)
        {
            string pathToFile = GetPath(name);

            if (File.Exists(pathToFile))
            {
                File.Delete(pathToFile);
            }
        }

        private string GetPath(string name)
        { 
            return Path.Combine(Application.persistentDataPath, name);
        }

        private bool CreateFileIfTheFileDoesNotExists(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                File.Create(pathToFile).Close();
                File.WriteAllText(pathToFile,"{}");
                return true;
            }
            
            return false;
        }
    }
}