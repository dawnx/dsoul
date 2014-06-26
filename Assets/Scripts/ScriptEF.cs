 using UnityEngine;
using System.Collections;

public class ScriptEF
{
    public static BloodBar CreateBloodBar(GameObject parent)
    {
        Transform pt = parent.transform;
        GameObject heroPanel = GameObject.Instantiate(ObjectManager.bloodBar, pt.position, pt.rotation) as GameObject;
        heroPanel.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        BloodBar bloodBar = heroPanel.GetComponent<BloodBar>();
        bloodBar.setParent(parent);
        bloodBar.Damaged(100, 100);
        bloodBar.distance = 2f;
        return bloodBar;
    }
     
    public static void CreateDamageNum(Vector3 pos, float distance, int damage, Vector3 attackdir, bool isCrit=false, bool isDodge=false)
    {

        pos.y += distance;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        screenPos.z = 0f;

        // 使用UI摄像机转换到NGUI的世界坐标
        Vector3 nguiPos = UICamera.currentCamera.ScreenToWorldPoint(screenPos);
        UILabel label = NGUITools.AddChild<UILabel>(ObjectManager.damageNumPanel);
        label.bitmapFont = ObjectManager.font;
        label.gameObject.transform.position = nguiPos;

        string txt = "";
        if (isDodge)
            txt = "闪避";
        else
            txt = damage.ToString();

        label.text = txt;
        label.fontSize = 16;

        DamageNum dn = label.gameObject.AddComponent<DamageNum>();
        dn.setDir(attackdir);
    }

}
