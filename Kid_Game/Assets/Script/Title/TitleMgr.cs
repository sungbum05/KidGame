using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleMgr : MonoBehaviour
{
    [SerializeField]
    private GameObject StartBtn;
    [SerializeField]
    private GameObject Title;
    [SerializeField]
    private GameObject FadePanel;

    [Space(10)]
    [SerializeField]
    Camera MainCamera;

    [Space(10)]
    [SerializeField]
    bool StartGamechk = false;
    [SerializeField]
    float ShowTiem = 1.0f;

    [Space(10)]
    [SerializeField]
    private Vector2 TitleOriginPos;
    [SerializeField]
    private float MoveDis = 30.0f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(YetStartGame());

        StartBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            StartCoroutine(StartGame());
        });
    }

    IEnumerator YetStartGame()
    {
        yield return null;
        TitleOriginPos = Title.GetComponent<RectTransform>().anchoredPosition;

        while(StartGamechk == false)
        {
            yield return null;

            Title.GetComponent<RectTransform>().DOAnchorPosY(TitleOriginPos.y + MoveDis, ShowTiem * 6);
            yield return new WaitForSeconds(ShowTiem * 1.5f);
            Title.GetComponent<RectTransform>().DOAnchorPosY(TitleOriginPos.y - MoveDis, ShowTiem * 6);
            yield return new WaitForSeconds(ShowTiem * 1.5f);
        }
    }

    IEnumerator StartGame()
    {
        StartGamechk = true;

        yield return null;
        FadePanel.SetActive(true);
        FadeOutObj();
        yield return new WaitForSeconds(ShowTiem/3);
        StartCoroutine(CameraFocusUp());
        yield return new WaitForSeconds(ShowTiem);
        FadeInObj();
        yield return new WaitForSeconds(ShowTiem / 2);
        SceneManager.LoadScene("SelectStageScene");
    }

    void FadeOutObj()
    {
        Title.GetComponent<Image>().DOFade(0, ShowTiem/2);
        StartBtn.GetComponent<Image>().DOFade(0, ShowTiem/2);
    }

    void FadeInObj()
    {     
        FadePanel.GetComponent<Image>().DOFade(1, ShowTiem/1.2f);
    }

    IEnumerator CameraFocusUp()
    {
        yield return null;

        MainCamera.DOOrthoSize(2, ShowTiem / 1.3f);
    }
}
