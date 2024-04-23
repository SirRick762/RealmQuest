using System.IO;
using UnityEngine;
using static UnityEditor.AssetDatabase;
using static System.IO.Path;
using static System.IO.Directory;
using UnityEditor;

public static class Setup
{
    [MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault(root: "_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
        Refresh();
    }
    static class Folders
    {

        public static void CreateDefault(string root , params string[] folders) {
            var fullpath = Combine(Application.dataPath, root);
            foreach (var folder in folders)
            {
                var path = Combine(fullpath, folder);
                if (!Exists(path))
                {
                    CreateDirectory(path);

                }
            }
        
        }
    } 
}