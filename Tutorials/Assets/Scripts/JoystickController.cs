using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    //设置摇杆状态
    //Floating：浮动，点击任意位置出现摇杆
    //Fixed：固定，固定在某个位置上
    public enum JoystickStateEnum
    {
        Floating,
        Fixed
    }

    //摇杆出现的位置范围
    //LeftHalfScreen:左半屏幕
    //RightHalfScreen:右半屏幕
    //FullScreen:全屏幕
    public enum JoystickSafeAreaEnum
    {
        LeftHalfScreen,
        RightHalfScreen,
        FullScreen
    }


    public JoystickStateEnum joystickState;
    public JoystickSafeAreaEnum joystickSafeArea;
    
    //摇杆的组成部分 image
    [SerializeField] private Image JoystickBackground;
    [SerializeField] private Image JoystickHandler;

    //摇杆的背景Transform
    private RectTransform JoystickBackgroundTransform;

    //拖拽脚本
    [SerializeField] private JoystickHandler JoystickHandlerScript;

    //触摸的手指id
    private int TouchUniqueId = 0;

    //1/2屏幕范围
    private float HalfScreen = Screen.width / 2;


    private void Start()
    {
        JoystickBackgroundTransform = JoystickBackground.rectTransform;
        SetupJoystickImageState(!(joystickState == JoystickStateEnum.Floating));
        JoystickHandlerScript.SetJoystickHandler(JoystickHandler.rectTransform, 100);
    }

    /// <summary>
    /// 此处需要用
    /// </summary>
    private void FixedUpdate()
    {
        if (Input.touchCount <= 0) return;
        foreach (Touch tmp_Touch in Input.touches)
        {
            switch (tmp_Touch.phase)
            {
                case TouchPhase.Began:
                    switch (joystickSafeArea)
                    {
                        case JoystickSafeAreaEnum.LeftHalfScreen:
                            if (tmp_Touch.position.x < HalfScreen)
                            {
                                TouchUniqueId = tmp_Touch.fingerId;
                                MoveInTouch(tmp_Touch);
                            }

                            break;
                        case JoystickSafeAreaEnum.RightHalfScreen:
                            if (tmp_Touch.position.x > HalfScreen)
                            {
                                TouchUniqueId = tmp_Touch.fingerId;
                                MoveInTouch(tmp_Touch);
                            }

                            break;
                        case JoystickSafeAreaEnum.FullScreen:
                            TouchUniqueId = tmp_Touch.fingerId;
                            MoveInTouch(tmp_Touch);
                            break;
                    }

                    break;

                case TouchPhase.Ended:
                    //Hide joystick
                    if (tmp_Touch.fingerId == TouchUniqueId)
                        SetupJoystickImageState(false);
                    break;
            }
        }
    }


    /// <summary>
    /// 触摸触发
    /// </summary>
    /// <param name="_touch">触摸位置</param>
    private void MoveInTouch(Touch _touch)
    {
        switch (joystickState)
        {
            case JoystickStateEnum.Floating:
                //将摇杆移动到当前触摸位置
                Vector2 tmp_JoystickStartPosition = JoystickBackgroundTransform.position;
                tmp_JoystickStartPosition.x = _touch.position.x;
                tmp_JoystickStartPosition.y = _touch.position.y;
                JoystickBackgroundTransform.position = tmp_JoystickStartPosition;

                //重置摇杆knob中心点位置为当前触摸位置
                JoystickHandlerScript.SetHandlerPositin(tmp_JoystickStartPosition);

                //显示joystick
                SetupJoystickImageState(true);
                break;
            case JoystickStateEnum.Fixed:
                break;
        }
    }

    /// <summary>
    /// 设置当前摇杆的显示状态
    /// </summary>
    /// <param name="_state">True:显示，False:不显示</param>
    private void SetupJoystickImageState(bool _state)
    {
        JoystickBackground.enabled = _state;
        JoystickHandler.enabled = _state;
    }
}