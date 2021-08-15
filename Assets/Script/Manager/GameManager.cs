using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //싱글톤 사용
    public static GameManager instance = null;
    [SerializeField]
    GameObject player;
    [SerializeField]
    Image loadingPanel;//로딩 검은 배경
    [SerializeField]
    Slider loadingSlider;//로딩 슬라이더
    [SerializeField]
    GameObject diePanel;//죽엇을때 나오는 패널
    [SerializeField]
    GameObject pausePanel;//Pauer패널
    public bool Loading;//로딩중인지
    float loadingTime;//로딩시간

    [SerializeField]
    GameObject mainLight;//마을 라이트
    public bool pauseOn;//Pause중인지
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    
    void Update()
    {
  
        LoadingSystem();
        DieSystem();
        Pause();
    }
    //로딩함수
    public void LoadingOn()
    {

        Loading = true;//로딩중
        loadingPanel.gameObject.SetActive(true);//로딩 배경 활성화
        loadingPanel.color = new Color(0, 0, 0, 1);//로딩 배경색 검정색
        loadingSlider.transform.parent.gameObject.SetActive(true);//로딩 슬라이더 활성화
        loadingSlider.value = 0;//슬라이더 값 초기화
        loadingTime = Time.time;//로딩시간 초기화

        player.GetComponentInChildren<Animator>().Rebind();//플레이어 실행중이던 애니메이터 초기화(구르거나 뛰면서 로딩창들어가면 이상해짐방지

        player.GetComponentInChildren<Inventory>().CloseInventory();//인벤토리 종료
        Inventory.invectoryActivated = false;
    }
    //마을 라이트온
    void LightOn()
    {
        mainLight.SetActive(true);

    }
    //죽었을때 로딩
    void DeathLoadingOn()
    {

        Loading = true;
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.color = new Color(0, 0, 0, 1);
        loadingSlider.transform.parent.gameObject.SetActive(true);
        loadingSlider.value = 0;
        loadingTime = Time.time;
        //위까지 로딩시스템 밑에는 플레이어 관련 설정
        player.transform.position = new Vector3(-8.449997f, 0.7792221f, -18.39f);//플레이어 위치 마을로 
        player.transform.GetChild(1).rotation = Quaternion.Euler(Vector3.zero);//카메라,플레이어 회전값 초기화
        player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(20, 0, 0));

        StatManager.instance.p_curHealth = StatManager.instance.p_maxHealth;//체력 초기화
        player.GetComponentInChildren<Inventory>().CloseInventory();//인벤토리 닫기
        Inventory.invectoryActivated = false;
        Invoke("LightOn", 3);//마을 라이트 켜기

    }
    //로딩 시스템
    void LoadingSystem()
    {
        //로딩이 true가되면 슬라이더값 증가
        if (Loading)
        {
            
            loadingSlider.value += (Time.time - loadingTime) / 3f;
        }
        //로딩슬라이더값 최대로차면
        if(loadingSlider.value == loadingSlider.maxValue)
        {
            loadingSlider.transform.parent.gameObject.SetActive(false);//슬라이더 끄고
        loadingPanel.color = new Color(0, 0, 0, loadingPanel.color.a -0.02f);//로딩 배경 점점 밝아지게

        }
        //죽엇을때 배경 다 밝아졌으면
        if (loadingPanel.color.a <= 0&&Loading&&PlayerHealth.Death)
        {
            Loading = false;
            PlayerHealth.Death = false;//죽음 초기화

            loadingPanel.gameObject.SetActive(false); //로딩패널 비활성화
            player.GetComponentInChildren<Animator>().SetTrigger("Revive");//되살아나는 애니 실행
            player.GetComponent<Player>().hitTime = Time.time + 0.8f;//힛타임 초기화
            SoundManager.instance.BGMPlay(0);//음악 초기화
            SoundManager.instance.SetBGMVolumn(0.9f);//볼륨 조절

        }
        //죽지않고 로딩때 다밝아지면
        if (loadingPanel.color.a <= 0 && Loading && !PlayerHealth.Death)
        {
            Loading = false;//로딩 초기화
            loadingPanel.gameObject.SetActive(false);//로딩 배경 비활성화

        }
    }
    //죽었을때 패널
    public void DiePanelOn()
    {
        diePanel.SetActive(true);//활성화
        //색상 초기화
        diePanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        diePanel.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0, 0, 0);

    }
    //죽엇을때
    void DieSystem()
    {
        //플레이어가 죽었고 죽음패널 활성화되어있다면
        if (PlayerHealth.Death && diePanel.activeSelf)
        {
            diePanel.GetComponent<Image>().color = new Color(0, 0, 0, diePanel.GetComponent<Image>().color.a + 0.02f);//색상 점점 어둡게
            diePanel.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0, 0, diePanel.transform.GetChild(0).GetComponent<Text>().color.a + 0.02f);//텍스트 색상도 점점진하게

        }
        //다 어두워졋다면
        if (diePanel.transform.GetChild(0).GetComponent<Text>().color.a>=1 && diePanel.GetComponent<Image>().color.a >= 1&&PlayerHealth.Death&&diePanel.activeSelf)
        {
            DeathLoadingOn();
            diePanel.SetActive(false);
            SoundManager.instance.BGMStop();//음악종료

        }

    }
    //정지
    void Pause()
    {
        //esc 누르고 , 인벤토리,샵,대화,로딩중이 아닐때
        if (Input.GetKeyDown(KeyCode.Escape) && !Inventory.invectoryActivated && !Shop.shopActive&& !TalkManager.instance.talking&&!Loading)
        {
            pauseOn = !pauseOn; //반전
        }
        //pause 중엔
        if (pauseOn)
        {
            Time.timeScale = 0; //시간멈춤
            pausePanel.SetActive(true); //패널 활성화
            SoundManager.instance.Pause();//음악 중지
        }
        //아닐땐
        else if (!pauseOn)
        {
            Time.timeScale = 1;//다시 시간감
            pausePanel.SetActive(false);
            SoundManager.instance.Unpause();//음악 다시나옴

        }
    }
    
    //pause중 resume버튼눌럿을때
    public void ResumeButton()
    {
        pauseOn = !pauseOn;
    }
    //Quit 버튼누르면 꺼지게
    public void QuitButton()
    {
        
        Application.Quit();
    }
}
