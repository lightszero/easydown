using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace genhash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitVerAndPath();
            listBoxConsole.Items.Add(ver.ToString());
            foreach(var v in ver.groups)
            {
                listBoxGroup.Items.Add(v.Key);
            }
        }

        Verall ver = null;
        Verall vernew = null;
        void InitVerAndPath()
        {
            ver = Verall.Read("./");
            if(ver==null)
            {
                ver = new Verall();
                string[] groups=              System.IO.Directory.GetDirectories("./");
                foreach(var g in groups)
                {
                    string path = g.Substring(2).ToLower();
                    if (path.IndexOf("path") == 0)
                    {
                        continue;
                    }
                    ver.groups[path] = new VerInfo(path);
                }
                ver.ver = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckVer();
        }
        void FileToTower(string rootp) 
        {
            string[] sDirectories = System.IO.Directory.GetDirectories(rootp);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootp);

            foreach (string path in sDirectories) 
            {

                FileToTower(path);
            }

           System.IO.FileInfo[] files = dir.GetFiles(); // 获取所有文件信息。。
            foreach (FileInfo file in files)
           {
              
               if (file.Name == file.Name.ToLower()) continue;

               string newfilefull=file.Directory+"\\"+file.Name.ToLower();
               listBoxConsole.Items.Add("修改了"+file.Name);
               File.Move(file.FullName, newfilefull);

           }   
        
        }

        void CheckVer()
        {
            vernew = new Verall();
            string[] groups = System.IO.Directory.GetDirectories("./");
            foreach (var g in groups)
            {
                string path = g.Substring(2).ToLower();
                if (path.IndexOf("path") == 0)
                {
                    continue;
                }
                if(ver.groups.ContainsKey(path)==false)
                {
                    listBoxConsole.Items.Add("目录未包含:"+path+ " 如果需要增加，修改allver增加一行");
                }

            }
            int delcount = 0;
            int updatecount = 0;
            int addcount = 0;
            foreach(var g in ver.groups)
            {
                vernew.groups[g.Key] = new VerInfo(g.Key);
                vernew.groups[g.Key].GenHash();
                foreach(var f in g.Value.filehash)
                {
                    if(vernew.groups[g.Key].filehash.ContainsKey(f.Key)==false)
                    {
                        listBoxConsole.Items.Add("文件被删除：" + g.Key+":"+f.Key);
                        delcount++;
                    }
                    else
                    {
                        string hash=    vernew.groups[g.Key].filehash[f.Key];
                        string oldhash = g.Value.filehash[f.Key];
                        if(hash!=oldhash)
                        {
                            listBoxConsole.Items.Add("文件更新：" + g.Key + ":" + f.Key);
                            updatecount++;
                        }
                    }
                }
                foreach(var f in vernew.groups[g.Key].filehash)
                {
                    if(g.Value.filehash.ContainsKey(f.Key)==false)
                    {
                        listBoxConsole.Items.Add("文件增加：" + g.Key + ":" + f.Key);
                        addcount++;
                    }
                }
                
            }
           
            if(addcount==0&&delcount==0&&updatecount==0)
            {
                vernew.ver = ver.ver;
                listBoxConsole.Items.Add("无变化 ver="+vernew.ver);
            }
            else
            {
                vernew.ver = ver.ver + 1;
                listBoxConsole.Items.Add("检查变化结果 add:" + addcount + " remove:" + delcount + " update:" + updatecount);
                listBoxConsole.Items.Add("版本号变为:" + vernew.ver);
            }
            //ver = vernew;
        }
        void GenVer()
        {
            if(vernew==null)
            {
                listBoxConsole.Items.Add("先检查一下版本再生成");
                return;
            }
            else if(vernew.ver==ver.ver)
            {
                listBoxConsole.Items.Add("版本无变化");
                //return;
            }
            vernew.SaveToPath("./");

            listBoxConsole.Items.Add("生成OK Ver:"+vernew.ver);
            ver = vernew;
            listBoxGroup.Items.Clear();
            foreach (var v in ver.groups)
            {
                listBoxGroup.Items.Add(v.Key);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GenVer();
        }

        private void listBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ver.groups.ContainsKey(listBoxGroup.SelectedItem as string))
            {
                var group = ver.groups[listBoxGroup.SelectedItem as string];
                listBoxFiles.Items.Clear();
                foreach(var f in group.filehash)
                {
                    listBoxFiles.Items.Add(f.Key + "|" + f.Value);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBoxConsole.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileToTower("./");
        }
    }
}
