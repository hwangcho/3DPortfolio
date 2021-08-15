using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{

    [SerializeField]
    Vector3 cameraRot;//카메라 회전값
    [SerializeField]
    Vector3 playerRot;//플레이어 회전값
    [SerializeField]
    GameObject nextGate;//다음 게이트
    [SerializeField]
    GameObject fBtn;// 버튼이미지
    [SerializeField]
    GameObject mainLight;//마을 라이트
    [SerializeField]
    GameObject boss;//보스

    [SerializeField]
    int potalNum;//포탈번호
  

    private void OnTriggerStay(Collider other)
    {
        //플레이어가 포탈에 들어가있으면 f버튼 활성화
        if(other.tag=="Player")
            fBtn.SetActive(true);

        //f버튼 눌럿을때
        //포탈 넘버에 따라 가는곳 다르게
        if (Input.GetKey(KeyCode.F))
        {
            
            if (potalNum == 0 && other.tag == "Player" && !GameManager.instance.Loading && other.GetComponent<Player>().moveInput.magnitude == 0)
            {
                GameManager.instance.LoadingOn();//로딩 시작
                other.transform.position = nextGate.transform.position;//넣어준 다음 게이트로 이동
                //플레이어와 카메라 회전값 초기화
                other.transform.GetChild(0).rotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
                other.transform.GetChild(1).rotation = Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z);
                mainLight.SetActive(true);//마을 라이트 활성화
                StartCoroutine(BGMOn(0,0.9f));//음악 실행
                SpawnManager.instance.AllOFF();//모든 몬스터 비활성화

            }
            else if (potalNum == 1 && other.tag == "Player" && !GameManager.instance.Loading && other.GetComponent<Player>().moveInput.magnitude == 0)
            {
                GameManager.instance.LoadingOn();
                other.transform.position = nextGate.transform.position;
                other.transform.GetChild(0).rotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
                other.transform.GetChild(1).rotation = Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z);
                mainLight.SetActive(false);
                StartCoroutine(BGMOn(1,0.6f));


            }
            else if (potalNum == 2 && other.tag == "Player" && !GameManager.instance.Loading && other.GetComponent<Player>().moveInput.magnitude == 0)
            {
                GameManager.instance.LoadingOn();
                other.transform.position = nextGate.transform.position;
                other.transform.GetChild(0).rotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
                other.transform.GetChild(1).rotation = Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z);
                mainLight.SetActive(false);
                StartCoroutine(BGMOn(2,0.4f));



            }
            else if (potalNum == 3 && other.tag == "Player" && !GameManager.instance.Loading && other.GetComponent<Player>().moveInput.magnitude == 0)
            {
                GameManager.instance.LoadingOn();
                other.transform.position = nextGate.transform.position;
                other.transform.GetChild(0).rotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
                other.transform.GetChild(1).rotation = Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z);
                SpawnManager.instance.AllRespawn();
                mainLight.SetActive(false);
                StartCoroutine(BGMOn(3,1));

            }
            else if (potalNum == 4 && other.tag == "Player" && !GameManager.instance.Loading && other.GetComponent<Player>().moveInput.magnitude == 0)
            {
                GameManager.instance.LoadingOn();
                other.transform.position = nextGate.transform.position;
                other.transform.GetChild(0).rotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
                other.transform.GetChild(1).rotation = Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z);

                StartCoroutine(bossOn());//보스 시작함수
                SpawnManager.instance.AllOFF();
            }
        }
        //0번 마을
        //1번 마켓
        //2번 헬스장
        //3번 던전
        //4번 보스던전
    }

    //플레이어가 포탈밖으로나오면 f버튼 비활성화
    private void OnTriggerExit(Collider other)
    {
            fBtn.SetActive(false);

    }
    //배경음 코루틴
    IEnumerator BGMOn(int BgmNum,float volum)
    {
        SoundManager.instance.BGMStop();//배경음 멈춤
        yield return new WaitForSeconds(3.8f);
        SoundManager.instance.BGMPlay(BgmNum);//bgmNum에 맞는 배경음 실행
        SoundManager.instance.SetBGMVolumn(volum);//볼륨

    }
    //보스시작
    IEnumerator bossOn()
    {
        
        SoundManager.instance.BGMStop();
        yield return new WaitForSeconds(2.5f);
        SoundManager.instance.BGMPlay(4);
        SoundManager.instance.SetBGMVolumn(0.8f);

        GameObject Boss = Instantiate(boss, new Vector3(243.29f, 0, 148.01f), Quaternion.Euler(0, 220, 0));//보스 생성위치
        
        yield return new WaitForSeconds(4.4f);
        Boss.GetComponent<Bull>().enabled = true;//보스 스크립트 활성화
        Boss.GetComponent<Bull>().attackTime = Time.time-5.5f;//보스 공격시간 설정


    }
}
