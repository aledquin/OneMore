﻿//************************************************************************************************
// Copyright © 2020 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn
{
	using System.Linq;
	using System.Xml.Linq;


	internal enum StatusColor
	{
		Blue,
		Gray,
		Green,
		Red,
		Yellow
	}

	internal class InsertStatusCommand : Command
	{

		public InsertStatusCommand()
		{
		}


		public override void Execute(params object[] args)
		{
			var statusColor = (StatusColor)args[0];

			using (var one = new OneNote(out var page, out var ns))
			{
				if (!page.ConfirmBodyContext(true))
				{
					return;
				}

				var elements = page.Root.Descendants(ns + "T")
					.Where(e => e.Attribute("selected")?.Value == "all");

				if (elements != null)
				{
					string color = "black";
					string background = "yellow";

					switch (statusColor)
					{
						case StatusColor.Gray:
							color = "white";
							background = "#42526E";
							break;

						case StatusColor.Red:
							color = "white";
							background = "#BF2600";
							break;

						case StatusColor.Yellow:
							color = "172B4D";
							background = "#FF991F";
							break;

						case StatusColor.Green:
							color = "white";
							background = "#00875A";
							break;

						case StatusColor.Blue:
							color = "white";
							background = "#0052CC";
							break;
					}

					var colors = $"color:{color};background:{background}";
					var text = "     STATUS     ";

					var content = new XElement(ns + "T",
						new XCData(
							new XElement("span",
								new XAttribute("style",
									$"font-family:'Segoe UI';font-size:10.0pt;font-weight:bold;{colors}"),
								text
							).ToString(SaveOptions.DisableFormatting) + "&#160;")
						);

					page.ReplaceSelectedWithContent(content);

					one.Update(page);
				}
			}
		}
	}
}
