using System;
using System.Collections;
using System.Collections.Generic;
using Ai;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class CH3BendyController : BaseController
{
	// Token: 0x0600065E RID: 1630 RVA: 0x00007B08 File Offset: 0x00005D08
	public CH3BendyController()
	{
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x00007707 File Offset: 0x00005907
	private BorisAi m_Boris
	{
		get
		{
			return GameManager.Instance.CharacterManager.Boris;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x00007B31 File Offset: 0x00005D31
	// (set) Token: 0x06000661 RID: 1633 RVA: 0x00007B39 File Offset: 0x00005D39
	public BendyAi Bendy { get; private set; }

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x00007B42 File Offset: 0x00005D42
	// (set) Token: 0x06000663 RID: 1635 RVA: 0x00007B4A File Offset: 0x00005D4A
	public bool IsActive { get; private set; }

	// Token: 0x06000664 RID: 1636 RVA: 0x00007B53 File Offset: 0x00005D53
	public override void InitOnComplete()
	{
		base.InitOnComplete();
		this.m_BendyMusicClip = GameManager.Instance.GetAudioClip("Audio/MUS/MUS_Little_Devil_Darling_Remastered");
		GameManager.Instance.CurrentChapter.DeathController.OnDeath += this.HandlePlayerOnDeath;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00007B90 File Offset: 0x00005D90
	private void HandlePlayerOnDeath(object sender, EventArgs e)
	{
		this.KillBendy();
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0003B3B8 File Offset: 0x000395B8
	private void Update()
	{
		if (GameManager.Instance.isPaused)
		{
			return;
		}
		if (this.m_PlayChaseMusic && GameManager.Instance.Player.CurrentStatus == CombatStatus.Hiding)
		{
			this.m_PlayChaseMusic = false;
		}
		if (!this.m_PlayChaseMusic && this.m_IsPlayingMusic && this.m_BendyMusic != null)
		{
			this.m_IsPlayingMusic = false;
			this.m_BendyMusic.AudioSource.DOKill(false);
			this.m_BendyMusic.AudioSource.DOFade(0f, 2f).OnComplete(delegate
			{
				if (!this.m_PlayChaseMusic)
				{
					this.m_BendyMusic.Clear();
					this.m_BendyMusic = null;
				}
			});
		}
		if (!this.IsActive || !this.m_CanSpawn)
		{
			return;
		}
		if (!this.Bendy)
		{
			this.m_Timer += Time.deltaTime;
			if (this.m_Timer > this.m_TimerLimit)
			{
				this.m_Timer = 0f;
				this.m_TimerLimit = UnityEngine.Random.Range(this.m_TimerMin, this.m_TimerMax);
				this.m_CanSpawn = false;
				this.SpawnBendy();
				return;
			}
		}
		else
		{
			Vector3 vector = this.Bendy.transform.position - GameManager.Instance.Player.transform.position;
			if (vector.magnitude > 250f || vector.y > 250f)
			{
				this.Bendy.ForceKill();
			}
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00007B98 File Offset: 0x00005D98
	public void SetSpawnTimer(float min, float max)
	{
		this.m_TimerMin = min;
		this.m_TimerMax = max;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00007BA8 File Offset: 0x00005DA8
	public void ResetSpawnTimer()
	{
		this.m_TimerMin = 10f;
		this.m_TimerMax = 20f;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0003B518 File Offset: 0x00039718
	public void SetActive(bool active)
	{
		this.IsActive = active;
		if (this.IsActive)
		{
			base.StopAllCoroutines();
			this.KillBendy();
			this.ClearAllSpawners();
			this.m_CanSpawn = true;
			base.DebugLog("[BENDY] - Bendy will spawn in " + this.m_TimerLimit + " seconds.");
			return;
		}
		base.StopAllCoroutines();
		this.KillBendy();
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0003B57C File Offset: 0x0003977C
	public void ForceSpawn()
	{
		this.m_Timer = 0f;
		this.m_TimerLimit = UnityEngine.Random.Range(this.m_TimerMin, this.m_TimerMax);
		this.m_CanSpawn = false;
		base.StopAllCoroutines();
		this.KillBendy();
		this.ClearAllSpawners();
		this.SpawnBendy();
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0003B5CC File Offset: 0x000397CC
	private void SpawnBendy()
	{
		if (this.m_BendySpawnerList == null || this.m_BendySpawnerList.Count <= 0)
		{
			return;
		}
		this.m_ActiveList = this.GetClosestSpawner();
		if (this.m_ActiveList == null)
		{
			base.DebugLog("No Spawner Found - Not Spawning Bendy...");
			return;
		}
		BendySpawner bendySpawner = null;
		BendySpawner bendySpawner2 = null;
		int num = UnityEngine.Random.Range(0, this.m_ActiveList.BendySpawners.Count);
		for (int i = 0; i < this.m_ActiveList.BendySpawners.Count; i++)
		{
			if (i == num)
			{
				bendySpawner = this.m_ActiveList.BendySpawners[i];
			}
			else
			{
				bendySpawner2 = this.m_ActiveList.BendySpawners[i];
			}
		}
		this.m_Boris.SetCower(true);
		this.Bendy = UnityEngine.Object.Instantiate<BendyAi>(this.m_BendyPrefab);
		base.DebugLog("[BENDY] - Spawned at (" + this.m_ActiveList.gameObject.name + ")");
		this.m_ActiveList.Use();
		this.Bendy.transform.position = bendySpawner.Watpoints[0].transform.position;
		this.Bendy.transform.eulerAngles = bendySpawner.Watpoints[0].transform.eulerAngles;
		this.Bendy.OnWaypointComplete += this.HandleBendyOnWaypointComplete;
		this.Bendy.OnSpotted += this.HandleBendyOnSpotted;
		this.Bendy.OnTrackingLost += this.HandleBendyOnTrackingLost;
		this.Bendy.UpdateWaypointList(bendySpawner2.Watpoints, false);
		this.Bendy.SetPassive(true);
		base.StartCoroutine(this.SetBendyNotPassive());
		this.m_BendyDespawner.OnEnter -= this.HandleBendyDespawnerOnEnter;
		this.m_BendyDespawner.OnEnter += this.HandleBendyDespawnerOnEnter;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00007BC0 File Offset: 0x00005DC0
	private void HandleBendyOnTrackingLost(object sender, EventArgs e)
	{
		this.StopChaseMusic();
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00007BC8 File Offset: 0x00005DC8
	private IEnumerator SetBendyNotPassive()
	{
		yield return new WaitForSeconds(5f);
		yield return new WaitForEndOfFrame();
		if (base.IsDisposed)
		{
			yield break;
		}
		if (this.Bendy)
		{
			this.Bendy.SetPassive(false);
		}
		yield break;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0003B7B0 File Offset: 0x000399B0
	private void HandleBendyDespawnerOnEnter(object sender, EventArgs e)
	{
		this.m_BendyDespawner.OnEnter -= this.HandleBendyDespawnerOnEnter;
		base.DebugLog("[BENDY] - Bendy has been despawned.");
		this.m_CanSpawn = true;
		this.m_ActiveList.Reset();
		if (this.Bendy)
		{
			this.m_Boris.SetCower(false);
			this.Bendy.ForceKill();
			this.Bendy = null;
		}
		if (this.m_BendyMusic != null)
		{
			this.m_IsPlayingMusic = false;
			this.m_BendyMusic.AudioSource.DOKill(false);
			this.m_BendyMusic.AudioSource.DOFade(0f, 2f).OnComplete(delegate
			{
				if (this.m_BendyMusic)
				{
					this.m_BendyMusic.Clear();
					this.m_BendyMusic = null;
				}
			});
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0003B870 File Offset: 0x00039A70
	private void HandleBendyOnSpotted(object sender, EventArgs e)
	{
		this.m_PlayChaseMusic = true;
		if (this.m_BendyMusic != null)
		{
			this.m_BendyMusic.Clear();
			this.m_BendyMusic = null;
		}
		this.m_IsPlayingMusic = true;
		this.m_BendyMusic = GameManager.Instance.AudioManager.Play(this.m_BendyMusicClip, AudioObjectType.MUSIC, -1, false);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00007BD7 File Offset: 0x00005DD7
	public void StopChaseMusic()
	{
		this.m_PlayChaseMusic = false;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0003B8CC File Offset: 0x00039ACC
	private void HandleBendyOnWaypointComplete(object sender, EventArgs e)
	{
		this.m_CanSpawn = true;
		if (this.Bendy)
		{
			this.Bendy.OnWaypointComplete -= this.HandleBendyOnWaypointComplete;
			this.Bendy.OnSpotted -= this.HandleBendyOnSpotted;
		}
		if (this.m_BendyMusic != null)
		{
			this.m_IsPlayingMusic = false;
			this.m_BendyMusic.AudioSource.DOKill(false);
			this.m_BendyMusic.AudioSource.DOFade(0f, 2f).OnComplete(delegate
			{
				this.m_BendyMusic.Clear();
				this.m_BendyMusic = null;
			});
		}
		this.m_ActiveList.Reset();
		this.KillBendy();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0003B980 File Offset: 0x00039B80
	public void GoToCutout()
	{
		if (this.Bendy)
		{
			List<WaypointNode> list = new List<WaypointNode>();
			list.Add(this.GetClosestCutout());
			if (list.Count > 0)
			{
				this.Bendy.OnWaypointComplete -= this.HandleBendyOnWaypointComplete;
				this.Bendy.OnWaypointComplete -= this.HandleBendyCutoutWaypointComplete;
				this.Bendy.OnWaypointComplete += this.HandleBendyCutoutWaypointComplete;
				this.Bendy.UpdateWaypointList(list, true);
				this.m_BendyDespawner.OnEnter -= this.HandleBendyDespawnerOnEnter;
				this.m_BendyDespawner.OnEnter += this.HandleBendyDespawnerOnEnter;
				return;
			}
		}
		else
		{
			this.SpawnBendy();
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0003BA44 File Offset: 0x00039C44
	private void HandleBendyCutoutWaypointComplete(object sender, EventArgs e)
	{
		this.Bendy.OnWaypointComplete -= this.HandleBendyCutoutWaypointComplete;
		this.m_ActiveList = this.GetClosestSpawner();
		BendySpawner bendySpawner = null;
		int num = UnityEngine.Random.Range(0, this.m_ActiveList.BendySpawners.Count);
		for (int i = 0; i < this.m_ActiveList.BendySpawners.Count; i++)
		{
			if (i == num)
			{
				bendySpawner = this.m_ActiveList.BendySpawners[i];
			}
		}
		this.m_ActiveList.Use();
		this.Bendy.OnWaypointComplete += this.HandleBendyOnWaypointComplete;
		this.Bendy.UpdateWaypointList(bendySpawner.Watpoints, false);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0003BAF4 File Offset: 0x00039CF4
	private BendySpawnerList GetClosestSpawner()
	{
		BendySpawnerList result = null;
		float num = float.PositiveInfinity;
		Vector3 position = GameManager.Instance.Player.transform.position;
		foreach (BendySpawnerList bendySpawnerList in this.m_BendySpawnerList)
		{
			foreach (BendySpawner bendySpawner in bendySpawnerList.BendySpawners)
			{
				float sqrMagnitude = (bendySpawner.transform.position - position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = bendySpawner.BendySpawnerList;
				}
			}
		}
		return result;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0003BBC8 File Offset: 0x00039DC8
	private WaypointNode GetClosestCutout()
	{
		WaypointNode result = null;
		float num = float.PositiveInfinity;
		Vector3 position = GameManager.Instance.Player.transform.position;
		foreach (WaypointNode waypointNode in this.m_CutoutWaypoints)
		{
			float sqrMagnitude = (waypointNode.transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				result = waypointNode;
			}
		}
		return result;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0003BC5C File Offset: 0x00039E5C
	private void KillBendy()
	{
		this.m_Boris.SetCower(false);
		if (this.Bendy)
		{
			this.Bendy.Dispose();
			this.Bendy = null;
		}
		if (this.m_BendyMusic)
		{
			this.m_BendyMusic.AudioSource.DOKill(false);
			this.m_BendyMusic.Clear();
			this.m_BendyMusic = null;
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0003BCC8 File Offset: 0x00039EC8
	private void ClearAllSpawners()
	{
		for (int i = 0; i < this.m_BendySpawnerList.Count; i++)
		{
			this.m_BendySpawnerList[i].Reset();
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00007BE0 File Offset: 0x00005DE0
	protected override void OnDisposed()
	{
		this.m_ActiveList = null;
		this.m_BendyMusic = null;
		this.m_BendyMusicClip = null;
		base.OnDisposed();
	}

	// Token: 0x04000504 RID: 1284
	private const float DEFAULT_TIMER_MIN = 180f;

	// Token: 0x04000505 RID: 1285
	private const float DEFAULT_TIMER_MAX = 240f;

	// Token: 0x04000506 RID: 1286
	private const float BENDY_GRACE_PERIOD = 5f;

	// Token: 0x04000507 RID: 1287
	[Header("Bendy")]
	[SerializeField]
	private BendyAi m_BendyPrefab;

	// Token: 0x04000508 RID: 1288
	[Header("Cutout Waypoints")]
	[SerializeField]
	private List<WaypointNode> m_CutoutWaypoints;

	// Token: 0x04000509 RID: 1289
	[Header("Spawners")]
	[SerializeField]
	private List<BendySpawnerList> m_BendySpawnerList;

	// Token: 0x0400050A RID: 1290
	[Header("Events")]
	[SerializeField]
	private EventTrigger m_BendyDespawner;

	// Token: 0x0400050C RID: 1292
	private BendySpawnerList m_ActiveList;

	// Token: 0x0400050D RID: 1293
	private AudioObject m_BendyMusic;

	// Token: 0x0400050E RID: 1294
	private AudioClip m_BendyMusicClip;

	// Token: 0x0400050F RID: 1295
	private bool m_PlayChaseMusic;

	// Token: 0x04000510 RID: 1296
	private bool m_CanSpawn;

	// Token: 0x04000511 RID: 1297
	private float m_TimerMin = 120f;

	// Token: 0x04000512 RID: 1298
	private float m_TimerMax = 240f;

	// Token: 0x04000513 RID: 1299
	private float m_Timer;

	// Token: 0x04000514 RID: 1300
	private float m_TimerLimit = 300f;

	// Token: 0x04000516 RID: 1302
	private bool m_IsPlayingMusic;
}
