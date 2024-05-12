using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Path = System.IO.Path;

namespace App.Scripts.General.UserData.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public void Save(ISavable savable)
        {
            string text = JsonConvert.SerializeObject(savable, Formatting.Indented);

            string pathToFile = Path.Combine(Application.persistentDataPath, savable.FileName);

            if (!CreateFileIfTheFileDoesNotExists(pathToFile))
            {
                File.Delete(pathToFile);
            }
            
            File.WriteAllText(pathToFile, text);
        }

        public void Load<TSavable>(ref TSavable savable) where TSavable : ISavable
        {
            string pathToFile = Path.Combine(Application.persistentDataPath, savable.FileName);

            CreateFileIfTheFileDoesNotExists(pathToFile);

            savable = JsonConvert.DeserializeObject<TSavable>(File.ReadAllText(pathToFile));
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