using UnityEngine;

public interface IMob 
{
    public int Steps { get; }
    public void MakeStep();
}
