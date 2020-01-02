﻿using System.Threading.Tasks;

namespace IAS04110
{
    public interface ILogger
    {
        event Log LogEvent;
        void SetFile(string name);
        bool IsFileSet { get; }

        Task Listen();
    }
}
