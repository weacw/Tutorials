using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //摇杆控制点
    private RectTransform JoystickHandlerTransform;

    //摇杆移动最大半径
    private float MaxRadius = 100;

    //摇杆初始化位置
    private Vector2 OriginalPosition;


    /// <summary>
    /// 设置摇杆移动对象，移动半径等
    /// </summary>
    /// <param name="_handler">摇杆移动knob</param>
    /// <param name="_radius">移动半径</param>
    public void SetJoystickHandler(RectTransform _handler, float _radius)
    {
        JoystickHandlerTransform = _handler;
        MaxRadius = _radius;
    }


    /// <summary>
    /// 设置knob的初始位置
    /// </summary>
    /// <param name="_position">当前joystick的位置</param>
    public void SetHandlerPositin(Vector2 _position)
    {
        OriginalPosition = _position;
    }


    /// <summary>
    /// 用户拖拽输入
    /// </summary>
    /// <param name="_eventData"></param>
    public void OnDrag(PointerEventData _eventData)
    {
        //计算滑动的位置与摇杆初始位置的朝向
        Vector2 tmp_Direction = _eventData.position - OriginalPosition;

        //移动的距离
        float tmp_Distance = Vector3.Magnitude(tmp_Direction);

        //限制移动的最大半径
        float tmp_Radius = Mathf.Clamp(tmp_Distance, 0, MaxRadius);

        //初始位置+ 移动的方向* 最大移动半径
        JoystickHandlerTransform.position = OriginalPosition + tmp_Direction.normalized * tmp_Radius;
    }


    /// <summary>
    /// 用户输入抬起
    /// </summary>
    /// <param name="_eventData"></param>
    public void OnPointerUp(PointerEventData _eventData)
    {
        JoystickHandlerTransform.position = OriginalPosition;
    }

    /// <summary>
    /// 用户输入按下
    /// </summary>
    /// <param name="_eventData"></param>
    public void OnPointerDown(PointerEventData _eventData)
    {
        OnDrag(_eventData);
    }
}