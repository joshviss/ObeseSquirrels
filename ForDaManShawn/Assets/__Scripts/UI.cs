using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {
//    public enum levels { Title, Game, Credit };
    string title;    //name of scene of title
    string credit;   //name of scene of credits
    string game;     //name of scene of main game
//    string path = "Button"; //path of prefab
    Canvas curCanvas;

    void Awake () {
        title = "_Title_Screen";
        credit = "_Credits";
        game = "_Scene_Main";
    }

    void Start () {
        string curScene = EditorSceneManager.GetActiveScene().name;
        curCanvas = FindObjectOfType<Canvas>();
        if (curScene == title) {
            Title_Initialization();
        } else if (curScene == credit) {
            Credit_Initialization();
        } else if (curScene == game) {
            Game_Initialization();
        }
    }

    void Update () {
        //print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != game) return;
        if (Input.GetKeyUp(KeyCode.P)) SceneManager.LoadScene(game);
        if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
    }

    void Title_Initialization() {
        int count = curCanvas.transform.childCount;
        /*
                Object buttonPrefab = Resources.Load(path, typeof(Button));
                Button start = Instantiate(buttonPrefab) as Button;
                Make_Button(start, "Start", new Vector2(0.6f, 0.4f), new Vector2(0.4f, 0.3f), (int)levels.Game);
                Button exit = Instantiate(buttonPrefab) as Button;
                Make_Button(exit, "Quit", new Vector2(0.6f, 0.3f), new Vector2(0.4f, 0.2f));
                Button credit = Instantiate(buttonPrefab) as Button;
                Make_Button(credit, "Credits", new Vector2(0.6f, 0.2f), new Vector2(0.4f, 0.1f), (int)levels.Credit);
        */
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Buttons")) {
            Button button = obj.GetComponent<Button>();
            if (button.name == "Start") {
                button.onClick.AddListener(() => SceneManager.LoadScene(game));
            } else if (button.name == "Exit") {
                button.onClick.AddListener(() => Application.Quit());
            } else if (button.name == "Credits") {
                button.onClick.AddListener(() => SceneManager.LoadScene(credit));
            }
        }

    }
    
    void Credit_Initialization() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Buttons")) {
            Button button = obj.GetComponent<Button>();
            if (button.name == "Return") {
                button.onClick.AddListener(() => SceneManager.LoadScene(title));
            }
        }
    }

    void Game_Initialization() {

    }

  /*  
    void Make_Button(Button button, string name, Vector2 anchorMax, Vector2 anchorMin, int level=-1) {
        button.transform.SetParent(curCanvas.transform);
        button.GetComponentInChildren<Text>().text = string.Format(name);
        button.GetComponent<RectTransform>().rect = 
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.6f, 0.4f);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.6f, 0.4f);
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.4f, 0.3f);
        if (name != "Quit") {
            button.onClick.AddListener(() => SceneManager.GetSceneAt((int)levels.Game));
        } else {
            button.onClick.AddListener(() => Application.Quit());
        }
    }
*/    
}
