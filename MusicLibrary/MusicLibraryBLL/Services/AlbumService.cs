﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IAlbumService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlbumService : IAlbumService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public AlbumService()
        { }

        public async Task<int> AddAlbum(string title, uint year, int artistId, int genreId)
        {
            string existsQuery = $"SELECT id FROM album WHERE title = @title";
            int id = await dataService.ExecuteScalar<int>(existsQuery, new { title });
            Album album = new Album(title, artistId, genreId, year);

            if (id == 0)
            {
                id = await dataService.Insert<Album, int>(album);
            }

            return id;
        }

        public async Task<IEnumerable<Album>> GetAlbums() => await dataService.GetList<Album>();

        public async Task<Album> GetAlbum(object id) =>  await dataService.Get<Album>(id);

        public async Task<int> InsertAlbum(Album album) => await dataService.Insert<Album,int>(album);

        public async Task<bool> DeleteAlbum(int id) => await dataService.Delete<Album>(id);

        public async Task<bool> DeleteAlbum(Album album) => await dataService.Delete(album);

        public async Task<bool> UpdateAlbum(Album album) => await dataService.Update(album);
    }
}