using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject SelectionUi;
    public GameObject TitleUi;

    public void GameStart()
    {
       TitleUi.gameObject.SetActive(false);
       SelectionUi.SetActive(true);
    }

    public void FinishCharacterSelection()
    {
        SceneManager.LoadScene(1);
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
