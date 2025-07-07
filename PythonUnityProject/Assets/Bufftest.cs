using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Profiling;
using UnityEngine;

class Buff
{
    private int pos;
    private byte[] buff;

    public Buff(int size)
    {
        this.pos = 0;
        this.buff = new byte[size];
    }

    public int CanWriteSize()
    {
        return this.buff.Length - this.pos;
    }

    public void Write(byte[] bytes, int size)
    {
        if (size > this.CanWriteSize())
        {
            throw new Exception("write bytes too long.");
        }
        Array.Copy(bytes, 0, this.buff, this.pos, size);
        this.pos += size;
    }

    public void SetPos(int pos)
    {
        this.pos = pos;
    }

    public int GetPos()
    {
        return this.pos;
    }

    public int GetSize()
    {
        return this.buff.Length;
    }

    public byte[] GetBytes()
    {
        return this.buff;
    }
}
class DoubleBuff
{
    private Buff read_buff;
    private Buff write_buff;

    public DoubleBuff(int size, int max_chunk_count)
    {
        this.read_buff = new Buff(size);
        this.write_buff = new Buff(size);
    }
}


public class Bufftest : MonoBehaviour
{
    ProfilerRecorder totalReservedMemoryRecorder;
    ProfilerRecorder gcReservedMemoryRecorder;
    ProfilerRecorder systemUsedMemoryRecorder;
    // Start is called before the first frame update
    void Update()
    {
       // Debug.Log(totalReservedMemoryRecorder.LastValue + "  ----" + gcReservedMemoryRecorder.LastValue + "  ----" + systemUsedMemoryRecorder.LastValue);

        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");


        Debug.Log(totalReservedMemoryRecorder.LastValue + "  --2--" + gcReservedMemoryRecorder.LastValue + "  --2--" + systemUsedMemoryRecorder.LastValue);

        DoubleBuff mDoubleBuff = new DoubleBuff(1000000, 1000000);
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");

        Buff read_buff = new Buff(1000000);
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");

        Debug.Log(totalReservedMemoryRecorder.LastValue + "  --3--" + gcReservedMemoryRecorder.LastValue + "  --3--" + systemUsedMemoryRecorder.LastValue);
    }

}
