﻿namespace Interface
{
    public class Hello
    {
        public Hello(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
