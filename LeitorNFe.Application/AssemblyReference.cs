﻿using System.Reflection;

namespace LeitorNFe.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}