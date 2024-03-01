﻿using System.Collections.Generic;

namespace LeitorNFe.App.Infrastructure.Wrapper;

public interface IResult
{
    List<string> Messages { get; set; }

    bool Succeeded { get; set; }
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}