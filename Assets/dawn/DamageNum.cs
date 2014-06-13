using System;
using UnityEngine;
public class DamageNum : MonoBehaviour
{
	private Transform mytransform;
	private float finishdelay = 1f;
	private Vector3 dir;
	private void Awake()
	{
		this.mytransform = base.transform;
        base.gameObject.SetActive(false);
	}
	public void setDir(Vector3 _dir)
	{
		this.dir = _dir;
        base.gameObject.SetActive(true);
	}
	private void Update()
	{
		this.finishdelay -= Time.deltaTime * 2f;
        if (this.finishdelay < 0f)
        {
            this.finishdelay = 1f;
            base.gameObject.SetActive(false);
            DestroyImmediate(gameObject);
        }
        else
        {
            this.mytransform.position += (Vector3.up + this.dir) * this.finishdelay * Time.deltaTime * 0.3f;
        }
	}
}
