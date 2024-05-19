using System;
using TMPro;

namespace App.Scripts.General.Ticker
{
    public interface ITimeTicker
    {
        event Action SecondsTicked;
        event Action MinutesTicked;
    }
}