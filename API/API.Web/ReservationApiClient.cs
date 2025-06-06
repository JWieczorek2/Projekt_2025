﻿using System.Net.Http.Json;

public class ReservationApiClient
{
    private readonly HttpClient _http;

    public ReservationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ZarezerwujSaleAsync(ReservationRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/rezerwacje/rezerwuj", request);
        return response.IsSuccessStatusCode;
    }
}
