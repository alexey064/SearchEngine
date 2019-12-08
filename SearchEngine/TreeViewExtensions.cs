using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    static class TreeViewExtensions
    {
        public static List<string> GetAll(this TreeView treeView)
        {
            TreeNodeCollection lis = treeView.Nodes;
            if (lis.Count == 0)
            {
                return new List<string>();
            }
            List<string> Fullnode = new List<string>();
            foreach (TreeNode Node in lis)
            {
                Get(Node);
            }

            void Get(TreeNode nodes)
            {
                foreach (TreeNode node in nodes.Nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        Get(node);
                    }
                    else Fullnode.Add(node.FullPath);
                }
            }

            return Fullnode;
        }
        public static bool add(this TreeView treeView, string FullPath)
        {
            string[] temp = FullPath.Split('\\');
            string Path;
            TreeNodeCollection treeNode = treeView.Nodes;
            Path = temp[0];
            if (treeNode.Count == 0)
                treeNode.Add(Path);
            foreach (TreeNode tn in treeNode)
            {
                TreeCreate(tn, 1);
            }
            void TreeCreate(TreeNode treeNode1, int j)
            {
                string path = "";
                path += temp[j];

                if (treeNode1.Nodes.Count != 0)
                    foreach (TreeNode node in treeNode1.Nodes)
                    {
                        if (node.Text == path)
                        {
                            j++;
                            TreeCreate(node, j);
                            return;
                        }
                    }
                var nod = treeNode1.Nodes.Add(path);
                j++;
                if (j > temp.Length - 1)
                {
                    return;
                }
                else { TreeCreate(nod, j); }
            }
            return true;
        }
    }
}
