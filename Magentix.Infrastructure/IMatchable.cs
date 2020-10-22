using System;

namespace Magentix.Infrastructure
{
    public interface IMatchable
    {
        bool Matches(object other);
    }
}
