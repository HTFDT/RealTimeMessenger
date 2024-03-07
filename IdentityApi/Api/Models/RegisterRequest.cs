﻿namespace Api.Models;

public record RegisterRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}