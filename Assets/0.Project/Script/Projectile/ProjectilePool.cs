using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PG.Battle
{

  public class ProjectilePool<T> : IObjectPoolSW<T> where T : class
  {
    internal readonly Stack<T> m_Stack;
    private readonly Func<int,T> m_CreateFunc;
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;
    private readonly Action<T> m_ActionOnDestroy;
    private readonly int m_MaxSize;
    private readonly int m_id;
   
    internal bool m_CollectionCheck;

    public int CountAll { get; private set; }

    public int CountLeft => this.CountAll - this.CountInactive;
    public int CountInactive => this.m_Stack.Count;

    public ProjectilePool(
      Func<int,T> createFunc,
      Action<T> actionOnGet = null,
      Action<T> actionOnRelease = null,
      Action<T> actionOnDestroy = null,
      bool collectionCheck = true,
      int id = 0,
      int maxSize = 10000)
    {
      if (createFunc == null)
        throw new ArgumentNullException(nameof (createFunc));
      if (maxSize <= 0)
        throw new ArgumentException("Max Size must be greater than 0", nameof (maxSize));
      this.m_Stack = new Stack<T>(0);
      this.m_CreateFunc = createFunc;
      this.m_MaxSize = maxSize;
      this.m_id = id;
      this.m_ActionOnGet = actionOnGet;
      this.m_ActionOnRelease = actionOnRelease;
      this.m_ActionOnDestroy = actionOnDestroy;
      this.m_CollectionCheck = collectionCheck;
    }


    public void FillStack()
    {
      T obj;
      if (m_Stack.Count < m_MaxSize)
      {
        obj = this.m_CreateFunc(m_id);
        ++this.CountAll;
        this.m_Stack.Push(obj);
      }else
        throw new ArgumentException("Even though it reached Max Size it Try to fill");

    }

    public T PickUp()
    {
      T obj;
      if (this.m_Stack.Count == 0)
      {
        obj = this.m_CreateFunc(m_id);
        ++this.CountAll;
      }
      else
        obj = this.m_Stack.Pop();
      Action<T> actionOnGet = this.m_ActionOnGet;
      if (actionOnGet != null)
        actionOnGet(obj);
      return obj;
    }

    /// <summary>
    /// PooledObject는 봉인 할꺼임.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public PooledObject<T> Get(out T v)
    {
      v = null;
      return new PooledObject<T>();
    }

    public void SetBack(T element)
    {
      if (this.m_CollectionCheck && this.m_Stack.Count > 0 && this.m_Stack.Contains(element))
        throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
      Action<T> actionOnRelease = this.m_ActionOnRelease;
      if (actionOnRelease != null)
        actionOnRelease(element);
      if (this.CountInactive < this.m_MaxSize)
      {
        this.m_Stack.Push(element);
      }
      else
      {
        Action<T> actionOnDestroy = this.m_ActionOnDestroy;
        if (actionOnDestroy != null)
          actionOnDestroy(element);
      }
    }

    public void Clear()
    {
      if (this.m_ActionOnDestroy != null)
      {
        foreach (T obj in this.m_Stack)
          this.m_ActionOnDestroy(obj);
      }
      this.m_Stack.Clear();
      this.CountAll = 0;
    }

  }
}
