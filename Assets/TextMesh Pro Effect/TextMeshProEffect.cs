using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000002 RID: 2
	[global::UnityEngine.RequireComponent(typeof(global::TMPro.TMP_Text))]
	[global::UnityEngine.ExecuteInEditMode]
	public class TextMeshProEffect : global::UnityEngine.MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Apply()
		{
			this.éÆ = base.GetComponent<global::TMPro.TMP_Text>();
			this.éô = this.Type;
			this.éö = (this.éô == global::TMPro.TextMeshProEffect.EffectType.Unfold || this.éô == global::TMPro.TextMeshProEffect.EffectType.Grow);
			this.éò = (this.éô == global::TMPro.TextMeshProEffect.EffectType.Sketch);
			this.éû = false;
			this.éù = -1f;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B4 File Offset: 0x000002B4
		private void OnEnable()
		{
			bool autoPlay = this.AutoPlay;
			if (autoPlay)
			{
				this.Play();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D4 File Offset: 0x000002D4
		private void OnValidate()
		{
			bool autoPlay = this.AutoPlay;
			if (autoPlay)
			{
				this.Play();
			}
			else
			{
				this.Apply();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC
		private void Update()
		{
			bool flag = this.éÆ == null;
			if (!flag)
			{
				bool flag2 = this.DurationInSeconds <= 0f;
				if (!flag2)
				{
					bool flag3 = !this.éû;
					if (!flag3)
					{
						bool flag4 = this.Repeat && this.IsFinished;
						if (flag4)
						{
							this.Play();
						}
						this.éÆ.ForceMeshUpdate();
						global::TMPro.TMP_TextInfo üù = this.éÆ.textInfo;
						int üÿ = üù.characterCount;
						bool flag5 = üÿ == 0;
						if (!flag5)
						{
							float üÖ = global::UnityEngine.Time.realtimeSinceStartup - this.éæ;
							bool flag6 = this.éò;
							if (flag6)
							{
								bool flag7 = this.éÿ != this.éÆ.text;
								if (flag7)
								{
									this.éù = -1f;
									this.éÿ = this.éÆ.text;
								}
								bool flag8 = üÖ >= this.éù;
								if (flag8)
								{
									this.éù = üÖ + this.DurationInSeconds;
									this.éÖ += 1;
									bool flag9 = this.éÜ.Length < üÿ * 2;
									if (flag9)
									{
										this.éÜ = new float[üÿ * 2];
									}
									for (int üí = 0; üí < this.éÜ.Length; üí++)
									{
										this.éÜ[üí] = global::UnityEngine.Random.value;
									}
								}
							}
							bool flag10 = this.éö && üÖ > this.DurationInSeconds;
							if (flag10)
							{
								üÖ = this.DurationInSeconds;
								this.IsFinished = true;
							}
							float üÜ = üÖ / this.DurationInSeconds;
							bool flag11 = !this.éö;
							if (flag11)
							{
								üÜ %= 1f;
							}
							float üƒ = this.CharacterDurationRatio;
							float üá = global::UnityEngine.Mathf.Lerp(1f / (float)üÿ, 1f, üƒ);
							for (int üó = 0; üó < üÿ; üó++)
							{
								global::TMPro.TMP_CharacterInfo üú = üù.characterInfo[üó];
								bool flag12 = !üú.isVisible;
								if (!flag12)
								{
									float üñ = global::UnityEngine.Mathf.Lerp((float)üó * 1f / (float)üÿ, 0f, üƒ);
									float üÑ = (üÜ - üñ) / üá;
									üÑ = global::UnityEngine.Mathf.Clamp01(üÑ);
									int üª = üú.materialReferenceIndex;
									int üº = üú.vertexIndex;
									global::UnityEngine.Color32[] éÇ = üù.meshInfo[üª].colors32;
									global::TMPro.TMP_MeshInfo[] éü = üù.CopyMeshInfoVertexData();
									global::UnityEngine.Vector3[] éé = éü[üª].vertices;
									global::UnityEngine.Vector3[] éâ = üù.meshInfo[üª].vertices;
									this.è(üù, üú, üº, éÇ, éâ, éé, üÜ, üÑ, this.éÖ);
								}
							}
							for (int éä = 0; éä < üù.meshInfo.Length; éä++)
							{
								üù.meshInfo[éä].mesh.vertices = üù.meshInfo[éä].vertices;
								this.éÆ.UpdateGeometry(üù.meshInfo[éä].mesh, éä);
							}
							this.éÆ.UpdateVertexData(global::TMPro.TMP_VertexDataUpdateFlags.Colors32);
						}
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002434 File Offset: 0x00000634
		private void è(global::TMPro.TMP_TextInfo ï, global::TMPro.TMP_CharacterInfo î, int ì, global::UnityEngine.Color32[] Ä, global::UnityEngine.Vector3[] Å, global::UnityEngine.Vector3[] É, float æ, float Æ, ushort ô)
		{
			bool flag = this.éò;
			if (flag)
			{
				this.í(î, ì, Ä, this.ö((int)this.éÖ + î.index));
			}
			else
			{
				this.í(î, ì, Ä, Æ);
			}
			switch (this.Type)
			{
			case global::TMPro.TextMeshProEffect.EffectType.Waves:
				this.ª(î, ì, Å, É, æ);
				break;
			case global::TMPro.TextMeshProEffect.EffectType.Grow:
				this.üä(î, ì, Å, É, Æ);
				break;
			case global::TMPro.TextMeshProEffect.EffectType.Unfold:
				this.üè(î, ì, Å, É, Æ);
				break;
			case global::TMPro.TextMeshProEffect.EffectType.UnfoldAndWaves:
				this.üè(î, ì, Å, É, Æ);
				this.ª(î, ì, Å, Å, æ);
				break;
			case global::TMPro.TextMeshProEffect.EffectType.Sketch:
				this.û(î, ì, Å, É, Æ, (int)ô);
				break;
			default:
				throw new global::System.ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000250C File Offset: 0x0000070C
		private float ö(int ò)
		{
			int éà = global::UnityEngine.Mathf.Abs(ò % this.éÜ.Length);
			return this.éÜ[éà];
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002538 File Offset: 0x00000738
		private void û(global::TMPro.TMP_CharacterInfo ù, int ÿ, global::UnityEngine.Vector3[] Ö, global::UnityEngine.Vector3[] Ü, float ƒ, int á)
		{
			global::TMPro.TextMeshProEffect.â éå;
			éå.ä = ù;
			éå.à = this;
			Ö[ÿ] = Ü[ÿ] - this.üÉ(ÿ, á, ref éå);
			Ö[ÿ + 1] = Ü[ÿ + 1] - this.üÉ(ÿ + 1, á, ref éå);
			Ö[ÿ + 2] = Ü[ÿ + 2] - this.üÉ(ÿ + 2, á, ref éå);
			Ö[ÿ + 3] = Ü[ÿ + 3] - this.üÉ(ÿ + 3, á, ref éå);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000025E8 File Offset: 0x000007E8
		private void í(global::TMPro.TMP_CharacterInfo ó, int ú, global::UnityEngine.Color32[] ñ, float Ñ)
		{
			global::UnityEngine.Color éç = this.Gradient.Evaluate(Ñ);
			bool flag = this.Mix == global::TMPro.TextMeshProEffect.MixType.Multiply;
			if (flag)
			{
				ñ[ú] *= éç;
				int num = ú + 1;
				ñ[num] *= éç;
				int num2 = ú + 2;
				ñ[num2] *= éç;
				int num3 = ú + 3;
				ñ[num3] *= éç;
			}
			else
			{
				for (int éê = 0; éê < 4; éê++)
				{
					global::UnityEngine.Color éë = ñ[ú + éê] + éç;
					éë.a *= éç.a;
					ñ[ú + éê] = éë;
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026F4 File Offset: 0x000008F4
		private void ª(global::TMPro.TMP_CharacterInfo º, int üÇ, global::UnityEngine.Vector3[] üü, global::UnityEngine.Vector3[] üé, float üâ)
		{
			global::TMPro.TextMeshProEffect.å éè;
			éè.ç = üâ;
			éè.ê = º;
			éè.ë = this;
			üü[üÇ] = üé[üÇ] - this.üö(üÇ, ref éè);
			üü[üÇ + 1] = üé[üÇ + 1] - this.üö(üÇ + 1, ref éè);
			üü[üÇ + 2] = üé[üÇ + 2] - this.üö(üÇ + 2, ref éè);
			üü[üÇ + 3] = üé[üÇ + 3] - this.üö(üÇ + 3, ref éè);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000027A4 File Offset: 0x000009A4
		private void üä(global::TMPro.TMP_CharacterInfo üà, int üå, global::UnityEngine.Vector3[] üç, global::UnityEngine.Vector3[] üê, float üë)
		{
			üç[üå] = üê[üå];
			üç[üå + 3] = üê[üå + 3];
			üç[üå + 1] = global::UnityEngine.Vector3.Lerp(üê[üå], üê[üå + 1], üë);
			üç[üå + 2] = global::UnityEngine.Vector3.Lerp(üê[üå + 3], üê[üå + 2], üë);
			üç[üå] = global::UnityEngine.Vector3.LerpUnclamped(üê[üå], üç[üå], this.Amplitude);
			üç[üå + 1] = global::UnityEngine.Vector3.LerpUnclamped(üê[üå + 1], üç[üå + 1], this.Amplitude);
			üç[üå + 2] = global::UnityEngine.Vector3.LerpUnclamped(üê[üå + 2], üç[üå + 2], this.Amplitude);
			üç[üå + 3] = global::UnityEngine.Vector3.LerpUnclamped(üê[üå + 3], üç[üå + 3], this.Amplitude);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028B0 File Offset: 0x00000AB0
		private void üè(global::TMPro.TMP_CharacterInfo üï, int üî, global::UnityEngine.Vector3[] üì, global::UnityEngine.Vector3[] üÄ, float üÅ)
		{
			global::UnityEngine.Vector3 éï = (üÄ[üî] + üÄ[üî + 1]) * 0.5f;
			global::UnityEngine.Vector3 éî = (üÄ[üî + 3] + üÄ[üî + 2]) * 0.5f;
			üì[üî] = global::UnityEngine.Vector3.Lerp(éï, üÄ[üî], üÅ);
			üì[üî + 3] = global::UnityEngine.Vector3.Lerp(éî, üÄ[üî + 3], üÅ);
			üì[üî + 1] = global::UnityEngine.Vector3.Lerp(éï, üÄ[üî + 1], üÅ);
			üì[üî + 2] = global::UnityEngine.Vector3.Lerp(éî, üÄ[üî + 2], üÅ);
			üì[üî] = global::UnityEngine.Vector3.LerpUnclamped(üÄ[üî], üì[üî], this.Amplitude);
			üì[üî + 1] = global::UnityEngine.Vector3.LerpUnclamped(üÄ[üî + 1], üì[üî + 1], this.Amplitude);
			üì[üî + 2] = global::UnityEngine.Vector3.LerpUnclamped(üÄ[üî + 2], üì[üî + 2], this.Amplitude);
			üì[üî + 3] = global::UnityEngine.Vector3.LerpUnclamped(üÄ[üî + 3], üì[üî + 3], this.Amplitude);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002A02 File Offset: 0x00000C02
		public void Play()
		{
			this.Apply();
			this.IsFinished = false;
			this.éæ = global::UnityEngine.Time.realtimeSinceStartup;
			this.éû = true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002A28 File Offset: 0x00000C28
		public TextMeshProEffect()
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002A84 File Offset: 0x00000C84
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		private global::UnityEngine.Vector3 üÉ(int üæ, int üÆ, ref global::TMPro.TextMeshProEffect.â üô)
		{
			float éì = üô.ä.pointSize * 0.1f * this.Amplitude;
			float éÄ = this.ö(üæ << üÆ);
			float éÅ = this.ö(üæ << üÆ >> 5);
			return new global::UnityEngine.Vector3(éÄ * éì, éÅ * éì, 0f);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002ADC File Offset: 0x00000CDC
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		private global::UnityEngine.Vector3 üö(int üò, ref global::TMPro.TextMeshProEffect.å üû)
		{
			float éÉ = -6.2831855f * üû.ç + (float)(üò / 4) * 0.3f;
			return new global::UnityEngine.Vector3(0f, global::UnityEngine.Mathf.Cos(éÉ) * üû.ê.pointSize * 0.3f * this.Amplitude, 0f);
		}

		// Token: 0x04000001 RID: 1
		public global::TMPro.TextMeshProEffect.EffectType Type;

		// Token: 0x04000002 RID: 2
		public float DurationInSeconds = 0.5f;

		// Token: 0x04000003 RID: 3
		public float Amplitude = 1f;

		// Token: 0x04000004 RID: 4
		[global::UnityEngine.Range(0f, 1f)]
		public float CharacterDurationRatio = 0f;

		// Token: 0x04000005 RID: 5
		[global::UnityEngine.Space]
		public global::UnityEngine.Gradient Gradient = new global::UnityEngine.Gradient();

		// Token: 0x04000006 RID: 6
		public global::TMPro.TextMeshProEffect.MixType Mix = global::TMPro.TextMeshProEffect.MixType.Multiply;

		// Token: 0x04000007 RID: 7
		[global::UnityEngine.Space]
		public bool AutoPlay = true;

		// Token: 0x04000008 RID: 8
		public bool Repeat;

		// Token: 0x04000009 RID: 9
		[global::UnityEngine.HideInInspector]
		public bool IsFinished;

		// Token: 0x0400000A RID: 10
		private float éæ;

		// Token: 0x0400000B RID: 11
		private global::TMPro.TMP_Text éÆ;

		// Token: 0x0400000C RID: 12
		private global::TMPro.TextMeshProEffect.EffectType éô;

		// Token: 0x0400000D RID: 13
		private bool éö;

		// Token: 0x0400000E RID: 14
		private bool éò;

		// Token: 0x0400000F RID: 15
		private bool éû;

		// Token: 0x04000010 RID: 16
		private float éù;

		// Token: 0x04000011 RID: 17
		private string éÿ;

		// Token: 0x04000012 RID: 18
		private ushort éÖ;

		// Token: 0x04000013 RID: 19
		private float[] éÜ = new float[10];

		// Token: 0x02000003 RID: 3
		public enum EffectType : byte
		{
			// Token: 0x04000015 RID: 21
			Waves,
			// Token: 0x04000016 RID: 22
			Grow,
			// Token: 0x04000017 RID: 23
			Unfold,
			// Token: 0x04000018 RID: 24
			UnfoldAndWaves,
			// Token: 0x04000019 RID: 25
			Sketch
		}

		// Token: 0x02000004 RID: 4
		public enum MixType : byte
		{
			// Token: 0x0400001B RID: 27
			Multiply,
			// Token: 0x0400001C RID: 28
			Add
		}

		// Token: 0x02000005 RID: 5
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Auto)]
		private struct â
		{
			// Token: 0x0400001D RID: 29
			public global::TMPro.TMP_CharacterInfo ä;

			// Token: 0x0400001E RID: 30
			public global::TMPro.TextMeshProEffect à;
		}

		// Token: 0x02000006 RID: 6
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Auto)]
		private struct å
		{
			// Token: 0x0400001F RID: 31
			public float ç;

			// Token: 0x04000020 RID: 32
			public global::TMPro.TMP_CharacterInfo ê;

			// Token: 0x04000021 RID: 33
			public global::TMPro.TextMeshProEffect ë;
		}
	}
}