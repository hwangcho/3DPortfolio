using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TalkManager : MonoBehaviour
{
    public static TalkManager instance = null;

    public bool talking; //대화중인지
    int talkNum;//대화 순서
    public bool questOn;//퀘스트를 받앗는지
    bool questClear; //퀘스트를 클리어햇는지
    [SerializeField]
    GameObject steroid; //스테로이드 프리펩
    [SerializeField]
    GameObject talkBase;//토크 UI
    [SerializeField]
    Text talkTXT; //대화 text
    [SerializeField]
    Text questTXT;// 퀘스트 버튼 text

    [SerializeField]
    private GameObject go_SlotsParent;  // Slot들의 부모
    private Slot[] slots;  // 슬롯들 배열

    [SerializeField]
    Text questChickenTxt; //오른쪽 퀘스트창 ui
    int chickenLegCount; //모은 치킨개수

    void Awake()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();

        if (instance == null)
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
        //대화중 esc버튼누르면 꺼지게
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
    }
    private void FixedUpdate()
    {
        //퀘스트중이라면
        if (questOn && !questClear)
        {
            //슬롯의 갯수만큼 포문 돌려서
            //슬롯에 치킨이있는지 확인
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == "치킨")
                    {
                        chickenLegCount = slots[i].itemCount;//아이템 갯수
                        questChickenTxt.text = "닭다리 " + (chickenLegCount >=3 ? 3: chickenLegCount) + "/ 3";//퀘스트에 필요한 갯수가 꽉차면 필요한 갯수로 나오게 삼항연산자
                        return;

                    }
                }
            }
        }
    }
    //대화 시스템
    public void TalkSystem(int id)
    {
        //퀘스트 주는사람
        if(id == 10)
        {
            //퀘스트안받고 처음대화때
            if (!talking && !questOn && !questClear)
            {
                talkBase.SetActive(true);//대화 ui 활성화
                talking = true;//대화중
                talkTXT.text = "자네.. 내부탁좀 들어주겠나..";//텍스트 설정
                questTXT.text = "퀘스트";
            }
            //퀘스트를 받앗을때
            else if (!talking && questOn && !questClear)
            {
                talkBase.SetActive(true);
                talking = true;
                talkTXT.text = "닭다리를 다 모아왔는가?!";
                questTXT.text = "완료";
            }
            //퀘스트를 다깬후
            else if (!talking && questOn && questClear)
            {
                talkBase.SetActive(true);
                talking = true;
                talkTXT.text = "정말 고맙네!";
                questTXT.transform.parent.gameObject.SetActive(false);
            }

        }
        //상인
        if (id == 20)
        {
            questTXT.transform.parent.gameObject.SetActive(true);

            talkBase.SetActive(true);
            talking = true;
            talkTXT.text = "구매하실 물건이 있나요?";
            questTXT.text = "상점";

        }


         
    }
    //퀘스트 버튼
    public void QuestBottun()
    {
        //퀘스트 버튼의 텍스트가 상점이라면
        if(questTXT.text == "상점")
        {
            Shop.shopActive = !Shop.shopActive; //상점 활성화 반전
        }
        //퀘스트 버튼을 누르면
        if (talkNum == 0&&!questOn&& questTXT.text != "상점")
        {
            talkNum++;//숫자 증가시켜 다음행동하게
            talkTXT.text = "퀘스트 \n 마을에있는 닭을잡아 닭다리를 3개 구해오시오. \n\n 보상 : 스테로이드"; //텍스트 변경
            questTXT.text = "수락";

        }
        else if (talkNum == 1&&!questOn && questTXT.text != "상점")
        {
            questOn = true;//퀘스트 수락
            questChickenTxt.transform.parent.gameObject.SetActive(true);//오른쪽위 퀘스트 UI 활성화

            ExitButton();//ui닫힘
        }
        //퀘스트받고 다 모아오면
        else if (questOn&&!questClear&&chickenLegCount >=3 && questTXT.text != "상점")
        {
            questClear = true;
            talkTXT.text = "정말 고맙네!\n약속했던 스테로이드라네";

            questChickenTxt.transform.parent.gameObject.SetActive(false);
            questTXT.transform.parent.gameObject.SetActive(false);

            go_SlotsParent.transform.parent.transform.parent.GetComponent<Inventory>().AcquireItem(steroid.GetComponent<ItemPickup>().item); //퀘스트 보상 아이템 템창에 넣어줌
            //템창에있는 퀘스트아이템 3개 감소시킴
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == "치킨")
                    {

                        slots[i].SetSlotCount(-3);
                        return;

                    }
                }
            }
            

        }

    }
    //대화 종료버튼
    public void ExitButton()
    {
        talkBase.SetActive(false);//비활성화
        talkNum = 0;//처음대화로 돌아가게
        Shop.shopActive = false;

        StartCoroutine(a());//뭔가 이상해서 0.1초후에 talking false로

    }
    IEnumerator a()
    {
        yield return new WaitForSeconds(0.1f);
        talking = false;
    }
}
