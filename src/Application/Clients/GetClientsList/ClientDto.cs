﻿namespace Application.Clients.GetClientsList;

public class ClientDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string CountryCode { get; init; }
}