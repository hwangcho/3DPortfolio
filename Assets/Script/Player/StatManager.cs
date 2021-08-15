using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    //싱글톤 사용
    public static StatManager instance = null;

    string p_name; //플레이어 이름
    public int p_level;// 레벨
    public int p_maxEXP;//레벨업에 필요한 경험치
    public int p_curEXP;//현재경험치
    public int p_maxHealth;//최대체력
    public int p_curHealth;//현재체력
    public float p_maxMP;//최대스태미너
    public float p_curMP;//현재 스태미너
    public float p_MpTime;//스태미너 회복시간
    public int p_attackMin;//최저대미지
    public int p_attackMax;//최대대미지
    public int p_str;//스텟 힘
    public int p_dex;//스텟 지구력
    public int p_faith;//스텟 신앙
    public int p_statPoint; //스탯 포인트
    public int Skill1Min; //
    public int Skill1Max; //
    public int Skill2Max; //
    public int Skill2Min; //
  
    float mpPotionTime; //스태미나 포션 시간
    public int coin; //돈

    [SerializeField]
    Slider hpSlider;
    [SerializeField]
    Slider mpSlider;
    [SerializeField]
    Slider expSlider;
    [SerializeField]
    Text Level;
    [SerializeField]
    Text hpText;

    [SerializeField]
    Text expText;
    [SerializeField]
    Text curStrTxt;
    [SerializeField]
    Text curDexTxt;
    [SerializeField]
    Text curFaithTxt;
    [SerializeField]
    Text curSkillPoint;
    [SerializeField]
    Image BcaaTime;
    [SerializeField]
    Text CoinText;
    [SerializeField]
    GameObject levelUpEffect;
    public Image skill1Img;

    public Image skill2Img;

    public int equipStr;//무기착용 공격력
    int strStat; //힘 스탯

    void Awake()
    {
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
        p_name = "김주형";
        p_level = 1;
        p_maxEXP = 100 * p_level;
        p_curEXP = 0;
        p_maxHealth = 100;
        p_curHealth = p_maxHealth;
        p_maxMP = 100;
        p_curMP = p_maxMP;
        p_MpTime = Time.time;
        p_attackMin = 5;
        p_attackMax = 11;
        p_str = 1;
        p_dex = 1;
        p_faith = 1;
        p_statPoint = 0;
        Skill1Min = 8;
        Skill1Max = 15;
        Skill2Max = 12;
        Skill2Min = 6;

        mpPotionTime = Time.time - 10;
    }

    
    void FixedUpdate()
    {
        //UI들 계속 업데이트
        hpSlider.maxValue = p_maxHealth;
        hpSlider.value = p_curHealth;
        mpSlider.maxValue = p_maxMP;
        mpSlider.value = p_curMP;
        expSlider.maxValue = p_maxEXP;
        expSlider.value = p_curEXP;
        Level.text = p_level.ToString();
        hpText.text = p_curHealth + " / " + p_maxHealth;
        expText.text = p_curEXP + " / " + p_maxEXP;
        curStrTxt.text = p_str.ToString();
        curDexTxt.text = p_dex.ToString();
        curFaithTxt.text = p_faith.ToString();
        curSkillPoint.text = p_statPoint.ToString();
        CoinText.text = coin.ToString();
        SkillImg();
        MPSystem();
        LevelSystem();
        EquipSTRSystem();

    }
    //스킬 쿨타임 이미지
    void SkillImg()
    {
        if (skill1Img.fillAmount < 1)
            skill1Img.fillAmount += Time.deltaTime / 7;
        if (skill2Img.fillAmount < 1)
            skill2Img.fillAmount += Time.deltaTime / 12;

    }
    //스태미나 관련 함수
    void MPSystem()
    {
        //스태미나를 다써버리면 스태미나 회복시간 더늦춤
        if(p_curMP < 0)
            {
            p_curMP = 0;
            p_MpTime = Time.time + 1.2f;
        }
        //포션 지속시간이 아닐떈 0.8씩회복
        if (Time.time - p_MpTime > 0.8f&& p_maxMP > p_curMP && Time.time-mpPotionTime >15f)
        {                  
            p_curMP += 0.8f;
        //포션 지속시간이라면 1.5씩회복
        }else if(Time.time - p_MpTime > 0.8f && p_maxMP > p_curMP && Time.time - mpPotionTime <= 15f)
        {
            p_curMP += 1.4f;
           
        }
        //포션 끝나는거처럼 보이게
        //이미지 fillAmount로 흐리게해줌
        if(Time.time - mpPotionTime <= 15)
        {
            BcaaTime.fillAmount -= Time.deltaTime / 15;
        }
        else
        {
            BcaaTime.gameObject.SetActive(false);
        }
    }
    //경험치 획득
    public void getEXP(int exp)
    {
        p_curEXP += exp;
      
    }
    //레벨업 시스템
    void LevelSystem()
    {
        //최대경험치보다 현재경험치가 높으면
        if (p_maxEXP <= p_curEXP)
        {
            StartCoroutine(levelUpEffectOn());//레벨업 이펙트
            p_level++;//레벨상승
            p_curEXP = p_curEXP - p_maxEXP; //경험치 초기화 및 초과한 경험치 남겨줌
            p_maxEXP = 100 * p_level; //레벨업시 필요한 경험치 추가
            p_maxHealth += 10;//체력 증가
            p_curHealth = p_maxHealth;//체력회복
            hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(hpSlider.GetComponent<RectTransform>().sizeDelta.x + 10, hpSlider.GetComponent<RectTransform>().sizeDelta.y); //체력 슬라이더 길이 증가
            p_statPoint += 2;//스탯포인트 증가
        }
    }
    //레벨업 이펙트 
    IEnumerator levelUpEffectOn()
    {
        //효과음 내면서 활성화 시킨후 비활성화
        SoundManager.instance.Play("LevelUp");

        levelUpEffect.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        levelUpEffect.SetActive(false);

    }
    //원판착용시 공격력 증가 함수
    void EquipSTRSystem()
    {
        //플레이어의 무기장착 힘증가량 + 스탯포인트로 올린힘 에따라 공격력 증가
        p_str = equipStr + 1 + strStat ;
        p_attackMin = 5 + (equipStr *2) + (strStat *2);
        p_attackMax = 11 + (equipStr * 2) + (strStat * 2);
        Skill1Min = 8 + (equipStr * 2) + (strStat * 2);
        Skill1Max = 15 + (equipStr * 2) + (strStat * 2);
        Skill2Max = 12 + (equipStr * 2) + (strStat * 2);
        Skill2Min = 6 + (equipStr * 2) + (strStat * 2);
    }
    //힘스탯 증가
    public void STRup()
    {
        if (p_statPoint >= 1)
        {
            p_statPoint -= 1;
            strStat += 1;
        }
        else
            return;

    }
    //dex 스탯 증가 (스태미나
    public void DexUp()
    {
        if (p_statPoint >= 1)
        {
            p_statPoint -= 1;
            p_dex += 1;
            //최대,현재 스태미나 증가시키고
            //슬라이더 길이 증가
            p_maxMP += 10;
            p_curMP += 10;
            mpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(mpSlider.GetComponent<RectTransform>().sizeDelta.x + 10, mpSlider.GetComponent<RectTransform>().sizeDelta.y);
        }
        else
            return;

    }
    //신성 증가
    public void FaithUp()
    {
        if (p_statPoint >= 1)
        {
            p_statPoint -= 1;
            p_faith += 1;
        }
        else
            return;
    }
    //체력회복 아이템 먹엇을때
    public void IncreaseHP(int _count)
    {
        //최대체력보다 낮을때
        if (p_curHealth + _count < p_maxHealth)
            p_curHealth += _count;
        else//최대체력이면 현재체력을 최대체력으로
            p_curHealth = p_maxHealth;

    }
    //스태미나 포션 먹엇을때
    public void IncreaseMP(int _count)
    {
        mpPotionTime = Time.time;//시간초기화
        BcaaTime.gameObject.SetActive(true);//스태미나 이미지 활성화
        BcaaTime.fillAmount = 1;//1로 초기화

    }
    //최대체력,스태미나 증가
    public void IncreaseMaxHPMp(int _count)
    {
        p_maxHealth += _count;
        p_maxMP += _count;
        mpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(mpSlider.GetComponent<RectTransform>().sizeDelta.x + _count, mpSlider.GetComponent<RectTransform>().sizeDelta.y);
        hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(hpSlider.GetComponent<RectTransform>().sizeDelta.x + _count, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
        p_curHealth = p_maxHealth;
        p_curMP = p_maxMP;

    }
   
}
