﻿using System.Data;
using System.Data.SqlClient;
using Dapper;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public partial class SqlCrud
    {
        private async Task<List<EventModel>> ExecuteEventCrudSql(string sql, object param = null)
        {
            using IDbConnection conn = new SqlConnection(_db.ConnectionString);
            return (await conn.QueryAsync<EventModel, PlaceModel, CityModel, EventModel>(
                sql,
                (ev, place, city) => { place.City = city; ev.Place = place; return ev; },
                param: param ?? "",
                splitOn: "PlaceId,CityId",
                commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<List<EventModel>> GetAllEvents()
        {
            return await ExecuteEventCrudSql("dbo.spEvent_GetAll");
        }
        public async Task<EventModel> GetEventById(int id)
        {
            return (await ExecuteEventCrudSql("dbo.spEvent_GetById", new { Id = id }))?.FirstOrDefault();
        }

        public async Task<List<EventModel>> GetEventsByUserId(string userId)
        {
            return await ExecuteEventCrudSql("dbo.spEvent_GetAllCreatedByUserId", new { CreatedByUserId = userId });
        }

        public async Task InsertEvent(EventModel model)
        {
            var p = new
            {
                Title = model.Title,
                Description = model.Description,
                PlaceId = model.Place.Id,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                EntranceFee = model.EntranceFee,
                CreatedByUserId = model.CreatedByUserId,
                DateCreated = model.DateCreated,
                Url = model.Url,
                AllowRequests = 1
            };
            await _db.SaveData("dbo.spEvent_Insert", p, true);
        }
    }
}