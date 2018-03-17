﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LightGive
{
	public static class LocalizeSystem
	{
		private static List<ILocalizeUI> m_localizeList = new List<ILocalizeUI>();
		private static LocalizeSettingData m_settingData;

		/// <summary>
		/// 設定のデータ
		/// </summary>
		/// <value>The setting data.</value>
		public static LocalizeSettingData SettingData
		{
			get
			{
				if (m_settingData == null)
				{
					m_settingData = Resources.Load<LocalizeSettingData>(LocalizeDefine.SettingPath);
				}
				return m_settingData;
			}
		}

		/// <summary>
		/// シーンを読み込む前
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitAwakeBefore()
		{
			m_settingData = Resources.Load<LocalizeSettingData>(LocalizeDefine.SettingPath);
		}

		/// <summary>
		/// シーンが読み込まれた後
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitAwakeAfter()
		{
			ChangeLanguage(SettingData.NowLanguage);
		}

		public static List<SystemLanguage> GetCorrespondenceLanguageList()
		{
			List<SystemLanguage> correspondenceLanguageList = new List<SystemLanguage>();
			for (int i = 0; i < LocalizeDefine.LanguageNum;i++)
			{
				if (SettingData == null)
					break;
				if (SettingData.IsCorrespondence[i])
					correspondenceLanguageList.Add((SystemLanguage)i);
			}
			return correspondenceLanguageList;
		}

		public static void SetFont(Font[] _fonts)
		{
			List<LocalizeText> textList = new List<LocalizeText>();
			LocalizeText tmp;
			foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
			{
				if (obj.activeInHierarchy)
				{
					tmp = obj.GetComponent<LocalizeText>();
					if (tmp == null)
						continue;
					textList.Add(tmp);
				}
			}

			for (int i = 0; i < textList.Count;i++)
			{
				textList[i].SetFont(_fonts);
			}
		}

		public static void ChangeLanguage(SystemLanguage _language)
		{
			SettingData.NowLanguage = _language;
			m_localizeList = new List<ILocalizeUI>();
			ILocalizeUI tmp;
			foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
			{
				if (obj.activeInHierarchy)
				{
					tmp = (ILocalizeUI)obj.GetComponent(typeof(ILocalizeUI));
					if (tmp == null)
						continue;
					m_localizeList.Add(tmp);
				}
			}

			for (int i = 0; i < m_localizeList.Count; i++)
			{
				if (m_localizeList[i] == null)
					continue;
				
				m_localizeList[i].ChangeLanguage(_language);
			}
		}
	}
}