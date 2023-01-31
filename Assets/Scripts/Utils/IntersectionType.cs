using System;

[Flags]
public enum IntersectionType
{
    Not = 1,
    Does = 2,
    Continuous = 4,
    Once = 8
}