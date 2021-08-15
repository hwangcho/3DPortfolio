using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//비동기 로딩
public class LoadingScene : MonoBehaviour
{
    public Slider slider;//로딩 슬라이더

    float fTime = 0f;//시간
    AsyncOperation async_operation; //씬이동관리

    void Start()
    {
        StartCoroutine(StartLoad("MainGame"));//시작시 코룬틴실행
    }

    void Update()
    {
        
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(strSceneName);//전환될 씬 넣어줌
        async_operation.allowSceneActivation = false;//씬에 넘어갈수 없게

      
        while (!async_operation.allowSceneActivation)//씬에 못넘어가면 무한반복
        {
            fTime += Time.deltaTime;//시간 증가
            slider.value = fTime;//슬라이더값에 넣어줌

            if (fTime >= 5)//5초가 지나면
            {
                async_operation.allowSceneActivation = true;//씬 넘어갈수있게해서 씬에넘어감
            }
            yield return null;
        }
        
    }

}
