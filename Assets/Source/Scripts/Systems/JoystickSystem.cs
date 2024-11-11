using UnityEngine;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// в GameData надо добавить public Vector3 Joystick - это и будут значения джойстика
public class JoystickSystem : GameSystem
{
    [SerializeField] bool pointerOverUICheck = true;
    [SerializeField] bool convertXYToXZ = true;
    [SerializeField] float cameraYAngle = 0f;
    [SerializeField] float minJoystickSens = 25;

    [Header("Screen")]
    [SerializeField] RectTransform backGround;
    [SerializeField] RectTransform handle;
    [SerializeField] float handleMaxOffset = 75;

    bool touched;
    float screenScaler;
    Vector3 touchPos;
    Vector3 output;

    public override void OnInit()
    {
        // скаляр для точного определения координат на экране
        screenScaler = FindObjectOfType<CanvasScaler>().referenceResolution.y / (float)Screen.height;

        if (backGround && handle)
        {
            backGround.gameObject.SetActive(false);
            // Якорь Бэкграунда должен быть в левом нижнем углу
            backGround.anchorMin = new Vector2(0, 0);
            backGround.anchorMax = new Vector2(0, 0);
            // Якорь стика должен быть в центре
            handle.anchorMin = new Vector2(0.5f, 0.5f);
            handle.anchorMax = new Vector2(0.5f, 0.5f);
            // Стик должен быть дочерним к бэкграунду
            handle.transform.SetParent(backGround.transform);
            // убираем RayCast Target у джойстика
            backGround.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            handle.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
        }
    }
    public override void OnUpdate()
    {
        // при нажатии проверяем не ткнул ли игрок в какую-то UI'ку
        if (Input.GetMouseButtonDown(0) && !PointerOverUI())
        {
            // палец нажат
            touched = true;

            // обновляем место тыка пальцем
            touchPos = Input.mousePosition;

            // включем UI джойстика
            if (backGround)
            {
                backGround.gameObject.SetActive(true);
                backGround.anchoredPosition = Input.mousePosition * screenScaler;
            }
        }

        // при отпускании сюда можно добавить дополнительные параметры и проверки, чтобы игрок не тапал в какие-то моменты
        if (Input.GetMouseButtonUp(0))
            touched = false;

        if (touched)
        {
            // Если палец сдвинули слишком мало или после движения решили остановиться не отпуская палец
            if ((Input.mousePosition - touchPos).magnitude < minJoystickSens)
                output = Vector3.zero;
            else
                // отклонение пальца от места нажатия
                output = (Input.mousePosition - touchPos).normalized;

            // Выставляем стик на нужное место
            if (handle)
                handle.anchoredPosition = output * handleMaxOffset;

            // онвертируем из вертикальной плоскости в горизонтальную
            if (convertXYToXZ)
            {
                output.z = output.y;
                output.y = 0;
            }

            // Угол поворота камеры
            output = Quaternion.Euler(0, cameraYAngle, 0) * output;
            game.Joystick = output;
        }
        else
        {
            // Если палец отпущен, то движение нулевое
            game.Joystick = Vector3.zero;
            if (backGround)
                backGround.gameObject.SetActive(false);
        }
    }

    // Проверка не навёл ли игрок на ЮАйку какую-то, рекомендую к использованию
    public bool PointerOverUI()
    {
        if (!pointerOverUICheck || EventSystem.current == null)
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
