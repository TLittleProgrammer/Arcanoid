using System;

namespace App.Scripts.General.Time
{
    public interface ITimeTicker
    {
        event Action SecondsTicked;
    }
}