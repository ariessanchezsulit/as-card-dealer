#region Header

// SceneLoader.cs
// Aries Sanchez Sulit
// 2019-09-19 at 9:21 PM

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PP.Utility.SceneLoader
{
    /// <inheritdoc />
    /// <summary>
    ///     Creates a dockable window that contains all scenes included in the build with buttons for quick navigation.
    /// </summary>
    public sealed class SceneLoader : EditorWindow
    {
        #region Constants

        private const string ADDITIVE         = "Addt";
        private const string SINGLE           = "Load";
        private const string PLAY             = "Play";
        private const string ADDITIVE_TOOLTIP = "Adds a Scene to the current open Scenes and loads it.";
        private const string LOAD_TOOLTIP     = "Close all current open Scenes and loads a Scene.";
        private const string PLAY_TOOLTIP     = "Close all current open Scenes and loads a Scene then play.";
        private const string FAV_TOOLTIP      = "Add Scene to Favorite Collection.";
        private const string REM_TOOLTIP      = "Remove Scene from Favorite Collection";
        private const string ENABLE_TOOLTIP   = "Scene added to Build";

        private const string EMPTY_SCENE =
            "No Scenes In Build \n Click Ctrl + Shift + B \n to open Build Settings! \n\n or \n\n Click Button Below!";

        private const string WINDOW_LABEL    = "Scenes in Build";
        private const string SAVE_FAVORITE   = "Favorite Scenes";
        private const string SEARCH_SCENE    = "Search Scene";
        private const string ADD_FAVORITE    = "★";
        private const string REMOVE_FAVORITE = "ꭙ";
        private const string BUILD_SETTING   = "Open Build Settings";

        private const int BUTTON_HEIGHT  = 15;
        private const int BUTTON_WIDTH   = 45;
        private const int FAVORITE_WIDTH = 25;

        #endregion Constants

        #region Static Properties

        private static EditorWindow _windowInstance;

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        ///     Menu callback on opening Scene Loader
        /// </summary>
        [MenuItem("PP/Scene Loader _%`")] 
        public static void ShowSceneLoader()
        {
            if (_windowInstance != null)
            {
                _windowInstance.Close();
                _windowInstance = null;
            }
            else
            {
                _windowInstance = GetWindow(typeof(SceneLoader), false, "Scene Loader", true);
            }
        }

        /// <summary>
        ///     Custom GUI Style
        /// </summary>
        /// <returns></returns>
        private static GUIStyle GetEmptyStyle()
        {
            var style = GUI.skin.GetStyle("Label");
            style.stretchWidth  = true;
            style.stretchHeight = true;
            style.alignment     = TextAnchor.MiddleCenter;
            style.fontSize      = 15;
            return style;
        }

        /// <summary>
        ///     Checks if Scene is added to build.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool CheckSceneEnabled(string path)
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            var scene  = scenes.Find(_ => _.path == path);
            return scene != null && scene.enabled;
        }

        /// <summary>
        ///     Display Loader Button and Load Scene.
        /// </summary>
        /// <param name="path"></param>
        private static void SceneButtonLoader(string path)
        {
            if (GUILayout.Button(
                new GUIContent(SINGLE, LOAD_TOOLTIP),
                GUILayout.Width(BUTTON_WIDTH),
                GUILayout.Height(BUTTON_HEIGHT)
            ))
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            }
            else if (GUILayout.Button(
                new GUIContent(ADDITIVE, ADDITIVE_TOOLTIP),
                GUILayout.Width(BUTTON_WIDTH),
                GUILayout.Height(BUTTON_HEIGHT)
            ))
            {
                var sceneLoaded = SceneManager.GetSceneByPath(path);
                if (string.IsNullOrEmpty(sceneLoaded.name))
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                else
                    EditorSceneManager.CloseScene(sceneLoaded, true);
            }
            else if (GUILayout.Button(
                new GUIContent(PLAY, PLAY_TOOLTIP),
                GUILayout.Width(BUTTON_WIDTH),
                GUILayout.Height(BUTTON_HEIGHT)
            ))
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                EditorApplication.isPlaying = true;
            }
        }

        /// <summary>
        ///     Custom GUI Style
        /// </summary>
        /// <returns></returns>
        private static GUIStyle GetStyle(string styleType = "Label")
        {
            var style = GUI.skin.GetStyle(styleType);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize  = 12;
            return style;
        }

        #endregion

        #region Unity Event Methods

        private void OnFocus()
        {
            Favorite.FavoriteCollection = new List<string>();
            RetrieveFavorites();
        }

        private void OnGUI()
        {
            if (EditorBuildSettings.scenes.Length > 0)
            {
                ScrollPos = GUILayout.BeginScrollView(ScrollPos);
                AddInputField();

                if (string.IsNullOrEmpty(SearchKey)) FavoriteSceneHandler();

                AllSceneHandler();
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label(EMPTY_SCENE, GetEmptyStyle());
            }

            if (GUILayout.Button(BUILD_SETTING, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
                GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));

            while (SceneToDelete.Count > 0)
            {
                var sceneRemove = SceneToDelete.Dequeue();
                Favorite.FavoriteCollection.Remove(sceneRemove);
                SaveToJson();
            }
        }

        private void OnDestroy()
        {
            _windowInstance = null;
        }

        #endregion Unity Event Methods

        #region Properties

        private readonly Queue<string> _scenesToDelete = new Queue<string>();

        private Queue<string> SceneToDelete {
            get { return _scenesToDelete; }
        }

        private List<EditorBuildSettingsScene> ActiveScene { get; set; }

        private readonly FavoriteSceneData _favorite = new FavoriteSceneData();

        private FavoriteSceneData Favorite {
            get { return _favorite; }
        }

        private Vector2 ScrollPos { get; set; }

        private string _searchKey = string.Empty;

        private string SearchKey {
            get { return _searchKey; }
            set { _searchKey = value; }
        }

        #endregion Properties

        #region Fields

        private readonly char[] _delimiter = { '/' };

        private char[] Splitter {
            get { return _delimiter; }
        }

        #endregion Fields

        #region GroupHandlers

        /// <summary>
        ///     Search bar for all Active Scene.
        /// </summary>
        private void AddInputField()
        {
            GUILayout.Label(SEARCH_SCENE, EditorStyles.boldLabel);
            SearchKey = GUILayout.TextField(SearchKey, 50, GetStyle("TextField"), GUILayout.Height(BUTTON_HEIGHT));
        }

        /// <summary>
        ///     Display Collection of Tagged Scene for favorite.
        /// </summary>
        private void FavoriteSceneHandler()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(SAVE_FAVORITE, EditorStyles.boldLabel);
            Favorite.FavoriteCollection.ForEach(
                favScene => {
                    if (EditorBuildSettings
                        .scenes.ToList()
                        .Find(sceneBuild => sceneBuild.path == favScene)
                        == null)
                        SceneToDelete.Enqueue(favScene);
                    CreateFavoriteLoader(favScene);
                }
            );
            GUILayout.EndVertical();
        }

        /// <summary>
        ///     Display Collection of entire Active Scene.
        /// </summary>
        private void AllSceneHandler()
        {
            ActiveScene = EditorBuildSettings.scenes.ToList();
            GUILayout.BeginVertical();
            GUILayout.Label(WINDOW_LABEL, EditorStyles.boldLabel);
            ActiveScene.RemoveAll(_ => string.IsNullOrEmpty(_.path));
            ActiveScene.ForEach(CreateSceneLoaderButton);
            GUILayout.EndVertical();
        }

        #endregion GroupHandlers

        #region Item Creator

        /// <summary>
        ///     Display Collection item elements.
        /// </summary>
        /// <param name="path"></param>
        private void CreateFavoriteLoader(string path)
        {
            GUILayout.BeginHorizontal("Box", GUILayout.MaxHeight(BUTTON_HEIGHT));
            GUILayout.Toggle(CheckSceneEnabled(path), string.Empty, GUILayout.Width(10));
            GUILayout.Label(
                new GUIContent(ExtractSceneName(path), path),
                GetStyle(),
                GUILayout.Height(BUTTON_HEIGHT)
            );
            RemoveSceneFavorite(path);
            SceneButtonLoader(path);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Display Collection item elements.
        /// </summary>
        /// <param name="sceneItem"></param>
        private void CreateSceneLoaderButton(EditorBuildSettingsScene sceneItem)
        {
            if (!string.IsNullOrEmpty(SearchKey) && !CheckSceneName(sceneItem.path)) return;

            GUILayout.BeginHorizontal("Box", GUILayout.MaxHeight(BUTTON_HEIGHT));
            sceneItem.enabled = GUILayout.Toggle(
                sceneItem.enabled,
                new GUIContent(string.Empty, ENABLE_TOOLTIP),
                GUILayout.Width(10)
            );
            GUILayout.Label(
                new GUIContent(ExtractSceneName(sceneItem.path), sceneItem.path),
                GetStyle(),
                GUILayout.Height(BUTTON_HEIGHT)
            );
            AddSceneFavorite(sceneItem.path);
            SceneButtonLoader(sceneItem.path);
            GUILayout.EndHorizontal();
        }

        #endregion Item Creator

        #region Button Handlers

        /// <summary>
        ///     Display and Add scene to favorite Collection.
        /// </summary>
        /// <param name="path"></param>
        private void AddSceneFavorite(string path)
        {
            var isFavorite = Favorite.FavoriteCollection.Find(scenePath => scenePath == path) != null;

            if (isFavorite) return;
            if (!GUILayout.Button(
                new GUIContent(ADD_FAVORITE, FAV_TOOLTIP),
                GUILayout.Width(FAVORITE_WIDTH),
                GUILayout.Height(BUTTON_HEIGHT)
            )) return;

            Favorite.FavoriteCollection.Add(path);
            SaveToJson();
        }

        /// <summary>
        ///     Display Remove Button and Remove Scene from Favorite Collection.
        /// </summary>
        /// <param name="path"></param>
        private void RemoveSceneFavorite(string path)
        {
            if (GUILayout.Button(
                new GUIContent(REMOVE_FAVORITE, REM_TOOLTIP),
                GUILayout.Width(FAVORITE_WIDTH),
                GUILayout.Height(BUTTON_HEIGHT)
            ))
                SceneToDelete.Enqueue(path);
        }

        #endregion Button Handlers

        #region Helpers

        /// <summary>
        ///     Retrieve saved json data of Favorite Collection.
        /// </summary>
        private void RetrieveFavorites()
        {
            var saveData = PlayerPrefs.GetString(SAVE_FAVORITE, string.Empty);
            Favorite.FavoriteCollection = !string.IsNullOrEmpty(saveData)
                ? JsonUtility.FromJson<FavoriteSceneData>(saveData).FavoriteCollection
                : new List<string>();
        }

        /// <summary>
        ///     Save a json data of Favorite Collection.
        /// </summary>
        private void SaveToJson()
        {
            var serializeData = JsonUtility.ToJson(Favorite);
            PlayerPrefs.SetString(SAVE_FAVORITE, serializeData);
            RetrieveFavorites();
        }

        /// <summary>
        ///     Extract Scene name from path.
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private string ExtractSceneName(string scenePath)
        {
            var splitVal = scenePath.Split(Splitter);
            return splitVal[splitVal.Length - 1];
        }

        /// <summary>
        ///     Checks if scene name contains keyword from search bar.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckSceneName(string path)
        {
            return ExtractSceneName(path).ToLower().Contains(SearchKey.ToLower());
        }

        #endregion
    }
}
