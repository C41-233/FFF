﻿namespace FFF.Base.Util.Generator
{

    public interface IULongGenerator : IValueGenerator<ulong>
    {
    }

    public class SequenceULongGenerator : IULongGenerator
    {

        private ulong current;

        public SequenceULongGenerator(ulong init = 0)
        {
            this.current = init;
        }

        public ulong NextValue()
        {
            return current++;
        }

    }
}