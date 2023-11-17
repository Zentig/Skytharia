using System;
using System.IO;
using Skytharia.SaveManagement.Serializers;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Skytharia.SaveManagement
{
    public static class SaveManager
    {
        private const string ENCRYPTIONKEY = "rPMAfMYaBXuCcittn1tui5e2eTrna42S";

        private static ISerializer s_serializer = new JSONSerializer(); // Switch this field out to switch serialization system.
        private static string s_pathPrefix;

        public static void Save(object toSave, string path, bool encrypt)
        {
            s_pathPrefix ??= Application.persistentDataPath + "/Saved Data/";
            string toWrite;

            if (encrypt)
            {
                toWrite = s_serializer.SerializeAndEncrypt(toSave, ENCRYPTIONKEY);
            } else 
            {
                toWrite = s_serializer.SerializeObject(toSave);
            }

            try 
            {
                string totalPath = Path.Combine(s_pathPrefix, path);
                string directoryPath = Path.GetDirectoryName(totalPath);
                if (!Directory.Exists(directoryPath)) 
                { 
                    Directory.CreateDirectory(directoryPath); 
                }
                using (FileStream stream = new FileStream(totalPath, FileMode.Create)) 
                {
                    using (StreamWriter writer = new StreamWriter(stream)) {

                        writer.Write(toWrite);
                    }
                }
            } catch (Exception err) 
            {
                Debug.LogException(err);
                return;
            }
        }
        public static object Load<T>(string path, bool encrypted)
        {
            s_pathPrefix ??= Application.persistentDataPath + "/Saved Data/";
            try {
                string totalPath = Path.Combine(s_pathPrefix, path);
                string directoryPath = Path.GetDirectoryName(totalPath);
                string loaded = "";
                if (!File.Exists(totalPath)) return null;
                if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
                using (FileStream stream = new FileStream(totalPath, FileMode.Open)) 
                {
                    using (StreamReader reader = new StreamReader(stream)) 
                    {
                        loaded = reader.ReadToEnd();
                    }
                }
                if (encrypted)
                {
                    return s_serializer.DecryptAndDeserialize<T>(loaded, ENCRYPTIONKEY);
                } else
                {
                    return s_serializer.DeserializeObject<T>(loaded);
                }

            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }

        #if UNITY_EDITOR

        [Serializable]
        private class TestSaveData
        {
            public string a;
            public int b;
            public bool c;
            public float d;

            public TestSaveData() 
            {
                a = "Now is the time for all good men to come to the aid of the party.";
                b = 42;
                c = true;
                d = 3.14f;
            }
        }
        [MenuItem("Debug/SaveTestDataNoEncryption")]
        public static void DebugSaveTestDataNoEncrypt()
        {
            TestSaveData tosave = new();
            Save(tosave, "Debug/TestSaveManagerDataNoEncrypt.data", false);
        }
        [MenuItem("Debug/SaveTestDataWithEncryption")]
        public static void DebugSaveTestDataEncrypt()
        {
            TestSaveData tosave = new();
            Save(tosave, "Debug/TestSaveManagerDataEncrypt.data", true);
        }
        [MenuItem("Debug/LoadTestDataNoEncryption")]
        public static void DebugLoadTestDataNoEncrypt()
        {
            TestSaveData loaded = (TestSaveData)Load<TestSaveData>("Debug/TestSaveManagerDataNoEncrypt.data", false);
            Debug.Log($"Loaded non encrypted data: A: {loaded.a}, B: {loaded.b}, C: {loaded.c}, D: {loaded.d}");
        }
        [MenuItem("Debug/LoadTestDataWithEncryption")]
        public static void DebugLoadTestDataEncrypted()
        {
            TestSaveData loaded = (TestSaveData)Load<TestSaveData>("Debug/TestSaveManagerDataEncrypt.data", true);
            Debug.Log($"Loaded encrypted data: A: {loaded.a}, B: {loaded.b}, C: {loaded.c}, D: {loaded.d}");
        }

        #endif
    }
}