﻿using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ISqlCrud
    {
        Task<List<EventModel>> GetAllEvents();
        Task<List<PlaceModel>> GetAllPlaces();
        Task<PlaceModel> GetPlaceById(int id);
        Task InsertEvent(EventModel model);
        Task<List<EventModel>> GetEventsByUserId(string userId);
        Task<EventModel> GetEventById(int id);
    }
}