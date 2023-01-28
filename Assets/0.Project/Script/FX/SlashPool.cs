using System;
using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;


  public class SlashPool<T> : IObjectPoolSW<T> where T : class
  {
    internal readonly Queue<T> m_Queue;
    private readonly Func<T> m_CreateFunc;
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;
    private readonly Action<T> m_ActionOnDestroy;
    private readonly int m_MaxSize;
   
    internal bool m_CollectionCheck;

    public int CountAll { get; private set; }

    public int CountLeft => this.CountAll - this.CountInactive;
    public int CountInactive => this.m_Queue.Count;

    public SlashPool(
      Func<T> createFunc,
      Action<T> actionOnGet = null,
      Action<T> actionOnRelease = null,
      Action<T> actionOnDestroy = null,
      bool collectionCheck = true,
      int maxSize = 15)
    {
      if (createFunc == null)
        throw new ArgumentNullException(nameof (createFunc));
      if (maxSize <= 0)
        throw new ArgumentException("Max Size must be greater than 0", nameof (maxSize));
      this.m_Queue = new Queue<T>(0);
      this.m_CreateFunc = createFunc;
      this.m_MaxSize = maxSize;
      this.m_ActionOnGet = actionOnGet;
      this.m_ActionOnRelease = actionOnRelease;
      this.m_ActionOnDestroy = actionOnDestroy;
      this.m_CollectionCheck = collectionCheck;
    }


    public void FillStack()
    {
      T obj;
      if (m_Queue.Count < m_MaxSize)
      {
        obj = this.m_CreateFunc();
        ++this.CountAll;
      }else
        throw new ArgumentException("Even though it reached Max Size it Try to fill");

    }

    public T PickUp()
    {
      T obj;
      if (this.m_Queue.Count == 0)
      {
        obj = this.m_CreateFunc();
        ++this.CountAll;
      }
      else
        obj = this.m_Queue.Dequeue();
      Action<T> actionOnGet = this.m_ActionOnGet;
      if (actionOnGet != null)
        actionOnGet(obj);
      return obj;
    }



    public void SetBack(T element)
    {
      if (this.m_CollectionCheck && this.m_Queue.Count > 0 && this.m_Queue.Contains(element))
        throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
      Action<T> actionOnRelease = this.m_ActionOnRelease;
      if (actionOnRelease != null)
        actionOnRelease(element);
      if (this.CountInactive < this.m_MaxSize)
      {
        this.m_Queue.Enqueue(element);
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
        foreach (T obj in this.m_Queue)
          this.m_ActionOnDestroy(obj);
      }
      this.m_Queue.Clear();
      this.CountAll = 0;
    }

  }


