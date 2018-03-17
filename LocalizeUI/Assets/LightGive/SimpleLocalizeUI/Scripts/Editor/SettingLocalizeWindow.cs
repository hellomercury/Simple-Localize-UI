﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace LightGive
{
	public class SettingLocalizeWindow : EditorWindow
	{
		private LocalizeSettingData m_settingData;
		private int m_addSelectLanguageNo = 0;
		private int m_selectLanguageNo = 0;

		public LocalizeSettingData SettingData
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


		[MenuItem("Tools/LightGive/Localize/SettingWindow")]
		static void Open()
		{
			EditorWindow.GetWindow<SettingLocalizeWindow>("SettingLocalize");
		}

		void OnGUI()
		{
			if (SettingData == null)
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Please 'Tools/LightGive/Language/Create SettingData'");
				return;
			}

			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			{
				m_selectLanguageNo = EditorGUILayout.Popup("NowLanguage", m_selectLanguageNo, SettingData.CorrespondenceLanguageNameList.ToArray());
			}
			var isChange = EditorGUI.EndChangeCheck();
			if (isChange)
			{
				LocalizeSystem.ChangeLanguage(LocalizeDefine.GetLanguage(SettingData.CorrespondenceLanguageNameList[m_selectLanguageNo]));
				SceneView.RepaintAll();
			}

			EditorGUILayout.BeginVertical(GUI.skin.box);
			for (int i = 0; i < SettingData.CorrespondenceLanguageNameList.Count; i++)
			{
				int languageIndex = (int)LocalizeDefine.GetLanguage(SettingData.CorrespondenceLanguageNameList[i]);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(SettingData.CorrespondenceLanguageNameList[i]);

				EditorGUI.BeginChangeCheck();
				{
					SettingData.FontList[languageIndex] = (Font)EditorGUILayout.ObjectField(SettingData.FontList[languageIndex], typeof(Font), false);
				}
				var isChangeFont = EditorGUI.EndChangeCheck();
				if (isChangeFont)
				{
					if (SettingData.FontList[languageIndex] == null)
						SettingData.FontList[languageIndex] = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
					LocalizeSystem.SetFont(SettingData.FontList);
				}

				if (GUILayout.Button("Remove"))
				{
					SettingData.IsCorrespondence[languageIndex] = false;
					SettingData.ChangeNameList();
					Repaint();
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();

			if (SettingData.NotCorrespondenceLanguageNameList.Count != 0)
			{
				EditorGUILayout.BeginHorizontal();
				m_addSelectLanguageNo = EditorGUILayout.Popup(m_addSelectLanguageNo, SettingData.NotCorrespondenceLanguageNameList.ToArray());
				if (GUILayout.Button("Add language."))
				{
					int addIndex = (int)((SystemLanguage)Enum.Parse(typeof(SystemLanguage), SettingData.NotCorrespondenceLanguageNameList[m_addSelectLanguageNo]));
					SettingData.IsCorrespondence[addIndex] = true;
					SettingData.ChangeNameList();
					m_addSelectLanguageNo = 0;
					Repaint();
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}