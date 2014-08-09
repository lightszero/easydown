class RefCountObj<T> where T : UnityEngine.Object
{
    public RefCountObj(T obj)
    {
        this.obj = obj;
        this.RefCount = 0;
    }
    public T obj
    {
        get;
        private set;
    }
    public int RefCount
    {
        get;
        private set;
    }
    public void AddRef()
    {
        RefCount++;
        //Debug.Log("AddRef:" + obj.name + "(" + RefCount + ")");
    }
    public int DecRef()
    {
        RefCount--;
        if (RefCount < 0) RefCount = 0;
        //Debug.Log("DecRef:" + obj.name + "(" + RefCount + ")");
        if (RefCount == 0)
        {

            UnityEngine.Object.Destroy(obj);
            this.obj = null;
        }
        return RefCount;
    }

}

class RefCountObjImmediate<T> where T : UnityEngine.Object
{
    public RefCountObjImmediate(T obj)
    {
        this.obj = obj;
        this.RefCount = 0;
    }
    public T obj
    {
        get;
        private set;
    }
    public int RefCount
    {
        get;
        private set;
    }
    public int AddRef()
    {
        RefCount++;
        //Debug.Log("AddRef:" + obj.name + "(" + RefCount + ")");
        return RefCount;
    }
    public int DecRef()
    {
        RefCount--;
        if (RefCount < 0) RefCount = 0;
        //Debug.Log("DecRef:" + obj.name + "(" + RefCount + ")");
        if (RefCount == 0)
        {

            UnityEngine.Object.DestroyImmediate(obj,true);
            this.obj = null;
        }
        return RefCount;
    }

}
