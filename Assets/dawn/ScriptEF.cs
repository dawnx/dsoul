 using UnityEngine;
using System.Collections;

public class ScriptEF
{
    public static GameObject damageNumPanel;
    public static UIFont font;
    public static Transform CreatHpbar(Vector2 _size, bool _ally, Material hp_bar)
    {
        GameObject gameObject = new GameObject("hp_bar");
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>();
        mesh.vertices = new Vector3[]
		{
			new Vector3(-_size.x, _size.y, 0f) * 0.5f,
			new Vector3(_size.x, _size.y, 0f) * 0.5f,
			new Vector3(-_size.x, -_size.y, -0.02f) * 0.5f,
			new Vector3(_size.x, -_size.y, -0.02f) * 0.5f
		};
        float num = 0f;
        if (_ally)
        {
            num = 0.75f;
        }
        mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0.25f + num),
			new Vector2(0.5f, 0.25f + num),
			new Vector2(0f, num),
			new Vector2(0.5f, num)
		};
        Renderer renderer = gameObject.renderer;
        renderer.receiveShadows = false;
        renderer.castShadows = false;
        renderer.sharedMaterial = hp_bar;
        mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        //gameObject.transform.parent = this.mytransform;
//        if (!_turretmode)
//        {
            gameObject.AddComponent("Hp_bar");
//        }
//        else
//       {
//            gameObject.AddComponent("Hp_bar_fixed");
//        }
        return gameObject.transform;
    }

    public static void CreateDamageNum(Vector3 pos, float distance, int damage, Vector3 attackdir)
    {

        pos.y += distance;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        screenPos.z = 0f;

        // 使用UI摄像机转换到NGUI的世界坐标
        Vector3 nguiPos = UICamera.currentCamera.ScreenToWorldPoint(screenPos);
        UILabel label = NGUITools.AddChild<UILabel>(damageNumPanel);
        label.bitmapFont = font;
        label.gameObject.transform.position = nguiPos;
        label.text = damage.ToString();
        label.fontSize = 16;

        DamageNum dn = label.gameObject.AddComponent<DamageNum>();
        dn.setDir(attackdir);
    }

}
