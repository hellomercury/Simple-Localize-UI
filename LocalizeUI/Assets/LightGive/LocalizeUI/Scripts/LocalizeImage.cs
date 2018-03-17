﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LightGive
{
	[RequireComponent(typeof(Image))]
	public class LocalizeImage : MonoBehaviour, ILocalizeUI
	{
		[SerializeField]
		private Sprite[] m_spriteList = new Sprite[LocalizeDefine.LanguageNum];

		private Image m_mainImage;
		public Image MainImage
		{
			get
			{
				if (!m_mainImage)
				{
					if (this.gameObject == null) { return null; }
					m_mainImage = this.gameObject.GetComponent<Image>();
				}
				return m_mainImage;
			}
		}

		void Reset()
		{
			for (int i = 0; i < LocalizeDefine.LanguageNum; i++)
			{
				m_spriteList[i] = MainImage.sprite;
			}
		}

		public void ChangeLanguage(SystemLanguage _language)
		{
			int index = (int)_language;
			MainImage.sprite = m_spriteList[index];
		}
	}
}