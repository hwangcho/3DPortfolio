using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//커서변경 스크립트
public class setCursor : MonoBehaviour
{
    [SerializeField]
    Texture2D cursorImg;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImg, new Vector3(7f,5f,0f), CursorMode.ForceSoftware);
    }

    
}
