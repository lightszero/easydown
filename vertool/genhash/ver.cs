using System;
using System.Collections.Generic;
using System.Text;


    public class VerInfo
    {
        public VerInfo(string group)
        {
            this.group = group;
        }
        public string group
        {
            get;
            private set;
        }
        public int match = 0;
        public Dictionary<string, string> filehash = new Dictionary<string, string>();

        static System.Security.Cryptography.SHA1CryptoServiceProvider osha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        public void GenHash()
        {
            string [] files=  System.IO.Directory.GetFiles(this.group, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (var f in files)
            {
                if (f.IndexOf(".crc.txt") >= 0
                    ||
                    f.IndexOf(".meta") >= 0
                    ||
                    f.IndexOf(".db") >= 0
                    ) continue;
                 GenHashOne(f);
            }
        }
        public void GenHashOne(string filename)
        {
            using (System.IO.Stream s = System.IO.File.OpenRead(filename))
            {
                
                var hash = osha1.ComputeHash(s);
                var shash = Convert.ToBase64String(hash) +"@"+s.Length;
                filename = filename.Substring(group.Length + 1);

                filename = filename.Replace('\\', '/');
                filehash[filename] = shash;
            }
        }
        public string SaveToPath(int ver, string path)
        {
            string outstr = "Ver:"+ver+"|FileCount:" + this.filehash.Count + "\n";
            foreach(var f in filehash)
            {
                outstr += f.Key + "|" + f.Value + "\n";
            }
            string g = this.group.Replace('/', '_');
            string outfile = System.IO.Path.Combine(path, g + ".ver.txt");
            System.IO.File.WriteAllText(outfile, outstr, Encoding.UTF8);
            using (System.IO.Stream s = System.IO.File.OpenRead(outfile))
            {
                var hash = osha1.ComputeHash(s);
                var shash = Convert.ToBase64String(hash);
                return shash;
            }
        }
        public bool Read(int ver,string hash,int filecount,string path)
        {
            string g = this.group.Replace('/', '_');
            string file = System.IO.Path.Combine(path, g + ".ver.txt");
            if (System.IO.File.Exists(file) == false) return false;
            using (System.IO.Stream s = System.IO.File.OpenRead(file))
            {
                var rhash = osha1.ComputeHash(s);
                var shash = Convert.ToBase64String(rhash);
                if (shash != hash) return false;//Hash 不匹配
            }
            string txt =System.IO.File.ReadAllText(file, Encoding.UTF8);
            string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var l in lines)
            {
                if (l.IndexOf("Ver:") == 0)
                {
                    var sp=   l.Split(new string[] { "Ver:", "|FileCount:" }, StringSplitOptions.RemoveEmptyEntries);
                    int mver = int.Parse(sp[0]);
                    int mcount = int.Parse(sp[1]);
                    if (ver != mver) return false;
                    if (mcount != filecount) return false;
                }
                else
                {
                    var sp = l.Split('|');
                    filehash[sp[0]] = sp[1];
                }
            }
            return true;
        }
    }

    public class Verall
    {
        public int ver;//版本号
        public Dictionary<string, VerInfo> groups = new Dictionary<string, VerInfo>();

        public override string ToString()
        {
            int useful = 0;
            int filecount = 0;
            int filematch=0;
            foreach(var i in groups)
            {
                if (i.Value.match>0) useful++;
                filematch += i.Value.match;
                filecount+=i.Value.filehash.Count;
            }
            return "ver=" + ver + " group=(" + useful + "/" + groups.Count + ") file=("+filematch+"/"+filecount+")";
        }

        public static Verall Read(string path)
        {
            if(System.IO.File.Exists(System.IO.Path.Combine(path, "allver.ver.txt"))==false)
            {
                return null;
            }
            string txt = System.IO.File.ReadAllText(System.IO.Path.Combine(path, "allver.ver.txt"),Encoding.UTF8);
            string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            Verall var = new Verall();
            foreach(var l in lines)
            {
                if(l.IndexOf("Ver:")==0)
                {
                    var.ver = int.Parse(l.Substring(4));
                }
                else
                {
                    var sp= l.Split('|');
                    var.groups[sp[0]] = new VerInfo(sp[0]);
                    var.groups[sp[0]].Read(var.ver,sp[1],int.Parse(sp[2]),path);
                }
            }
            return var;   
        }
        public void SaveToPath(string path)
        {
            Dictionary<string, string> grouphash = new Dictionary<string, string>();
            foreach(var i in groups.Values)
            {
                grouphash[i.group] = i.SaveToPath(this.ver,path);
            }

            string outstr = "Ver:" + this.ver + "\n";
            foreach(var g in grouphash)
            {
                outstr +=g.Key + "|" + g.Value + "|" + groups[g.Key].filehash.Count+"\n";
            }
            System.IO.File.WriteAllText(System.IO.Path.Combine(path, "allver.ver.txt"), outstr,Encoding.UTF8);
        }
  

    }

