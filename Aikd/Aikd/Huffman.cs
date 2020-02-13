using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Aikd
{
    class Huffman
    {
        private string FileText;
        private readonly string FileName;
        private List<Node> Nodes;
        private List<Node> Tree;
        private List<Node> Code;
        public Huffman(string FileName)
        {
            this.FileName = FileName;
            Tree = new List<Node>();
            Nodes = new List<Node>();
            Code = new List<Node>();
            if(FileOpen())
            {
                Console.WriteLine($"zawartosc pliku:  {FileText}\n\n");
                GenerateNodes();
                Tree = Nodes;
                GenerateTree();
                GenerateCode();

                foreach(var letter in FileText)
                {
                    Console.Write(Code.First(a => a.Char == letter).Code + " ");
                }
            }
        }

        private void GenerateTree()
        {
            while (Tree.Count() != 1)
            {
                Tree = Tree.OrderBy(t => t.Count).ToList();

                Tree[0].Dir = "LEWO";
                Tree[1].Dir = "PRAWO";

                var newNode = new Node(null);
                newNode.Left = Tree[0];
                newNode.Right = Tree[1];
                Tree.Add(newNode);

                Tree.RemoveAt(0);
                Tree.RemoveAt(0);
            }
        }

        private void GenerateCode(Node node = null, string code="")
        {
            if (node == null)
                node = Tree.First();

            if (node.Char != '+')
            {
                node.Code = code;
                Code.Add(node);
            }

            if (node.Left != null)
                GenerateCode(node.Left, code + "0");
            if (node.Right != null)
                GenerateCode(node.Right, code + "1");
        }

        private void GenerateNodes()
        {
            foreach(var letter in FileText)
            {
                if (Nodes.All(a => a.Char != letter))
                    Nodes.Add(new Node(letter));
                else
                    Nodes.First(b => b.Char == letter).CountAdd();
            }
        }

        public Boolean FileOpen()
        {
            string text = "";
            try
            {
                using (var sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line.ToUpper();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"nie znaleziono pliku({e.Message})");
                return false;
            }
            FileText = text;
            return true;
        }
    }
}
