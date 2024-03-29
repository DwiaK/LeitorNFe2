﻿using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
