using System.IO;
using UnityEngine;

public class MTGSetData {

    public class MTGSet {
        private string Name;
        private string Code;

        public MTGSet(string name, string code) {
            Name = name;
            Code = code;
        }

        public static MTGSet[] CreateFromJSON(string jsonString) {
            return JsonUtility.FromJson<MTGSet[]>(jsonString);
        }

    }

    private MTGSet[] setList;

    public MTGSetData() {
        // Read set data from JSON file and populate setList array
        string path = "C:\\stuff\\unity3d\\MTG\\MTG\\Assets\\_Imported\\Static Data\\SetList.json";
        if (!File.Exists(path)) {
            Debug.LogError("Unable to open set data file:" + path);
        }
        string[] setDefStr = File.ReadAllLines(path);
        Debug.Log(setDefStr);
        setList = MTGSet.CreateFromJSON(setDefStr.ToString());
        Debug.Log(setList);
    }
}
