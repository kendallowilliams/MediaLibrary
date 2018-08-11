﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITrackService))]
    public class TrackService : ITrackService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public TrackService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Track>> GetTracks() => await dataService.GetList<Track>();

        public async Task<Track> GetTrack(object id) => await dataService.Get<Track>(id);

        public async Task<int> InsertTrack(Track track) => await dataService.Insert<Track,int>(track);

        public async Task<bool> DeleteTrack(int id) => await dataService.Delete<Track>(id);

        public async Task<bool> DeleteTrack(Track track) => await dataService.Delete(track);

        public async Task DeleteAllTracks()
        {
            await dataService.Execute(@"DELETE track;");
            await dataService.Execute(@"DELETE track_file;");
        }

        public async Task<bool> UpdateTrack(Track track) => await dataService.Update(track);

        public async Task<int?> AddPath(string location)
        {

            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(location))
            {
                string existsQuery = $"SELECT id FROM path WHERE location = @location";
                TrackPath path = new TrackPath(location);

                id = await dataService.ExecuteScalar<int?>(existsQuery, new { location });

                if (!id.HasValue)
                {
                    id = await dataService.Insert<TrackPath, int>(path);
                }
            }

            return id;
        }

        public async Task<int?> AddTrackFile(int trackId)
        {
            Track track = await GetTrack(trackId);
            TrackPath path = await dataService.Get<TrackPath>(track.PathId);
            string filePath = Path.Combine(path.Location, track.FileName);
            byte[] data = File.ReadAllBytes(filePath);
            TrackFile file = new TrackFile(data, MimeMapping.GetMimeMapping(track.FileName));

            track.FileId = await dataService.Insert<TrackFile, int>(file);
            await UpdateTrack(track);

            return track.FileId;
        }

        public async Task<TrackFile> GetTrackFile(int id)
        {
            Track track = await GetTrack(id);
            TrackFile file = null;

            if (track.FileId.HasValue)
            {
                file = await dataService.Get<TrackFile>(id);
            }
            else
            {
                TrackPath path = await dataService.Get<TrackPath>(track.PathId);
                string fileName = Path.Combine(path.Location, track.FileName);
                byte[] data = File.ReadAllBytes(fileName);

                file = new TrackFile { Data = data, Type = MimeMapping.GetMimeMapping(track.FileName) };
            }

            return file;
        }
    }
}