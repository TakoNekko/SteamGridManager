using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class DetailsPanelOptionGroupBox : UserControl
	{
		private readonly bool designMode;
		private readonly Item libraryCapsule;
		private readonly Item heroGraphic;
		private readonly Item logo;
		private readonly Item header;
		private readonly Item icon;

		public GroupBox GroupBox => optionsGroupBox;
		public Item LibraryCapsule => libraryCapsule;
		public Item HeroGraphic => heroGraphic;
		public Item Logo => logo;
		public Item Header => header;
		public Item Icon => icon;

		public DetailsPanelOptionGroupBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				libraryCapsule = new Item { Panel = libraryCapsulePanelBackgroundColorPanel, Label = libraryCapsulePanelBackgroundColorLabel, ColorBox = libraryCapsulePanelBackgroundColorBox };
				heroGraphic = new Item { Panel = heroGraphicPanelBackgroundColorPanel, Label = heroGraphicPanelBackgroundColorLabel, ColorBox = heroGraphicPanelBackgroundColorBox };
				logo = new Item { Panel = logoPanelBackgroundColorPanel, Label = logoPanelBackgroundColorLabel, ColorBox = logoPanelBackgroundColorBox };
				header = new Item { Panel = headerPanelBackgroundColorPanel, Label = headerPanelBackgroundColorLabel, ColorBox = headerPanelBackgroundColorBox };
				icon = new Item { Panel = iconPanelBackgroundColorPanel, Label = iconPanelBackgroundColorLabel, ColorBox = iconPanelBackgroundColorBox };
			}
		}

		public class Item
		{
			public Panel Panel { get; set; }
			public Label Label { get; set; }
			public ColorBox ColorBox { get; set; }
		}
	}
}
