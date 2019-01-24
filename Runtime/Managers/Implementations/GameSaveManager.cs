using NaughtyAttributes;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace GameplayIngredients
{
    [ManagerDefaultPrefab("GameSaveManager")]
    public class GameSaveManager : Manager
    {
        private const string systemSaveName = "system.sav";
        private const string userSaveName = "user{0}.sav";

        Dictionary<string, System.Object> systemSaveEntries;
        Dictionary<string, System.Object> currentUserSaveEntries;

        [ReorderableList]
        public Callable[] OnLoad;

        [ReorderableList]
        public Callable[] OnSave;

        void Awake()
        {
            systemSaveEntries = new Dictionary<string, System.Object>();
            currentUserSaveEntries = new Dictionary<string, System.Object>();
        }

        private void OnEnable()
        {
            // Load System Save
            LoadSystemSave();
            //TestFunc();
        }

        void TestFunc()
        {
            SetValue("Toto", Location.System, 1.0f);
            SetValue("Tata", Location.System, 10);
            SetValue("Titi", Location.System, "1.0");
            SetValue("Tutu", Location.System, false);
            SaveSystemSave();
        }

        #region SAVE/LOAD

        public void LoadSystemSave()
        {
            systemSaveEntries = LoadFile(systemSaveName);
            Callable.Call(OnLoad);
        }

        public void SaveSystemSave()
        {
            SaveFile(systemSaveName, systemSaveEntries);
            Callable.Call(OnSave);
        }

        private byte currentUserIndex = 0;

        public void LoadUserSave(byte index)
        {
            currentUserIndex = index;
            Callable.Call(OnLoad);
            currentUserSaveEntries = LoadFile(string.Format(userSaveName, index));
        }

        public void SaveUserSave()
        {
            SaveUserSave(currentUserIndex);
        }

        public void SaveUserSave(byte index)
        {
            // Save
            currentUserIndex = index;
            Callable.Call(OnSave);
            SaveFile(string.Format(userSaveName, index), currentUserSaveEntries);
        }

        #endregion

        #region VALUES

        public enum Location
        {
            System = 0,
            User = 1,
        }

        Dictionary<string, object> GetEntriesFor(Location location)
        {
            if (location == Location.System)
                return systemSaveEntries;
            else
                return currentUserSaveEntries;

        }

        public bool HasValue<T>(string name, Location location)
        {
            var dict = GetEntriesFor(location);
            return dict.ContainsKey(name) && dict[name] is T;
        }

        public T GetValue<T>(string name, Location location)
        {
            if (HasValue<T>(name, location))
            {
                var dict = GetEntriesFor(location);
                return (T)dict[name];
            }
            else
                return default(T);
        }

        void SetValue<T>(string name, Location location, T value)
        {
            var dict = GetEntriesFor(location);

            if (HasValue<T>(name, location))
            {
                dict[name] = value;
            }
            else if (dict.ContainsKey(name)) // bad type
            {
                Debug.LogWarning(string.Format("GameSaveManager : {0} entry '{1}' changed type to {2}", location, name, typeof(T)));
                dict[name] = value;
            }
            else
            {
                dict.Add(name, value);
            }
        }

        #endregion

        #region SERIALIZATION

        Dictionary<string, object> LoadFile(string fileName)
        {
            if(!System.IO.File.Exists(Application.dataPath+fileName))
            {
                SaveFile(fileName, new Dictionary<string, object>());
            }

            var dict = new Dictionary<string, System.Object>();

            string contents= File.ReadAllText(Application.dataPath + fileName);

            SerializableOutput data = JsonUtility.FromJson<SerializableOutput>(contents);

            for(int i = 0; i < data.keys.Length; i++)
            {
                string val = data.values[i];
                object value;
                if (data.types[i] == SerializableOutput.ValueType.Bool)
                    value = bool.Parse(val);
                else if (data.types[i] == SerializableOutput.ValueType.Int)
                    value = int.Parse(val);
                else if (data.types[i] == SerializableOutput.ValueType.Float)
                    value = float.Parse(val);
                else
                    value = val;

                dict.Add(data.keys[i], value);
            }

            return dict;
        }

        void SaveFile(string filename, Dictionary<string, System.Object> entries)
        {
            int count = entries.Count;

            SerializableOutput data = new SerializableOutput();

            data.keys = new string[count];
            data.values = new string[count];
            data.types = new SerializableOutput.ValueType[count];

            int i = 0;
            foreach (var kvp in entries)
            {
                data.keys[i] = kvp.Key;
                object value = kvp.Value;

                if (value is bool)
                    data.types[i] = SerializableOutput.ValueType.Bool;
                else if (value is int)
                    data.types[i] = SerializableOutput.ValueType.Int;
                else if (value is float)
                    data.types[i] = SerializableOutput.ValueType.Float;
                else
                    data.types[i] = SerializableOutput.ValueType.String;

                data.values[i] = kvp.Value.ToString();
                i++;
            }

            File.WriteAllText(Application.dataPath+filename, JsonUtility.ToJson(data));
        }

        [System.Serializable]
        class SerializableOutput
        {
            public string[] keys;
            public string[] values;
            public ValueType[] types;

            [System.Serializable]
            public enum ValueType
            {
                Bool = 0,
                Int = 1,
                Float = 2,
                String = 3
            }
        }

        #endregion

    }
}
