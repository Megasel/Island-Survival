using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Свернуть все области Ctrl + M + L
// DOTween.To(() => balance, x => balance = x, to, 2);
// StartCoroutine(CoroutineLibrary.InvokeAfterTime(1f, () => Bootstrap.ChangeGameState(EGamestate.Result);}));
// ListType ItemName = inventario.Find((x) => x.VarName == VarValue); // Искать внутри листа по переменной
// transform.position = Cam.WorldToScreenPoint(AllResources[i].position);
// int index = pricePublicList.FindIndex(item => item.Size == 200);
/*
private Queue<Rigidbody> RigidsToDestroy = new Queue<Rigidbody>();
private IEnumerator DestroyVoxel()
{
	float timestamp = Time.realtimeSinceStartup;
	while (true)
	{
		if (RigidsToDestroy.Count > 0)
		{
			///!!!!Action!!!!
		}
		else
		{
			yield return new WaitForFixedUpdate();
			timestamp = Time.realtimeSinceStartup;
		}
		if (Time.realtimeSinceStartup - timestamp > 0.005f)
		{
			yield return new WaitForFixedUpdate();
			timestamp = Time.realtimeSinceStartup;
		}
	}
}*/
public static class My : System.Object
{
    public static void Log(object message)
    {
        Color color = Color.green;
        //Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), message));
    }

    public static string ScoreLerp(this UnityEngine.UI.Text txt, float count, float t)
    {
        float startCount = int.Parse(txt.text);
        float result = Mathf.RoundToInt(Mathf.Lerp(startCount, count, t));
        if (startCount == result)
            if (result < count)
                result++;
            else if (result > count)
                result--;
        return txt.text = result.ToString("F0");
    }
    public static int FibonacciNumber(int n)
    {
        int a = 1;
        int b = 1;
        for (int i = 3; i <= n; i++)
        {
            int c = a + b;
            a = b;
            b = c;
        }
        return b;
    }
    //Видны ли все части и подчасти таргета из камеры
    public static bool IsVisible(this GameObject target)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Bounds rendererBounds;
        if (null != target.GetComponent<Renderer>())
        {
            rendererBounds = target.GetComponent<Renderer>().bounds;
        }
        else
        {
            rendererBounds = new Bounds(target.transform.position, Vector3.zero);
            Component[] meshes = target.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter mesh in meshes)
            {
                rendererBounds.Encapsulate(mesh.GetComponent<Renderer>().bounds);
            }
        }
        if (GeometryUtility.TestPlanesAABB(planes, rendererBounds))
            return true;
        else
            return false;
    }
    public static bool IsVisible(this GameObject target, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        Bounds rendererBounds;
        if (null != target.GetComponent<Renderer>())
        {
            rendererBounds = target.GetComponent<Renderer>().bounds;
        }
        else
        {
            rendererBounds = new Bounds(target.transform.position, Vector3.zero);
            Component[] meshes = target.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter mesh in meshes)
            {
                rendererBounds.Encapsulate(mesh.GetComponent<Renderer>().bounds);
            }
        }
        if (GeometryUtility.TestPlanesAABB(planes, rendererBounds))
            return true;
        else
            return false;
    }

    //Плавный переход из одного в другое. Плавный старт и плавный финиш
    public static float Interpolate(float Target, float Start, float End, float TimeTo, bool Smooth = true)
    {
        float range = End - Start;

        if (range > 0 && (Target > Start + range || Target < End - range))
            Target = Start + Time.deltaTime * range / (100 * TimeTo);
        if (range < 0 && (Target < Start + range || Target > End - range))
            Target = Start + Time.deltaTime * range / (100 * TimeTo);

        float Percent = (Target - Start) / range;

        if (Target != End)
        {
            if (Smooth)
                Target += range * Time.deltaTime * Mathf.Min(2f, Mathf.Max(0.27f, Percent * 8f), Mathf.Max(0.27f, (1f - Percent) * 8f)) / TimeTo;
            else
                Target += range * Time.deltaTime / TimeTo;
        }
        if (range > 0 && Target > End)
        {
            Target = End;
            //Percent = 1;
        }
        if (range < 0 && Target < End)
        {
            Target = End;
            //Percent = 1;
        }
        return Target;
    }
    public static float Interpolate(float Target, float Start, float End, float TimeTo, out float Percent, bool Smooth = true)
    {
        float range = End - Start;
        if (range > 0 && (Target > Start + range || Target < End - range))
            Target = Start + Time.deltaTime * range / (100 * TimeTo);
        if (range < 0 && (Target < Start + range || Target > End - range))
            Target = Start + Time.deltaTime * range / (100 * TimeTo);

        Percent = (Target - Start) / range;

        if (Target != End)
        {
            if (Smooth)
                Target += range * Time.deltaTime * Mathf.Min(2f, Mathf.Max(0.27f, Percent * 8f), Mathf.Max(0.27f, (1f - Percent) * 8f)) / TimeTo;
            else
                Target += range * Time.deltaTime / TimeTo;
        }
        if (range > 0 && Target > End)
        {
            Target = End;
            Percent = 1;
        }
        if (range < 0 && Target < End)
        {
            Target = End;
            Percent = 1;
        }
        return Target;
    }
    public static Vector3 Interpolate(Vector3 Target, Vector3 Start, Vector3 End, float TimeTo, bool Smooth = true)
    {
        Vector3 Temp = new Vector3();
        Temp.x = Interpolate(Target.x, Start.x, End.x, TimeTo, Smooth);
        Temp.y = Interpolate(Target.y, Start.y, End.y, TimeTo, Smooth);
        Temp.z = Interpolate(Target.z, Start.z, End.z, TimeTo, Smooth);
        Target = Temp;
        return Target;
    }
    public static Vector2 Interpolate(Vector2 Target, Vector2 Start, Vector2 End, float TimeTo, bool Smooth = true)
    {
        Vector2 Temp = new Vector2();
        Temp.x = Interpolate(Target.x, Start.x, End.x, TimeTo, Smooth);
        Temp.y = Interpolate(Target.y, Start.y, End.y, TimeTo, Smooth);
        Target = Temp;
        return Target;
    }
    public static Quaternion Interpolate(Quaternion Target, Quaternion Start, Quaternion End, float TimeTo, bool Smooth = true)
    {
        Quaternion Temp = new Quaternion();
        Temp.x = Interpolate(Target.x, Start.x, End.x, TimeTo, Smooth);
        Temp.y = Interpolate(Target.y, Start.y, End.y, TimeTo, Smooth);
        Temp.z = Interpolate(Target.z, Start.z, End.z, TimeTo, Smooth);
        Temp.w = Interpolate(Target.w, Start.w, End.w, TimeTo, Smooth);
        Target = Temp;
        return Target;
    }

    //Ближайший или дальний к таргету из списка
    public static Transform Closest(this Transform Target, List<Transform> List)
    {
        int index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static GameObject Closest(this GameObject Target, List<GameObject> List)
    {
        int index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static GameObject Closest(this GameObject Target, List<GameObject> List, out int index)
    {
        index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static GameObject Closest(this List<GameObject> List, GameObject Target)
    {
        int index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static Transform Closest(this List<Transform> List, Transform Target)
    {
        int index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static Transform Closest(this Transform Target, List<Transform> List, out int index)
    {
        index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static GameObject Closest(this List<GameObject> List, GameObject Target, out int index)
    {
        index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static Transform Closest(this List<Transform> List, Transform Target, out int index)
    {
        index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return List[index];
    }
    public static Transform Farest(this Transform Target, List<Transform> List)
    {
        int index = 0;
        float max = 0;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist > max)
            {
                index = i;
                max = dist;
            }
        }
        return List[index];
    }
    public static Transform Farest(this Transform Target, List<Transform> List, out int index)
    {
        index = 0;
        float max = 0;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].position, Target.position);
            if (dist > max)
            {
                index = i;
                max = dist;
            }
        }
        return List[index];
    }
    public static GameObject Farest(this GameObject Target, List<GameObject> List)
    {
        int index = 0;
        float max = 0;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist > max)
            {
                index = i;
                max = dist;
            }
        }
        return List[index];
    }
    public static GameObject Farest(this GameObject Target, List<GameObject> List, out int index)
    {
        index = 0;
        float max = 0;
        for (int i = 0; i < List.Count; i++)
        {
            float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
            if (dist > max)
            {
                index = i;
                max = dist;
            }
        }
        return List[index];
    }

    //Перемешать список
    public static List<GameObject> Shuffle(this List<GameObject> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<int> Shuffle(this List<int> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<float> Shuffle(this List<float> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<Color> Shuffle(this List<Color> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<Transform> Shuffle(this List<Transform> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<Vector2> Shuffle(this List<Vector2> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static List<Vector3> Shuffle(this List<Vector3> List)
    {
        for (int i = List.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static GameObject[] Shuffle(this GameObject[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static int[] Shuffle(this int[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static float[] Shuffle(this float[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static Color[] Shuffle(this Color[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static Transform[] Shuffle(this Transform[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static Vector2[] Shuffle(this Vector2[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }
    public static Vector3[] Shuffle(this Vector3[] List)
    {
        for (int i = List.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = List[j];
            List[j] = List[i];
            List[i] = temp;
        }
        return List;
    }


    //Возвращает нормаль к террайну
    public static Vector3 SampleNormal(Vector3 position)
    {
        Terrain terrain = Terrain.activeTerrain;
        var terrainLocalPos = position - terrain.transform.position;
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
            Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
        );
        var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        return terrainNormal;
    }
    public static Vector3 SampleNormal(Vector3 position, Terrain terrain)
    {
        var terrainLocalPos = position - terrain.transform.position;
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
            Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
        );
        var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        return terrainNormal;
    }

    //Ставит геймобджект на террайн и под углом нормали
    public static Transform PlaceOnTerrain(this Transform transform, bool SetHeight = true)
    {
        Vector3 sample = SampleNormal(transform.position);
        Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, sample)) * sample;
        transform.rotation = Quaternion.LookRotation(proj, sample);
        //высота по террайну
        if (SetHeight)
            transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z);
        return transform;
    }
    public static Transform PlaceOnTerrain(this Transform transform, Terrain terrain, bool SetHeight = true)
    {
        Vector3 sample = My.SampleNormal(transform.position, terrain);

        Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, sample)) * sample;
        transform.rotation = Quaternion.LookRotation(proj, sample);
        //высота по террайну
        if (SetHeight)
            transform.position = new Vector3(transform.position.x, terrain.SampleHeight(transform.position), transform.position.z);
        return transform;
    }
    public static Transform PlaceOnLevel(this Transform transform, LayerMask TerrainMask, float Range = 1, float NormalHeight = 0, bool SetHeight = true, bool SetAngles = true)
    {
        RaycastHit raycastHit;
        Vector3 First = transform.position + new Vector3(0, Range, 0);
        Vector3 Second = transform.position + new Vector3(0, -Range, 0);

        if (Physics.Linecast(First, Second, out raycastHit, TerrainMask))
        {
            //Угол по террайну
            if (SetAngles)
            {
                Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, raycastHit.normal)) * raycastHit.normal;
                transform.rotation = Quaternion.LookRotation(proj, raycastHit.normal);
            }
            //высота по террайну
            if (SetHeight)
                transform.position = new Vector3(transform.position.x, raycastHit.point.y, transform.position.z);
        }
        else transform.position = new Vector3(transform.position.x, NormalHeight, transform.position.z);
        return transform;
    }

    //Показывает время в минутах и секундах. Может ещё добавить FPS
    /*
	public static string TimeMinSec(float Timer, bool FPSShow = false)
	{
		string Result;
		int timersecs = (int)(Timer % 60);
		int timermins = (int)((Timer - Timer % 60) / 60);
		string timerminsstring;
		string timersecsstring;

		if (timermins < 10f)
			timerminsstring = "0" + timermins.ToString();
		else timerminsstring = timermins.ToString();
		if (timersecs < 10f)
			timersecsstring = "0" + timersecs.ToString();
		else timersecsstring = timersecs.ToString();
		Result = timerminsstring + ":" + timersecsstring;
		if (FPSShow)
			Result += " " + ((int)(1 / Time.deltaTime)).ToString();
		return Result;
	}*/
    public static string TimeMinSec(float Timer, bool msecs = false)
    {
        string Result;
        int timersecs = (int)(Timer % 60);
        int timermins = (int)((Timer - Timer % 60) / 60);
        float timermsecs = Timer % 1;
        string timerminsstring;
        string timersecsstring;

        if (timermins < 10f)
            timerminsstring = "0" + timermins.ToString();
        else timerminsstring = timermins.ToString();
        if (timersecs < 10f)
            timersecsstring = "0" + timersecs.ToString();
        else timersecsstring = timersecs.ToString();
        Result = timerminsstring + ":" + timersecsstring;
        if (msecs)
            Result += ":" + (timermsecs * 100).ToString("F0");
        return Result;
    }

    //Наименьшие и наибольшие значения в листах
    public static int Minimal(List<int> List)
    {
        int Result = int.MaxValue;
        for (int i = 0; i < List.Count; i++)
            if (List[i] < Result)
                Result = List[i];
        return List.IndexOf(Result);
    }
    public static int Minimal(List<float> List)
    {
        float Result = float.MaxValue;
        for (int i = 0; i < List.Count; i++)
            if (List[i] < Result)
                Result = List[i];
        return List.IndexOf(Result);
    }
    public static int Smallest(List<int> List)
    {
        return Minimal(List);
    }
    public static int Smallest(List<float> List)
    {
        return Minimal(List);
    }
    public static int Maximal(List<int> List)
    {
        int Result = int.MinValue;
        for (int i = 0; i < List.Count; i++)
            if (List[i] > Result)
                Result = List[i];
        return List.IndexOf(Result);
    }
    public static int Maximal(List<float> List)
    {
        float Result = float.MinValue;
        for (int i = 0; i < List.Count; i++)
            if (List[i] > Result)
                Result = List[i];
        return List.IndexOf(Result);
    }
    public static int Biggest(List<int> List)
    {
        return Maximal(List);
    }
    public static int Biggest(List<float> List)
    {
        return Maximal(List);
    }

    //Texture scaling
    public static Texture2D TextureScale(this Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Reinitialize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        return result;
    }
    //вспомогательная к Texture Scaling
    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        src.filterMode = fmode;
        src.Apply(true);
        RenderTexture rtt = new RenderTexture(width, height, 32);

        Graphics.SetRenderTarget(rtt);

        GL.LoadPixelMatrix(0, 1, 1, 0);

        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

    //Рэйкасты
    public static GameObject RayCast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return hit.transform.gameObject;
        else return null;
    }
    public static GameObject RayCast(Camera Cam)
    {
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return hit.transform.gameObject;
        else return null;
    }
    public static GameObject RayCast(Vector3 ScreenPoint)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(ScreenPoint), out RaycastHit hit))
            return hit.transform.gameObject;
        else return null;
    }
    public static GameObject RayCast(out Vector3 point)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            point = hit.point;
            return hit.transform.gameObject;
        }
        else
        {
            point = new Vector3(-1000, -1000, -1000);
            return null;
        }
    }
    public static GameObject RayCast(Vector3 ScreenPoint, out Vector3 point)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(ScreenPoint);

        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
            return hit.transform.gameObject;
        }
        else
        {
            point = new Vector3(-1000, -1000, -1000);
            return null;
        }
    }

    // Большие числа для игрока в виде k M T B aa ab...
    public static string ScoreShow(double Score)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        string result;
        string[] ScoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (Score < 990)
                break;
            else Score = System.Math.Floor(Score / 100f) / 10f;

        if (Score == System.Math.Floor(Score) || Score > 100f || i == 0)
            result = Score.ToString("F0") + ScoreNames[i];
        else result = Score.ToString("F1") + ScoreNames[i];
        return result;
    }
    public static string ScoreShow(float Score)
    {
        return ScoreShow((double)Score);
    }
    public static string ScoreShow(int Score)
    {
        return ScoreShow((double)Score);
    }

    //Recursively
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, newLayer);
    }
    public static List<Rigidbody> GetRigidBodyRecursively(GameObject obj)
    {
        List<Rigidbody> Result = new List<Rigidbody>();
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody Rigid))
            Result.Add(Rigid);

        foreach (Transform child in obj.transform)
            Result.AddRange(GetRigidBodyRecursively(child.gameObject));

        return Result;
    }

    public static Transform FindChildByName(this Transform Parent, string Name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(Parent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == Name)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
    public static List<Transform> FindChildrenByName(this Transform obj, string Name)
    {
        List<Transform> Result = new List<Transform>();

        foreach (Transform child in obj)
            Result.AddRange(FindChildrenByNameRecursion(child, Name));

        return Result;
    }
    static List<Transform> FindChildrenByNameRecursion(Transform obj, string Name)
    {
        List<Transform> Result = new List<Transform>();
        if (obj.gameObject.name == Name)
            Result.Add(obj);

        foreach (Transform child in obj)
            Result.AddRange(FindChildrenByNameRecursion(child, Name));

        return Result;
    }
    public static Transform FindChildByTag(this Transform Parent, string Tag)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(Parent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.tag == Tag)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
    public static List<Transform> FindChildrenByTag(this Transform obj, string Name)
    {
        List<Transform> Result = new List<Transform>();

        foreach (Transform child in obj)
            Result.AddRange(FindChildrenByTagRecursion(child, Name));

        return Result;
    }
    static List<Transform> FindChildrenByTagRecursion(Transform obj, string Name)
    {
        List<Transform> Result = new List<Transform>();
        if (obj.gameObject.tag == Name)
            Result.Add(obj);

        foreach (Transform child in obj)
            Result.AddRange(FindChildrenByTagRecursion(child, Name));

        return Result;
    }

    public static Transform LookAtSmooth(this Transform Object, Transform Target, float SmoothSpeed = 1)
    {
        Quaternion OriginalRot = Object.rotation;
        Object.LookAt(Target.position);
        Quaternion NewRot = Object.rotation;
        Object.rotation = OriginalRot;
        Object.rotation = Quaternion.Lerp(Object.rotation, NewRot, SmoothSpeed * Time.deltaTime);
        return Object;
    }
    public static Transform LookAtSmooth(this Transform Object, Vector3 Point, float SmoothSpeed = 1)
    {
        Quaternion OriginalRot = Object.rotation;
        Object.LookAt(Point);
        Quaternion NewRot = Object.rotation;
        Object.rotation = OriginalRot;
        Object.rotation = Quaternion.Lerp(Object.rotation, NewRot, SmoothSpeed * Time.deltaTime);
        return Object;
    }

    public static Vector3 RandomVector3()
    {
        Vector3 Result = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        return Result;
    }
    public static Vector3 RandomVector3(float Power)
    {
        Vector3 Result = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Power;
        return Result;
    }
    public static Vector3 RandomVector3(float XMin, float XMax, float YMin, float YMax, float ZMin, float ZMax)
    {
        Vector3 Result = new Vector3(Random.Range(XMin, XMax), Random.Range(YMin, YMax), Random.Range(ZMin, ZMax));
        return Result;
    }
    public static Vector2 RandomVector2()
    {
        Vector2 Result = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        return Result;
    }
    public static Vector2 RandomVector2(float Power)
    {
        Vector2 Result = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Power;
        return Result;
    }
    public static Vector2 RandomVector2(float XMin, float XMax, float YMin, float YMax)
    {
        Vector2 Result = new Vector2(Random.Range(XMin, XMax), Random.Range(YMin, YMax));
        return Result;
    }
    /// <summary>
    /// Percentage 0.00-1.00
    /// </summary>
    /// <param name="Percentage"></param>
    /// <returns></returns>
    public static bool Rand(float Percentage)
    {
        bool result = Random.Range(0f, 1f) < Percentage ? true : false;
        return result;
    }
    /// <summary>
    /// Random between two values (int or float)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Ran(float min = -1, float max = 1)
    {
        return Random.Range(min, max);
    }
    /// <summary>
    /// Random between two values (int or float)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Ran(int min, int max)
    {
        return Random.Range(min, max);
    }
    public static int Ran(int max)
    {
        return Random.Range(0, max);
    }
    public static float Ran(float max)
    {
        return Random.Range(0f, max);
    }
    public static void MaterialToOpaqueMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
    public static void MaterialToFadeMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public static List<MeshRenderer> GetMeshRendererRecursively(Transform obj)
    {
        List<MeshRenderer> Result = new List<MeshRenderer>();
        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer Rigid))
            Result.Add(Rigid);

        foreach (Transform child in obj)
            Result.AddRange(GetMeshRendererRecursively(child));

        return Result;
    }
    public static void SkyBox(Material material)
    {
        RenderSettings.skybox = material;
    }
    public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (allign)
        {
            case (AnchorPresets.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPresets.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPresets.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPresets.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPresets.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPresets.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }
    public static void SetPivot(this RectTransform source, PivotPresets preset)
    {

        switch (preset)
        {
            case (PivotPresets.TopLeft):
                {
                    source.pivot = new Vector2(0, 1);
                    break;
                }
            case (PivotPresets.TopCenter):
                {
                    source.pivot = new Vector2(0.5f, 1);
                    break;
                }
            case (PivotPresets.TopRight):
                {
                    source.pivot = new Vector2(1, 1);
                    break;
                }

            case (PivotPresets.MiddleLeft):
                {
                    source.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleCenter):
                {
                    source.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleRight):
                {
                    source.pivot = new Vector2(1, 0.5f);
                    break;
                }

            case (PivotPresets.BottomLeft):
                {
                    source.pivot = new Vector2(0, 0);
                    break;
                }
            case (PivotPresets.BottomCenter):
                {
                    source.pivot = new Vector2(0.5f, 0);
                    break;
                }
            case (PivotPresets.BottomRight):
                {
                    source.pivot = new Vector2(1, 0);
                    break;
                }
        }
    }
    public static bool IsLayer(Transform other, LayerMask mask)
    {
        return mask.value == (mask | (1 << other.gameObject.layer));
    }
    public static bool IsLayer(GameObject other, LayerMask mask)
    {
        return mask.value == (mask | (1 << other.layer));
    }
    public static int Summ(List<int> IntList)
    {
        int Result = 0;
        foreach (int a in IntList)
            Result += a;
        return Result;
    }
    public static int Summ(int[] IntList)
    {
        int Result = 0;
        foreach (int a in IntList)
            Result += a;
        return Result;
    }
    public static float Summ(float[] IntList)
    {
        float Result = 0;
        foreach (float a in IntList)
            Result += a;
        return Result;
    }
    public static float Summ(List<float> IntList)
    {
        float Result = 0;
        foreach (float a in IntList)
            Result += a;
        return Result;
    }
    public static Transform FindGameobject(string name)
    {
        return GameObject.Find(name).transform;
    }
    public static float DistanceXZ(Vector3 pos, Vector3 point)
    {
        pos.y = 0;
        point.y = 0;
        return Vector3.Distance(pos, point);
    }
    public static float DistanceXZ(Transform trans, Vector3 point)
    {
        return DistanceXZ(trans.position, point);
    }
    public static float DistanceXZ(Transform transA, Transform transB)
    {
        return DistanceXZ(transA.position, transB.position);
    }
    public static void ParaMove(this Transform obj, Vector3 target, float time, System.Action onComplete, float width = 0, float height = 1, float depth = 0)
    {
        DOTween.Kill(obj);
        float Timer = 0;
        Vector3 StartPos = obj.position;
        DOTween.To(() => Timer, x => Timer = x, 1, time)
            .OnUpdate(() => obj.position = Parabola(StartPos, target, width, height, depth, Timer))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                obj.position = Parabola(StartPos, target, width, height, depth, Timer);
            });
    }
    public static void ParaMove(this Transform obj, Transform target, float time, System.Action onComplete, float width = 0, float height = 1, float depth = 0)
    {
        DOTween.Kill(obj);
        float Timer = 0;
        Vector3 StartPos = obj.position;
        DOTween.To(() => Timer, x => Timer = x, 1, time)
            .OnUpdate(() => obj.position = Parabola(StartPos, target.position, width, height, depth, Timer))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                obj.position = Parabola(StartPos, target.position, width, height, depth, Timer);
            });
    }
    public static void ParaMoveLocal(this Transform obj, Vector3 point, float time, System.Action onComplete, float width = 0, float height = 1, float depth = 0)
    {
        DOTween.Kill(obj);
        float Timer = 0;
        Vector3 StartPos = obj.localPosition;
        DOTween.To(() => Timer, x => Timer = x, 1, time)
            .OnUpdate(() => obj.localPosition = Parabola(StartPos, point, width, height, depth, Timer))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                obj.localPosition = Parabola(StartPos, point, width, height, depth, Timer);
            });
    }
    public static Vector3 Parabola(Vector3 start, Vector3 end, float width, float height, float depth, float t)
    {
        System.Func<float, float> w = x => -4 * width * x * x + 4 * width * x;
        System.Func<float, float> h = x => -4 * height * x * x + 4 * height * x;
        System.Func<float, float> d = x => -4 * depth * x * x + 4 * depth * x;

        return Vector3.Lerp(start, end, t) + new Vector3(w(t), h(t), d(t));
    }
    public static void TextScore(this UnityEngine.UI.Text Target, int NewValue, float time = 1)
    {
        DOTween.Kill(Target);
        int Start = int.Parse(Target.text);
        int Value = Start;
        DOTween.To(() => Value, x => Value = x, NewValue, time)
            .OnUpdate(() => { Target.text = Value.ToString("F0"); })
            .OnComplete(() => { Target.text = Value.ToString("F0"); });
    }
    public static bool PointerOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public static void Fade(this Transform target, float time = 1)
    {
        foreach (Image a in target.GetComponentsInChildren<Image>())
            a.DOFade(0, time);
        foreach (Text b in target.GetComponentsInChildren<Text>())
            b.DOFade(0, time);
        foreach (TMPro.TextMeshPro b in target.GetComponentsInChildren<TMPro.TextMeshPro>())
            b.DOFade(0, time);
        foreach (TMPro.TextMeshProUGUI b in target.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            b.DOFade(0, time);
    }
    public static Transform SetVisibleByScale(this Transform Target, bool status)
    {
        if (status) Target.localScale = Vector3.one;
        else Target.localScale = Vector3.one;

        return Target;
    }
    public static float GetCurrentAnimatorTime(this Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        return currentTime;
    }
    public static Texture2D ConvertToTexture2D(this Texture texture)
    {
        return Texture2D.CreateExternalTexture(
            texture.width,
            texture.height,
            TextureFormat.RGB24,
            false, false,
            texture.GetNativeTexturePtr());
    }

    public static Vector3 RowColumn(int index, int columns, int rows, Vector3 offset)
    {
        return new Vector3((index % rows) * offset.x,
                      Mathf.FloorToInt((float)index / ((float)columns * rows)) * offset.y,
                      (index / rows) % columns * offset.z);
    }
}
public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}
public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}