using System;
using System.Collections.Generic;
using DG.Tweening;
using S13Audio;
using TMG.Controls;
using TMG.Core;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class PlayerController : TMGMonoBehaviour
{
	// Token: 0x0600130C RID: 4876 RVA: 0x000759AC File Offset: 0x00073BAC
	public PlayerController()
	{
	}

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x0600130D RID: 4877 RVA: 0x00075A50 File Offset: 0x00073C50
	// (remove) Token: 0x0600130E RID: 4878 RVA: 0x00075A88 File Offset: 0x00073C88
	public event EventHandler OnSeeingToolActive;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x0600130F RID: 4879 RVA: 0x00075AC0 File Offset: 0x00073CC0
	// (remove) Token: 0x06001310 RID: 4880 RVA: 0x00075AF8 File Offset: 0x00073CF8
	public event EventHandler OnDeath;

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06001311 RID: 4881 RVA: 0x0000FB8D File Offset: 0x0000DD8D
	public CharacterController CharacterController
	{
		get
		{
			return this.m_CharacterController;
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06001312 RID: 4882 RVA: 0x0000FB95 File Offset: 0x0000DD95
	public Transform HeadContainer
	{
		get
		{
			return this.m_HeadContainer;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06001313 RID: 4883 RVA: 0x0000FB9D File Offset: 0x0000DD9D
	public Transform CameraParent
	{
		get
		{
			return this.m_CameraContainer;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06001314 RID: 4884 RVA: 0x0000FBA5 File Offset: 0x0000DDA5
	public Transform WeaponParent
	{
		get
		{
			return this.m_WeaponParent;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06001315 RID: 4885 RVA: 0x0000FBAD File Offset: 0x0000DDAD
	// (set) Token: 0x06001316 RID: 4886 RVA: 0x0000FBB5 File Offset: 0x0000DDB5
	public bool isLocked { get; private set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06001317 RID: 4887 RVA: 0x0000FBBE File Offset: 0x0000DDBE
	public bool useGravity
	{
		get
		{
			return this.m_EnableGravity;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06001318 RID: 4888 RVA: 0x0000FBC6 File Offset: 0x0000DDC6
	// (set) Token: 0x06001319 RID: 4889 RVA: 0x0000FBCE File Offset: 0x0000DDCE
	public bool isSlowed { get; private set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600131A RID: 4890 RVA: 0x0000FBD7 File Offset: 0x0000DDD7
	// (set) Token: 0x0600131B RID: 4891 RVA: 0x0000FBDF File Offset: 0x0000DDDF
	public bool isMoveLocked { get; private set; }

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600131C RID: 4892 RVA: 0x0000FBE8 File Offset: 0x0000DDE8
	// (set) Token: 0x0600131D RID: 4893 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
	public bool canJump { get; private set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x0000FBF9 File Offset: 0x0000DDF9
	// (set) Token: 0x0600131F RID: 4895 RVA: 0x0000FC01 File Offset: 0x0000DE01
	public bool canCameraSway { get; private set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06001320 RID: 4896 RVA: 0x0000FC0A File Offset: 0x0000DE0A
	// (set) Token: 0x06001321 RID: 4897 RVA: 0x0000FC12 File Offset: 0x0000DE12
	public bool canRun { get; private set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06001322 RID: 4898 RVA: 0x0000FC1B File Offset: 0x0000DE1B
	// (set) Token: 0x06001323 RID: 4899 RVA: 0x0000FC23 File Offset: 0x0000DE23
	public bool isSeeingToolActive { get; private set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06001324 RID: 4900 RVA: 0x0000FC2C File Offset: 0x0000DE2C
	// (set) Token: 0x06001325 RID: 4901 RVA: 0x0000FC34 File Offset: 0x0000DE34
	public CombatStatus CurrentStatus { get; private set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06001326 RID: 4902 RVA: 0x0000FC3D File Offset: 0x0000DE3D
	// (set) Token: 0x06001327 RID: 4903 RVA: 0x0000FC45 File Offset: 0x0000DE45
	public float CurrentSpeed { get; private set; }

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x0000FC4E File Offset: 0x0000DE4E
	public FootstepTypes CurrentFootstepType
	{
		get
		{
			return this.m_PlayerFootsteps.CurrentFootstepType;
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00075B30 File Offset: 0x00073D30
	public override void Init()
	{
		base.Init();
		GameManager.Instance.Player = this;
		this.m_CharacterController = base.GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.m_OriginWalkSpeed = this.m_MoveSpeed;
		this.m_OriginRunSpeed = this.m_RunSpeed;
		this.CurrentStatus = CombatStatus.Idle;
		this.canJump = this.m_EnableJump;
		this.canRun = this.m_EnableRun;
		this.m_CameraMovement.Init(this.m_HeadContainer, this.m_CameraContainer);
		this.m_CameraFOV.Init(new Camera[]
		{
			GameManager.Instance.GameCamera.Camera,
			GameManager.Instance.GameCamera.WeaponCamera
		});
		this.m_PlayerLook.Init(base.transform, this.m_HeadContainer);
		this.m_Interaction.Init();
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x00075C0C File Offset: 0x00073E0C
	public override void InitOnComplete()
	{
		base.InitOnComplete();
		List<FootstepsDataVO> list = new List<FootstepsDataVO>();
		list.Add(FootstepsDataVO.Create(FootstepTypes.WOOD, "step_wood"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.WOOD_STAIRS, "step_wood"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.INK, "step_wood"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.INK_STAIRS, "step_wood"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.INK_DEEP, "step_wood"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.VENT, "vent"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.DIRT, "step_dirt"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.METAL, "step_metal"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.TILE, "step_tile"));
		list.Add(FootstepsDataVO.Create(FootstepTypes.INK_DEEP, "step_water"));
		this.m_PlayerFootsteps.Init(list);
		this.m_HeadBob.Init(this.m_HeadContainer, this.m_PlayerFootsteps.StepInterval);
		GameManager.Instance.AudioManager.ListenerSetActive(false);
		DOTween.Sequence().InsertCallback(0.5f, delegate
		{
			this.m_LandingAudioBS = true;
		});
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00075D24 File Offset: 0x00073F24
	private void Update()
	{
		if (!this.setupCheats)
		{
			this.setupCheats = true;
			base.gameObject.AddComponent<BENDYCHEATS>().playerController = this;
		}
		if (GameManager.Instance.isPaused)
		{
			this.m_IsPaused = true;
			return;
		}
		if (this.m_IsPaused)
		{
			DOTween.Sequence().InsertCallback(0.1f, delegate
			{
				this.m_IsPaused = false;
			});
			return;
		}
		if (GameManager.Instance.isPaused)
		{
			return;
		}
		this.m_PlayerLook.GetInput();
		if (GameManager.Instance.PlayerSettings.ToggleRun && PlayerInput.RunDown())
		{
			this.m_ToggleRun = !this.m_ToggleRun;
		}
		if (this.m_CharacterController.isGrounded && !this.m_JumpInput && this.m_EnableJump && this.canJump && !this.isLocked && !this.isMoveLocked)
		{
			this.m_JumpInput = PlayerInput.Jump();
		}
		this.m_Interaction.UpdateInteraction(this.m_CameraContainer.position, this.m_CameraContainer.forward);
		if (this.m_CanHaveSeeingTool && this.m_CharacterController.isGrounded && this.CurrentStatus != CombatStatus.Hiding && !this.isLocked && this.m_IsSeeingToolEnabled && PlayerInput.SeeingTool())
		{
			this.isSeeingToolActive = !this.isSeeingToolActive;
			this.UseSeeingTool(this.isSeeingToolActive);
		}
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00075E7C File Offset: 0x0007407C
	private void FixedUpdate()
	{
		if (GameManager.Instance.isPaused)
		{
			this.m_IsPaused = true;
			return;
		}
		if (this.m_IsPaused)
		{
			DOTween.Sequence().InsertCallback(0.1f, delegate
			{
				this.m_IsPaused = false;
			});
			return;
		}
		float currentSpeed = 0f;
		float num = 10f;
		this.GetInput(out currentSpeed);
		this.CurrentSpeed = currentSpeed;
		this.CheckGrounding();
		if (this.m_CharacterController.isGrounded)
		{
			num = 60f;
		}
		if (!this.isMoveLocked)
		{
			this.Move(this.CurrentSpeed);
			if (this.m_ExternalForce.magnitude > 0f)
			{
				if (this.m_CharacterController.enabled)
				{
					this.m_CharacterController.Move(this.m_ExternalForce * Time.fixedDeltaTime);
				}
				this.m_ExternalForce = Vector3.MoveTowards(this.m_ExternalForce, Vector3.zero, 3f * Time.fixedDeltaTime * num);
			}
		}
		if (this.isSeeingToolActive)
		{
			this.m_HeadBob.UpdateCameraPosition(0f, 0f);
		}
		this.GetRotations();
		this.m_PlayerLook.UpdateCursorLock();
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00075F98 File Offset: 0x00074198
	private void GetRotations()
	{
		if (this.canCameraSway)
		{
			this.m_CameraMovement.Sway(this.m_HeadContainer);
		}
		if (!this.isLocked)
		{
			this.m_PlayerLook.Rotation(base.transform, new Transform[]
			{
				this.m_HeadContainer,
				this.m_HandContainer
			});
		}
		if (this.m_CharacterController.isGrounded && !this.m_PreviouslyGrounded && this.m_LandingAudioBS)
		{
			base.StartCoroutine(this.m_HeadBob.DoJumpBob());
			this.m_PlayerFootsteps.PlayLandAudio();
			this.m_ExternalForce = Vector3.zero;
		}
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x00076034 File Offset: 0x00074234
	private void GetInput(out float speed)
	{
		if (this.isLocked)
		{
			speed = 0f;
			return;
		}
		float x = PlayerInput.MoveX() * 0.8f;
		float num = PlayerInput.MoveY();
		if (num > 0f)
		{
			if (GameManager.Instance.PlayerSettings.ToggleRun)
			{
				this.m_IsRunning = (this.m_EnableRun && this.m_ToggleRun);
			}
			else
			{
				this.m_IsRunning = (this.m_EnableRun && PlayerInput.Run());
			}
		}
		else
		{
			if (!this.m_CanMoveBack)
			{
				num = 0f;
			}
			this.m_IsRunning = false;
		}
		speed = ((!this.m_IsRunning) ? this.m_MoveSpeed : this.m_RunSpeed);
		if (this.isSlowed)
		{
			speed = this.m_SlowedMoveSpeed;
			this.m_IsRunning = false;
		}
		else if (!this.canRun)
		{
			speed = this.m_MoveSpeed;
			this.m_IsRunning = false;
		}
		this.m_Input = new Vector2(x, num);
		if (this.m_Input.magnitude > 1f)
		{
			this.m_Input.Normalize();
		}
		if (num < 0f)
		{
			speed *= 0.75f;
		}
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00076148 File Offset: 0x00074348
	private void CheckGrounding()
	{
		if (this.m_CharacterController.isGrounded && !this.m_PreviouslyGrounded && this.m_GravityPower <= 0f)
		{
			this.m_GravityPower = -this.m_StickToGroundForce;
		}
		this.m_PreviouslyGrounded = this.m_CharacterController.isGrounded;
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x00076198 File Offset: 0x00074398
	private void Move(float speed)
	{
		Vector3 vector = base.transform.forward * this.m_Input.y + base.transform.right * this.m_Input.x;
		float num = Vector3.Angle(Vector3.up, this.m_GroundNormal);
		bool flag = num < this.m_CharacterController.slopeLimit || num > 85f;
		this.m_MoveDir.x = vector.x * speed;
		this.m_MoveDir.z = vector.z * speed;
		if (!flag)
		{
			this.m_MoveDir.x = this.m_MoveDir.x + (1f - this.m_GroundNormal.y) * this.m_GroundNormal.x * (speed / 2f);
			this.m_MoveDir.z = this.m_MoveDir.z + (1f - this.m_GroundNormal.y) * this.m_GroundNormal.z * (speed / 2f);
		}
		this.m_GroundNormal = Vector3.up;
		if (this.m_CharacterController.isGrounded)
		{
			if (this.m_JumpInput && flag)
			{
				this.m_GravityPower = this.m_JumpSpeed;
				this.m_PlayerFootsteps.PlayJumpAudio();
			}
			this.m_JumpInput = false;
		}
		else if (this.m_EnableGravity)
		{
			this.m_GravityPower += Physics.gravity.y * this.m_GravityMultiplier * Time.fixedDeltaTime;
		}
		if (this.m_CharacterController.enabled)
		{
			this.m_CharacterController.Move(this.m_MoveDir * Time.fixedDeltaTime + Vector3.up * this.m_GravityPower);
		}
		if (this.m_CharacterController.isGrounded)
		{
			float magnitude = this.m_CharacterController.velocity.magnitude;
			FootstepTypes footstepType = FootstepTypes.WOOD;
			Vector3 position = base.transform.position;
			position.y += this.m_CharacterController.bounds.extents.y - 0.1f;
			RaycastHit raycastHit;
			if (Physics.Raycast(position, Vector3.down, out raycastHit, this.m_CharacterController.height + 1f, ~(1 << LayerMask.NameToLayer("Player"))))
			{
				if (raycastHit.collider.CompareTag("Ink"))
				{
					footstepType = FootstepTypes.INK;
				}
				else if (raycastHit.collider.CompareTag("DeepInk"))
				{
					footstepType = FootstepTypes.INK_DEEP;
				}
				else if (raycastHit.collider.CompareTag("Stairs"))
				{
					footstepType = FootstepTypes.WOOD_STAIRS;
				}
				else if (raycastHit.collider.CompareTag("StairsInk"))
				{
					footstepType = FootstepTypes.INK_STAIRS;
				}
				else if (raycastHit.collider.CompareTag("Vent"))
				{
					footstepType = FootstepTypes.VENT;
				}
				else if (raycastHit.collider.CompareTag("Dirt"))
				{
					footstepType = FootstepTypes.DIRT;
				}
				else if (raycastHit.collider.CompareTag("Metal"))
				{
					footstepType = FootstepTypes.METAL;
				}
				else if (raycastHit.collider.CompareTag("Tile"))
				{
					footstepType = FootstepTypes.TILE;
				}
			}
			this.m_PlayerFootsteps.SetFootstepType(footstepType);
			this.m_PlayerFootsteps.ProgressStepCycle(magnitude, speed);
			this.m_HeadBob.UpdateCameraPosition(magnitude, speed);
			this.m_CameraFOV.UpdateVOD(magnitude, speed, this.m_IsRunning);
		}
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x000764EC File Offset: 0x000746EC
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		this.m_GroundNormal = hit.normal;
		if (hit.gameObject.isStatic)
		{
			return;
		}
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (!attachedRigidbody || attachedRigidbody.isKinematic)
		{
			return;
		}
		Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
		attachedRigidbody.velocity = a * 2f;
		this.m_ExternalForce = Vector3.zero;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0000FC5B File Offset: 0x0000DE5B
	public void AddForce(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x00076570 File Offset: 0x00074770
	public void EquipWeapon()
	{
		this.m_Interaction.HasWeapon = true;
		if (this.WeaponGameObject)
		{
			BaseWeapon component = this.WeaponGameObject.GetComponent<BaseWeapon>();
			if (component)
			{
				component.CleanEquip();
			}
		}
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000765B0 File Offset: 0x000747B0
	public void UnEquipWeapon()
	{
		this.m_Interaction.HasWeapon = false;
		if (this.WeaponGameObject)
		{
			BaseWeapon component = this.WeaponGameObject.GetComponent<BaseWeapon>();
			if (component)
			{
				component.UnEquip();
			}
		}
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x0000FC6F File Offset: 0x0000DE6F
	public void EnableSeeingTool(bool active)
	{
		this.m_IsSeeingToolEnabled = active;
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0000FC78 File Offset: 0x0000DE78
	public void AllowSeeingTool(bool active)
	{
		this.m_CanHaveSeeingTool = active;
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x000765F0 File Offset: 0x000747F0
	public void UseSeeingTool(bool active)
	{
		if (active && this.isMoveLocked)
		{
			this.isSeeingToolActive = !this.isSeeingToolActive;
			return;
		}
		this.m_SeeingToolSequence.Kill(false);
		this.m_SeeingToolSequence = DOTween.Sequence();
		float num = 0f;
		if (active)
		{
			this.OnSeeingToolActive.Send(this);
			this.SetInteraction(false);
			GameManager.Instance.HideCrosshair();
			this.m_SeeingTool.localPosition = new Vector3(0f, -5f, 0f);
			if (this.WeaponGameObject)
			{
				this.m_SeeingToolSequence.Insert(num, this.WeaponGameObject.transform.DOLocalMoveY(-5f, 0.4f, false).SetEase(Ease.InQuad));
				num += 0.35f;
				this.m_SeeingToolSequence.InsertCallback(num, delegate
				{
					this.WeaponGameObject.SetActive(false);
					this.m_SeeingTool.gameObject.SetActive(true);
				});
			}
			else
			{
				this.m_SeeingTool.gameObject.SetActive(true);
			}
			this.m_SeeingToolSequence.Insert(num, this.m_SeeingTool.DOLocalMoveY(0f, 0.4f, false).SetEase(Ease.OutQuad));
			S13AudioManager.Instance.InvokeEvent("evt_seeing_tool_on", 0f);
			return;
		}
		this.m_SeeingToolSequence.Insert(num, this.m_SeeingTool.DOLocalMoveY(-5f, 0.4f, false).SetEase(Ease.InQuad));
		num += 0.4f;
		this.m_SeeingToolSequence.InsertCallback(num, delegate
		{
			this.m_SeeingTool.gameObject.SetActive(false);
			if (this.WeaponGameObject)
			{
				this.WeaponGameObject.SetActive(true);
			}
			this.SetInteraction(true);
			GameManager.Instance.ShowCrosshair();
		});
		if (this.WeaponGameObject)
		{
			this.m_SeeingToolSequence.Insert(num, this.WeaponGameObject.transform.DOLocalMoveY(0f, 0.4f, false).SetEase(Ease.OutQuad));
		}
		S13AudioManager.Instance.InvokeEvent("evt_seeing_tool_off", 0f);
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x000767C0 File Offset: 0x000749C0
	public void ForceHideSeeingTool(bool justHide = false)
	{
		this.isSeeingToolActive = false;
		Vector3 localPosition = this.m_SeeingTool.localPosition;
		localPosition.y = -5f;
		this.m_SeeingTool.localPosition = localPosition;
		this.m_SeeingTool.gameObject.SetActive(false);
		if (this.WeaponGameObject)
		{
			Vector3 localPosition2 = this.WeaponGameObject.transform.localPosition;
			localPosition2.y = 0f;
			this.WeaponGameObject.transform.localPosition = localPosition2;
		}
		if (!justHide)
		{
			this.SetInteraction(true);
			this.SetLockedMovement(false);
		}
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x00076858 File Offset: 0x00074A58
	public void GoToAndLookAt(Transform target)
	{
		base.transform.position = target.position;
		Vector3 zero = Vector3.zero;
		zero.x = target.eulerAngles.x;
		Vector3 zero2 = Vector3.zero;
		zero2.y = target.localEulerAngles.y;
		this.LookRotation(Quaternion.Euler(zero2), Quaternion.Euler(zero));
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0000FC81 File Offset: 0x0000DE81
	public void LookRotation(Quaternion playerRotation)
	{
		this.m_PlayerLook.ForceRotation(playerRotation);
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0000FC8F File Offset: 0x0000DE8F
	public void LookRotation(Quaternion playerRotation, Quaternion cameraRotation)
	{
		this.m_PlayerLook.ForceRotation(playerRotation);
		this.m_PlayerLook.ForceCameraRotation(cameraRotation);
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0000FCA9 File Offset: 0x0000DEA9
	public void LockRotation(float x, float y)
	{
		this.m_PlayerLook.HorizontalClampSetActive(true);
		this.m_PlayerLook.SetHorizontalClamp(x);
		this.m_PlayerLook.SetVerticalClamp(y);
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0000FCCF File Offset: 0x0000DECF
	public void UnlockRotation()
	{
		this.m_PlayerLook.HorizontalClampSetActive(false);
		this.m_PlayerLook.ResetVerticalClamp();
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
	public void SetInteraction(bool active)
	{
		this.m_Interaction.SetActive(active);
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0000FCF6 File Offset: 0x0000DEF6
	public void SetSensitivity(float value)
	{
		this.m_PlayerLook.SetSensitivity(value);
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x0000FD04 File Offset: 0x0000DF04
	public void SetLockedMovement(bool active)
	{
		this.isMoveLocked = active;
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0000FD0D File Offset: 0x0000DF0D
	public void SetLock(bool active, bool ignoreSeeingTool = false)
	{
		if (!ignoreSeeingTool)
		{
			this.ForceHideSeeingTool(true);
		}
		this.isLocked = active;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0000FD20 File Offset: 0x0000DF20
	public void SetSlowed(bool active)
	{
		this.isSlowed = active;
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0000FD29 File Offset: 0x0000DF29
	public void SetInkSpeed(bool active)
	{
		this.m_MoveSpeed = ((!active) ? this.m_OriginWalkSpeed : this.m_InkWalkSpeed);
		this.m_RunSpeed = ((!active) ? this.m_OriginRunSpeed : this.m_InkRunSpeed);
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0000FD59 File Offset: 0x0000DF59
	public void SetRun(bool active)
	{
		this.canRun = active;
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x0000FD62 File Offset: 0x0000DF62
	public void SetJump(bool active)
	{
		this.canJump = active;
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0000FD6B File Offset: 0x0000DF6B
	public void SetCollision(bool active)
	{
		this.m_CharacterController.enabled = active;
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x0000FD79 File Offset: 0x0000DF79
	public void SetCameraSway(bool active)
	{
		this.canCameraSway = active;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x0000FD82 File Offset: 0x0000DF82
	public void SetCombatStatus(CombatStatus playerStatus)
	{
		if (this.CurrentStatus == playerStatus)
		{
			return;
		}
		this.CurrentStatus = playerStatus;
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0000FD95 File Offset: 0x0000DF95
	public void SetVent(bool active)
	{
		this.SetSlowed(active);
		this.m_HeadBob.CrawlSetActive(active);
		this.SetJump(!active);
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
	public void SetFOVValue(float value, float duration)
	{
		this.m_CameraFOV.DOFov(value, duration);
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x0000FDC3 File Offset: 0x0000DFC3
	public void SetActiveFOV(bool active)
	{
		this.m_CameraFOV.SetActiveFOV(active);
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x0000FDD1 File Offset: 0x0000DFD1
	public void PlayPickUpSound()
	{
		this.m_PlayerFootsteps.AudioSwitch.Play("pickup");
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
	public void SetBackMovement(bool active)
	{
		this.m_CanMoveBack = active;
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x0000FDF1 File Offset: 0x0000DFF1
	public void PlayRespawnEffects()
	{
		S13AudioManager.Instance.InvokeEvent("evt_player_respawned", 0f);
		GameManager.Instance.AudioManager.Play("Audio/SFX/SFX_Respawn_01", AudioObjectType.SOUND_EFFECT, 0, false);
		this.m_Death.Play();
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x0000FE2A File Offset: 0x0000E02A
	public void Die()
	{
		this.SetCombatStatus(CombatStatus.None);
		S13AudioManager.Instance.InvokeEvent("evt_player_dead", 0f);
		this.OnDeath.Send(this);
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x0000FE53 File Offset: 0x0000E053
	protected override void OnDisposed()
	{
		GameManager.Instance.Player = null;
		base.OnDisposed();
	}

	// Token: 0x04000F3D RID: 3901
	[Header("Transforms")]
	[SerializeField]
	private Transform m_HeadContainer;

	// Token: 0x04000F3E RID: 3902
	[SerializeField]
	private Transform m_HandContainer;

	// Token: 0x04000F3F RID: 3903
	[SerializeField]
	private Transform m_WeaponParent;

	// Token: 0x04000F40 RID: 3904
	[SerializeField]
	private Transform m_CameraContainer;

	// Token: 0x04000F41 RID: 3905
	[SerializeField]
	private Transform m_SeeingTool;

	// Token: 0x04000F42 RID: 3906
	[Header("Movement Options")]
	[SerializeField]
	private float m_SlowedMoveSpeed = 4f;

	// Token: 0x04000F43 RID: 3907
	[SerializeField]
	public float m_MoveSpeed = 7f;

	// Token: 0x04000F44 RID: 3908
	[SerializeField]
	public float m_RunSpeed = 10f;

	// Token: 0x04000F45 RID: 3909
	[Header("Run Options")]
	[SerializeField]
	private bool m_EnableRun = true;

	// Token: 0x04000F46 RID: 3910
	[Header("Jump Options")]
	[SerializeField]
	private bool m_EnableJump = true;

	// Token: 0x04000F47 RID: 3911
	[SerializeField]
	private float m_JumpSpeed = 10f;

	// Token: 0x04000F48 RID: 3912
	[SerializeField]
	private List<AudioClip> m_JumpClips;

	// Token: 0x04000F49 RID: 3913
	[Header("Gravity")]
	[SerializeField]
	private bool m_EnableGravity = true;

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	public float m_StickToGroundForce = 10f;

	// Token: 0x04000F4B RID: 3915
	[SerializeField]
	private float m_GravityMultiplier = 3f;

	// Token: 0x04000F4C RID: 3916
	[Space]
	[SerializeField]
	private CharacterLook m_PlayerLook;

	// Token: 0x04000F4D RID: 3917
	[Space]
	[SerializeField]
	private CameraMovements m_CameraMovement;

	// Token: 0x04000F4E RID: 3918
	[Space]
	[SerializeField]
	private CameraFOV m_CameraFOV;

	// Token: 0x04000F4F RID: 3919
	[Space]
	[SerializeField]
	private CharacterFootsteps m_PlayerFootsteps;

	// Token: 0x04000F50 RID: 3920
	[Space]
	[SerializeField]
	private FirstPersonHeadBob m_HeadBob;

	// Token: 0x04000F51 RID: 3921
	[Space]
	[SerializeField]
	private InteractableInputController m_Interaction;

	// Token: 0x04000F52 RID: 3922
	[Space]
	[SerializeField]
	private PlayerDeath m_Death;

	// Token: 0x04000F53 RID: 3923
	public GameObject WeaponGameObject;

	// Token: 0x04000F54 RID: 3924
	public GameObject InactiveWeapon;

	// Token: 0x04000F5D RID: 3933
	private bool m_IsSeeingToolEnabled;

	// Token: 0x04000F5E RID: 3934
	private bool m_CanHaveSeeingTool;

	// Token: 0x04000F5F RID: 3935
	private CharacterController m_CharacterController;

	// Token: 0x04000F60 RID: 3936
	private Vector3 m_GroundNormal;

	// Token: 0x04000F61 RID: 3937
	private Vector2 m_Input;

	// Token: 0x04000F62 RID: 3938
	private Vector3 m_MoveDir = Vector3.zero;

	// Token: 0x04000F63 RID: 3939
	private float m_GravityPower;

	// Token: 0x04000F64 RID: 3940
	private Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x04000F65 RID: 3941
	private bool m_IsRunning;

	// Token: 0x04000F66 RID: 3942
	private bool m_CanMoveBack = true;

	// Token: 0x04000F67 RID: 3943
	private bool m_PreviouslyGrounded = true;

	// Token: 0x04000F68 RID: 3944
	private bool m_JumpInput;

	// Token: 0x04000F69 RID: 3945
	private float m_OriginWalkSpeed;

	// Token: 0x04000F6A RID: 3946
	private float m_OriginRunSpeed;

	// Token: 0x04000F6B RID: 3947
	private float m_InkWalkSpeed = 4f;

	// Token: 0x04000F6C RID: 3948
	private float m_InkRunSpeed = 7f;

	// Token: 0x04000F6D RID: 3949
	private bool m_LandingAudioBS;

	// Token: 0x04000F6E RID: 3950
	private bool m_ToggleRun;

	// Token: 0x04000F70 RID: 3952
	private Sequence m_SeeingToolSequence;

	// Token: 0x04000F71 RID: 3953
	private bool m_IsPaused;

	// Token: 0x04000F72 RID: 3954
	private bool setupCheats;
}
