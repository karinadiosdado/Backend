using System;
using backend.DTOs;

namespace backend.services;

public interface IPostService
{
    public Task<IEnumerable<PostDto>> Get();
}