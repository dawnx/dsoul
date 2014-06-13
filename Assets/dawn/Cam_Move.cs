using System;
using UnityEngine;
public class Cam_Move : MonoBehaviour
{
	private int dx = 1;
	private bool zoom;
	private bool iconmove;
	private int z_speed;
	private float z_time;
	private float zoomdelay;
	private float fov = 30f;
	private float originfov = 30f;
	public Transform cha1;
	private Transform target;
	private Transform mytransform;
	private Vector3 distancetarget;
	private Vector3 chaposition;
	private float boundfactor = 1f;
	private Vector3 hit_shake1 = new Vector3(0.06f, 0f, 0.03f);
	private Vector3 hit_shake2 = new Vector3(0f, 0.03f, 0.02f);
	private float resetcam_delay;
	private bool resetstart;
	private bool fovchange;
	private Camera mycamera;
	public Vector3 limitpos;
	//private Icon_Skill script_skillicon;
	private float fovbk_delay;
	private float limit_x = 30f;
	private float limit_y_b = 30f;
	private float limit_y_f = 35f;
	private bool hidestop;
	private short topviewon;
	private float topviewdelay = 3f;
	private short movespeed = 10;
	//private UI_Ingame script_ui;
	private bool story_scene;
	private void Awake()
	{
		this.mytransform = base.transform;
		this.mycamera = base.camera;
		//this.cha1 = GameObject.FindWithTag("Player").transform;
		//this.script_skillicon = GameObject.FindWithTag("icon_skill").GetComponent<Icon_Skill>();
		this.ResetCam();
		//this.script_ui = GameObject.FindWithTag("ui").GetComponent<UI_Ingame>();
		this.LookTarget (this.cha1.transform, 0, 0);
	}
	private void Start()
	{
		//int num = Crypto.Load_int_key("play_kind");
		//if (num > 5)
		//{
		//	this.limit_x = 0.5f;
		//	this.limit_y_b = -1f;
		//	this.limit_y_f = 15.5f;
		//}
		this.distancetarget = new Vector3(0f, 1.3f, -1.04f);
	}
	public void Topview()
	{
		this.distancetarget = new Vector3(0f, 1.3f, -0.1f);
		this.topviewon = 1;
		this.topviewdelay = 3f;
		this.movespeed = 5;
	}
	public void LookTarget(Transform _target, int _fov, float _delay)
	{
		this.target = _target;
		if (_delay > 0f)
		{
			this.resetstart = true;
			this.resetcam_delay = _delay;
		}
		if (_fov != 0)
		{
			this.ZoomIn(-1, _fov, _delay);
		}
	}
	public void ThisIsStory()
	{
		this.story_scene = true;
	}
	public void IconHideStop(bool _hidestop)
	{
		this.hidestop = _hidestop;
	}
	public void ResetCam()
	{
		this.target = this.cha1;
		this.fov = this.originfov;
		this.fovchange = true;
		this.resetstart = false;
		this.zoom = false;
		this.zoomdelay = 0f;
	}
	public void FovTest(float _tempfov)
	{
		this.originfov = _tempfov;
		this.fov = this.originfov;
		this.mycamera.fieldOfView = this.originfov;
	}
	private void Update()
	{
		if (this.resetstart)
		{
			if (this.resetcam_delay > 0f)
			{
				this.resetcam_delay -= Time.deltaTime;
			}
			else
			{
				this.resetcam_delay = 0f;
				this.resetstart = false;
				this.ResetCam();
				//this.cha1.GetComponent<Cha_Control>().StartControl();
			}
		}
		this.chaposition = this.target.position + this.distancetarget;
		this.mytransform.position = Vector3.Lerp(this.mytransform.position, this.chaposition, Time.deltaTime * (float)this.movespeed);
		this.limitpos = this.mytransform.position;
		if (this.topviewon != 1)
		{
			this.limitpos[2] = Mathf.Clamp(this.limitpos[2], this.limit_y_b * this.boundfactor, this.limit_y_f * this.boundfactor);
		}
		this.limitpos[0] = Mathf.Clamp(this.limitpos[0], -this.limit_x * this.boundfactor, this.limit_x * this.boundfactor);
		if (this.topviewon == 2)
		{
			this.mytransform.position = Vector3.MoveTowards(this.mytransform.position, this.limitpos, Time.deltaTime * 3f);
		}
		else
		{
			this.mytransform.position = this.limitpos;
		}
		if (!this.hidestop)
		{
			if (this.chaposition[0] > 2.4f)
			{
				if (!this.iconmove)
				{
					this.iconmove = true;
					//this.script_skillicon.SkillIcon_Move(true);
				}
			}
			else
			{
				if (this.chaposition[0] < -2.4f)
				{
					if (!this.iconmove)
					{
						this.iconmove = true;
						//this.script_skillicon.PetIcon_Move(true);
					}
				}
				else
				{
					if (this.iconmove)
					{
						this.iconmove = false;
						//this.script_skillicon.SkillIcon_Move(false);
						//this.script_skillicon.PetIcon_Move(false);
					}
				}
			}
		}
		if (this.fovchange)
		{
			this.boundfactor = 0.8f + 0.2f * this.originfov / this.mycamera.fieldOfView;
			if (this.zoom)
			{
				if (this.z_speed == -1)
				{
					this.mycamera.fieldOfView = this.fov;
				}
				else
				{
					this.mycamera.fieldOfView = Mathf.Lerp(this.mycamera.fieldOfView, this.fov, Time.deltaTime * (float)this.z_speed);
				}
				if (this.zoomdelay < this.z_time)
				{
					this.zoomdelay += Time.deltaTime;
				}
				else
				{
					this.zoom = false;
					this.zoomdelay = 0f;
					this.fovbk_delay = 0f;
				}
			}
			else
			{
				if (this.fovbk_delay < 2f)
				{
					this.fovbk_delay += Time.deltaTime;
					this.mycamera.fieldOfView = Mathf.Lerp(this.mycamera.fieldOfView, this.originfov, Time.deltaTime * 3f);
				}
				else
				{
					this.fovbk_delay = 0f;
					this.fovchange = false;
					this.mycamera.fieldOfView = this.originfov;
				}
			}
		}
		if (this.topviewon > 0)
		{
			this.topviewdelay -= Time.deltaTime;
			if (this.topviewon == 1)
			{
				this.mytransform.rotation = Quaternion.Lerp(this.mytransform.rotation, Quaternion.Euler(80f, 0f, 0f), Time.deltaTime * 5f);
				this.mycamera.fieldOfView = Mathf.Lerp(this.mycamera.fieldOfView, 40f, Time.deltaTime * 3f);
				if (this.topviewdelay < 1.5f)
				{
					this.distancetarget = new Vector3(0f, 1.3f, -1.04f);
					this.topviewon = 2;
					this.movespeed = 5;
				}
			}
			else
			{
				if (this.topviewon == 2)
				{
					this.mytransform.rotation = Quaternion.Lerp(this.mytransform.rotation, Quaternion.Euler(50f, 0f, 0f), Time.deltaTime * 5f);
					this.mycamera.fieldOfView = Mathf.Lerp(this.mycamera.fieldOfView, this.originfov, Time.deltaTime * 3f);
					if (this.topviewdelay < 0f)
					{
						this.topviewon = 0;
						this.mytransform.rotation = Quaternion.Euler(50f, 0f, 0f);
						this.movespeed = 10;
					}
				}
			}
		}
	}
	public void ZoomIn(int _zoomspeed, int _fov, float _delay)
	{
		this.zoom = true;
		this.z_speed = _zoomspeed;
		this.fov = (float)_fov;
		this.z_time = _delay;
		this.fovchange = true;
	}
	public void Hitcam()
	{
//		if (!this.story_scene)
//		{
//			this.script_ui.ComboPlus(0.01f);
//		}
		this.dx = -this.dx;
		this.mytransform.position += this.hit_shake1 * (float)this.dx;
	}
	public void Hitcam2(float _factor)
	{
		this.mytransform.position += this.hit_shake2 * _factor;
	}
}
