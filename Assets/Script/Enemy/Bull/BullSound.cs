using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullSound : MonoBehaviour
{
    //효과음 플레이 함수들
    public void Atk1Sound()
    {
        SoundManager.instance.Play("BullAttack1");
    }
    public void Atk2Sound()
    {
        SoundManager.instance.Play("BullAttack2");
    }
    public void Atk3Sound()
    {
        SoundManager.instance.Play("BullAttack3");
    }
    public void Atk4Sound()
    {
        SoundManager.instance.Play("BullAttack4");
    }
    public void JumpAtkSound()
    {
        SoundManager.instance.Play("JumpAttack");
    }
    public void GrowlSound()
    {
        SoundManager.instance.Play("Growl");
    }
}
