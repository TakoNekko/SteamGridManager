using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	using Extensions.Control;
	using Options.Panels;
	using System.Drawing;

	public partial class OptionsDialog : Form
	{
		#region Fields

		private readonly Lazy<Font> boldFont = new Lazy<Font>(() => new Font(TreeView.DefaultFont, FontStyle.Bold));

		#endregion

		#region Constructors

		public OptionsDialog()
		{
			InitializeComponent();

			if (!DesignMode)
			{
				InitializeTreeView();
				this.ApplyWindowTheme();

				categoryTreeView.BeginUpdate();
				HighlightParentNodesRecursive(categoryTreeView.Nodes);
				HideDeveloperNodes();
				categoryTreeView.EndUpdate();
			}
		}

		#endregion

		#region Event Handlers

		#region Form

		private void OptionsDialog_Load(object sender, EventArgs e)
		{
			categoryTreeView.BeginUpdate();
			ReapplyParentNodesTextRecursive(categoryTreeView.Nodes);
			categoryTreeView.EndUpdate();
		}

		#endregion

		#region Category Tree View

		private void categoryTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void categoryTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			var child = e.Node.FirstNode;

			if (child != null)
			{
				//e.Cancel = true;
				//categoryTreeView.SelectedNode = child;
			}
		}

		private void categoryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is null)
			{
				return;
			}

			var node = GetFirstNodeWithValidTag(e.Node) ?? e.Node;

			if (node == activeNode)
			{
				return;
			}

			if (node.Tag is string panelName)
			{
				var type = GetOptionsPanelTypeByName(panelName);
				var panel = (UserControl)Activator.CreateInstance(type);
				
				panel.MinimumSize = System.Drawing.Size.Empty;
				panel.AutoScroll = true;
				panel.Dock = DockStyle.Fill;
				panel.ApplyWindowTheme();

				mainPanel.SuspendLayout();
				mainPanel.Controls.Clear();
				mainPanel.Controls.Add(panel);
				mainPanel.ResumeLayout(performLayout: true);

				var path = node.Text;

				for (var subNode = node.Parent; subNode != null; subNode = subNode.Parent)
				{
					path = $"{subNode.Text} / {path}";
				}

				Text = $"{path} - {"Options"}";

				activeNode = node;
			}
		}

		#endregion

		#endregion

		#region Implementation

		private void InitializeTreeView()
		{
			categoryTreeView.ExpandAll();
			categoryTreeView.SelectedNode = categoryTreeView.Nodes[0].Nodes[0];
		}

		private void HighlightParentNodesRecursive(TreeNodeCollection nodes)
		{
			if (nodes is null || nodes.Count == 0)
			{
				return;
			}

			foreach (TreeNode node in nodes)
			{
				if (node.GetNodeCount(includeSubTrees: false) == 0)
				{
					continue;
				}

				node.NodeFont = boldFont.Value;
				node.ForeColor = Color.Teal;

				HighlightParentNodesRecursive(node.Nodes);
			}
		}

		private void ReapplyParentNodesTextRecursive(TreeNodeCollection nodes)
		{
			if (nodes is null || nodes.Count == 0)
			{
				return;
			}

			foreach (TreeNode node in nodes)
			{
				if (node.GetNodeCount(includeSubTrees: false) == 0)
				{
					continue;
				}

				// HACK: changing a treenode's Font doesn't recompute its label bounds.
				var text = node.Text;
				node.Text = "";
				node.Text = text;

				ReapplyParentNodesTextRecursive(node.Nodes);
			}
		}

		private void HideDeveloperNodes()
		{
			var keys = new string[]
			{
				// NOTE: would require some refactoring.
				"Environment/Database",
				// NOTE: no options necessary so far.
				"Interface/Dialog/Property",
				// TODO: user-defined enums not implemented yet.
				"Interface/VdfDefinition/Enum",
			};

			foreach (var key in keys)
			{
				var nodes = categoryTreeView.Nodes.Find(key, searchAllChildren: true);

				if (nodes is null
					|| nodes.Length == 0)
				{
					continue;
				}

				categoryTreeView.Nodes.Remove(nodes[0]);
			}
		}

		private TreeNode GetFirstNodeWithValidTag(TreeNode baseNode)
		{
			foreach (TreeNode node in baseNode.Nodes)
			{
				if (node.Tag is null || string.IsNullOrEmpty((string)node.Tag))
				{
					if (node.GetNodeCount(includeSubTrees: false) > 0)
					{
						var childNode = GetFirstNodeWithValidTag(node);

						if (childNode != null)
						{
							return childNode;
						}
					}

					continue;
				}

				return node;
			}

			return null;
		}

		private static Type GetOptionsPanelTypeByName(string panelName)
		{
			if (!optionsPanelTypeByNames.TryGetValue(panelName, out var type))
			{
				throw new ArgumentOutOfRangeException(nameof(panelName));
			}

			return type;
		}

		#endregion

		#region Fields

		private TreeNode activeNode;

		private static readonly Dictionary<string, Type> optionsPanelTypeByNames = new Dictionary<string, Type>
		{
			{ "Environment/General", typeof(EnvironmentGeneralOptionsPanel) },
			{ "Environment/Paths", typeof(EnvironmentPathsOptionsPanel) },
			{ "Environment/Database", typeof(EnvironmentDatabaseOptionsPanel) },
			{ "Integration/ExternalApplications", typeof(IntegrationExternalApplicationsOptionsPanel) },
			{ "Interface/List/General", typeof(InterfaceListGeneralOptionsPanel) },
			{ "Interface/List/Filter", typeof(InterfaceListFilterOptionsPanel) },
			{ "Interface/List/Columns", typeof(InterfaceListColumnsOptionsPanel) },
			{ "Interface/Details/General", typeof(InterfaceDetailsGeneralOptionsPanel) },
			{ "Interface/Details/Preview", typeof(InterfaceDetailsPreviewOptionsPanel) },
			{ "Interface/Dialog/Prompts", typeof(InterfaceDialogPromptsOptionsPanel) },
			{ "Interface/Dialog/Properties", typeof(InterfaceDialogPropertiesOptionsPanel) },
			{ "Interface/Dialog/Property", typeof(InterfaceDialogPropertyOptionsPanel) },
			{ "Interface/VdfDefinition/General", typeof(InterfaceVdfDefinitionGeneralOptionsPanel) },
			{ "Interface/VdfDefinition/Object", typeof(InterfaceVdfDefinitionObjectOptionsPanel) },
			{ "Interface/VdfDefinition/String", typeof(InterfaceVdfDefinitionStringOptionsPanel) },
			{ "Interface/VdfDefinition/UInt", typeof(InterfaceVdfDefinitionUIntOptionsPanel) },
			{ "Interface/VdfDefinition/Boolean", typeof(InterfaceVdfDefinitionBooleanOptionsPanel) },
			{ "Interface/VdfDefinition/RelativeTime", typeof(InterfaceVdfDefinitionRelativeTimeOptionsPanel) },
			{ "Interface/VdfDefinition/Enum", typeof(InterfaceVdfDefinitionEnumerationOptionsPanel) },
		};

		#endregion
	}
}
