using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class resdown : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        List<string> wantdownGroup = new List<string>();
        wantdownGroup.Add("test1");
        wantdownGroup.Add("test1_ios");
        ResmgrNative.Instance.BeginInit("http://lightszero.github.io/publish/", OnInitFinish, wantdownGroup);
        strState = "检查资源";
    }
    bool indown = false;
    void OnInitFinish(System.Exception err)
    {
        if (err == null)
        {
            ResmgrNative.Instance.taskState.Clear();
            strState = "检查资源完成";
            List<string> wantdownGroup = new List<string>();
            wantdownGroup.Add("test1");
            wantdownGroup.Add("test1_ios");
            var downlist = ResmgrNative.Instance.GetNeedDownloadRes(wantdownGroup);
            foreach (var d in downlist)
            {
                d.Download(null);
            }
            ResmgrNative.Instance.WaitForTaskFinish(DownLoadFinish);
            indown = true;
        }
        else
            strState = null;
    }
    void DownLoadFinish()
    {
        indown = false;
        strState = "更新完成";
        foreach (var file in ResmgrNative.Instance.verLocal.groups["test1_ios"].listfiles.Values)
        {
            if(file.FileName.Contains(".jpg"))
            {
                file.BeginLoadTexture2D((tex, tag) =>
                    {
                        loadedTexs.Add(tex);
                    });
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        ResmgrNative.Instance.Update();
        if (indown)
        {
            strState = "have downloaded:" + ResmgrNative.Instance.taskState.downloadcount + " /Total " + ResmgrNative.Instance.taskState.taskcount;
        }

    }
    string strState = "";
    List<Texture2D> loadedTexs = new List<Texture2D>();
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 100), strState);
        for(int i=0;i<loadedTexs.Count;i++)
        {
            GUI.DrawTexture(new Rect(0, 50 + i * 50, 50, 50), loadedTexs[i]);
        }
    }
}
