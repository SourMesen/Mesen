using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms
{
	public enum Language
	{
		SystemDefault = 0,
		English = 1,
		French = 2,
		Japanese = 3
	}

	class ResourceHelper
	{
		private static Language _language;
		private static XmlDocument _resources = new XmlDocument();
		private static XmlDocument _originalEnglishResources = null;

		public static void LoadResources(Language language)
		{
			if(language == Language.SystemDefault) {
				switch(System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) {
					default:
					case "en": language = Language.English; break;
					case "fr": language = Language.French; break;
					case "ja": language = Language.Japanese; break;
				}
			}

			string filename;
			switch(language) {
				default:
				case Language.English: filename = "resources.en.xml"; break;
				case Language.French: filename = "resources.fr.xml"; break;
				case Language.Japanese: filename = "resources.ja.xml"; break;
			}

			_language = language;
			InteropEmu.SetDisplayLanguage(language);

			using(Stream stream = ResourceManager.GetZippedResource(filename)) {
				_resources.Load(stream);
			}
		}

		public static string GetMessage(string id, params string[] args)
		{
			var baseNode = _resources.SelectSingleNode("/Resources/Messages/Message[@ID='" + id + "']");
			if(baseNode != null) {
				return string.Format(baseNode.InnerText, args);
			} else {
				return "[[" + id + "]]";
			}
		}

		public static string GetEnumText(Enum e)
		{
			var baseNode = _resources.SelectSingleNode("/Resources/Enums/Enum[@ID='" + e.GetType().Name + "']/Value[@ID='" + e.ToString() + "']");
			if(baseNode != null) {
				return baseNode.InnerText;
			} else {
				return e.ToString();
			}
		}

		public static void ApplyResources(Form form)
		{
			if(form is frmMain && _originalEnglishResources == null) {
				_originalEnglishResources = BuildResourceFile(form);
			}

			XmlNode baseNode = null;
			if(form is frmMain && _language == Language.English) {
				baseNode = _originalEnglishResources.SelectSingleNode("/Resources/Forms/Form[@ID='" + form.Name + "']");
			} else {
				baseNode = _resources.SelectSingleNode("/Resources/Forms/Form[@ID='" + form.Name + "']");
			}

			if(baseNode != null) {
				form.Text = baseNode.Attributes["Title"].Value;
				ApplyResources(baseNode, form.Controls);
			}
		}

		private static void ApplyResources(XmlNode baseNode, IEnumerable container)
		{
			foreach(object ctrl in container) {
				string name = null;
				if(ctrl is Control) {
					name = ((Control)ctrl).Name;
				} else if(ctrl is ToolStripItem) {
					name = ((ToolStripItem)ctrl).Name;
				} else if(ctrl is ColumnHeader) {
					name = ((ColumnHeader)ctrl).Name;
				}

				var controlNode = baseNode.SelectSingleNode("Control[@ID='" + name + "']");
				if(controlNode != null) {
					if(ctrl is Control) {
						((Control)ctrl).Text = controlNode.InnerText;
					} else if(ctrl is ToolStripItem) {
						((ToolStripItem)ctrl).Text = controlNode.InnerText;
					} else if(ctrl is ColumnHeader) {
						((ColumnHeader)ctrl).Text = controlNode.InnerText;
					}
				}

				if(ctrl is MenuStrip) {
					ApplyResources(baseNode, ((MenuStrip)ctrl).Items);
				} else if(ctrl is ContextMenuStrip) {
					ApplyResources(baseNode, ((ContextMenuStrip)ctrl).Items);
				} else if(ctrl is ListView) {
					ApplyResources(baseNode, ((ListView)ctrl).Columns);
					if(((ListView)ctrl).ContextMenuStrip != null) {
						ApplyResources(baseNode, ((ListView)ctrl).ContextMenuStrip.Items);
					}
				} else if(ctrl is Control) {
					ApplyResources(baseNode, ((Control)ctrl).Controls);
				} else if(ctrl is ToolStripMenuItem) {
					ApplyResources(baseNode, ((ToolStripMenuItem)ctrl).DropDownItems);
				}
			}
		}

		private static XmlDocument BuildResourceFile(Form form)
		{
			XmlDocument document = new XmlDocument();
			XmlNode resources = document.CreateElement("Resources");
			document.AppendChild(resources);
			resources.AppendChild(document.CreateElement("Forms"));

			BuildResourceFile(document, form, form.Controls);

			return document;
		}

		private static void BuildResourceFile(XmlDocument xmlDoc, Form form, IEnumerable container)
		{
			var baseNode = xmlDoc.SelectSingleNode("/Resources/Forms/Form[@ID='" + form.Name + "']");
			if(baseNode == null) {
				baseNode = xmlDoc.CreateElement("Form");
				baseNode.Attributes.Append(xmlDoc.CreateAttribute("ID"));
				baseNode.Attributes.Append(xmlDoc.CreateAttribute("Title"));
				baseNode.Attributes["ID"].Value = form.Name;
				baseNode.Attributes["Title"].Value = form.Text;
				xmlDoc.SelectSingleNode("/Resources/Forms").AppendChild(baseNode);
			}

			foreach(Component ctrl in container) {
				string text = null;
				string name = null;
				if(ctrl is Control) {
					text = ((Control)ctrl).Text;
					name = ((Control)ctrl).Name;
				} else if(ctrl is ToolStripItem) {
					text = ((ToolStripItem)ctrl).Text;
					name = ((ToolStripItem)ctrl).Name;
				}

				if(!string.IsNullOrWhiteSpace(text)) {
					var controlNode = baseNode.SelectSingleNode("Control[@ID='" + name + "']");
					if(controlNode == null) {
						controlNode = xmlDoc.CreateElement("Control");
						controlNode.Attributes.Append(xmlDoc.CreateAttribute("ID"));
						controlNode.Attributes["ID"].Value = name;
						baseNode.AppendChild(controlNode);
					}
					controlNode.InnerText = text;
				}
				if(ctrl is MenuStrip) {
					BuildResourceFile(xmlDoc, form, ((MenuStrip)ctrl).Items);
				} else if(ctrl is Control) {
					BuildResourceFile(xmlDoc, form, ((Control)ctrl).Controls);
				} else if(ctrl is ToolStripMenuItem) {
					BuildResourceFile(xmlDoc, form, ((ToolStripMenuItem)ctrl).DropDownItems);
				}
			}
		}
	}
}
