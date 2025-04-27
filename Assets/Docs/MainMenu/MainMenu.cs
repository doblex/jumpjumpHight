using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public UIDocument doc;
    VisualElement root;

    Button play;
    Button quit;

    private void Awake()
    {
        root = doc.rootVisualElement;

        play = root.Q<Button>("Play");
        play.clicked += OnPlayButtonClick;


        quit = root.Q<Button>("Exit");
        quit.clicked += OnExitButtonClick;
    }

    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Load");
        StartCoroutine(LoadSceneAsync("Game"));
        
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        //yield return new WaitForSeconds(1);

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            if (asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnExitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
