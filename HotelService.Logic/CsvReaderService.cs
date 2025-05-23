﻿using System.Globalization;
using CsvHelper;
using HotelService.Db;
using HotelService.Db.DTOs;

namespace HotelService.Logic;

public class CsvReaderService
{
    private readonly string _filePath =
        "/Users/asyayakimova/RiderProjects/HotelService/HotelService.Logic/extra_data.csv";
    public async Task<(List<HotelDto> Hotels, int TotalCount)> GetHotelsPaginatedAsync(int pageNumber, int pageSize)
    {
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var hotels = new List<HotelDto>();
        int totalCount = 0;
        int skipCount = (pageNumber - 1) * pageSize;
        
        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            totalCount++;
            
            if (totalCount <= skipCount) continue;

            if (hotels.Count >= pageSize) break;

            var hotel = new HotelDto
            {
                Id = csv.TryGetField<int>("id", out var id) ? id : 0,
                Name = csv.TryGetField<string>("name", out var name) ? name : "Unknown",
                Description = csv.TryGetField<string>("description", out var desc) ? desc : "No description",
                ThumbnailUrl = csv.TryGetField<string>("thumbnail_url", out var url) ? url : "",
                LogPrice = csv.TryGetField<double>("log_price", out var price) ? price : 0
            };
            hotels.Add(hotel);
        }

        return await Task.FromResult((hotels, totalCount));
    }

    public async Task<HotelDto?> GetHotelsByIdAsync(int hotelId)
    {
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            if (csv.GetField<int>("id") == hotelId)
            {
                var hotel = new HotelDto
                {
                    Id = csv.TryGetField<int>("id", out var id) ? id : 0,
                    Name = csv.TryGetField<string>("name", out var name) ? name : "Unknown",
                    Description = csv.TryGetField<string>("description", out var desc) ? desc : "No description",
                    ThumbnailUrl = csv.TryGetField<string>("thumbnail_url", out var url) ? url : "",
                    LogPrice = csv.TryGetField<double>("log_price", out var price) ? price : 0
                };
                return await Task.FromResult(hotel);
            }
           
        }
        return null;
    }
}