using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Screen;

namespace Script
{
    public class FixedDisplay : MonoBehaviour
    {
        // private int deviceWidth;
        // private int deviceHeight;
        // public void Awake()
        // {
        //     SetResolution(); // 초기에 게임 해상도 고정
        // }
        //
        // private void SetUpCanvasScaler(int setWidth, int setHeight)
        // {
        //     CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
        //     canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //     canvasScaler.referenceResolution = new Vector2(setWidth, setHeight);
        //     canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        // }
        //
        // public void SetResolution(int setWidth = 1920, int setHeight = 1080)
        // {
        //     SetUpCanvasScaler(setWidth, setHeight);
        //
        //     if(deviceWidth.Equals(width) == false)
        //     {
        //         deviceWidth = width; // 기기 너비 저장
        //     }
        //
        //     if (deviceHeight.Equals(height) == false)
        //     {
        //         deviceHeight = height; // 기기 높이 저장
        //     }
        //
        //     //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
        //
        //     if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        //     {
        //         float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
        //         if (Camera.main != null)
        //         {
        //             Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        //         }
        //     }
        //     else // 게임의 해상도 비가 더 큰 경우
        //     {
        //         float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
        //         if (Camera.main != null)
        //         {
        //             Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        //         }
        //     }
        // }
    }
}