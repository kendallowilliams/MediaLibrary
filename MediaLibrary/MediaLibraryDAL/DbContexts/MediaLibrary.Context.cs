﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaLibraryDAL.DbContexts
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MediaLibraryEntities : DbContext
    {
        public MediaLibraryEntities()
            : base("name=MediaLibraryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public virtual DbSet<Podcast> Podcasts { get; set; }
        public virtual DbSet<PodcastItem> PodcastItems { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TrackPath> TrackPaths { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
    
        public virtual int DeleteAllAlbums()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllAlbums");
        }
    
        public virtual int DeleteAllArtists()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllArtists");
        }
    
        public virtual int DeleteAllGenres()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllGenres");
        }
    
        public virtual int DeleteAllPlaylists(Nullable<int> playlist_id)
        {
            var playlist_idParameter = playlist_id.HasValue ?
                new ObjectParameter("playlist_id", playlist_id) :
                new ObjectParameter("playlist_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllPlaylists", playlist_idParameter);
        }
    
        public virtual int DeleteAllPodcasts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllPodcasts");
        }
    
        public virtual int DeleteAllTracks()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteAllTracks");
        }
    
        public virtual int DeletePlaylist(Nullable<int> playlist_id)
        {
            var playlist_idParameter = playlist_id.HasValue ?
                new ObjectParameter("playlist_id", playlist_id) :
                new ObjectParameter("playlist_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeletePlaylist", playlist_idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> DeletePodcast(Nullable<int> podcastId)
        {
            var podcastIdParameter = podcastId.HasValue ?
                new ObjectParameter("PodcastId", podcastId) :
                new ObjectParameter("PodcastId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("DeletePodcast", podcastIdParameter);
        }
    
        public virtual ObjectResult<FindAlbums_Result> FindAlbums(string title, Nullable<int> artist_id, Nullable<int> year, Nullable<int> genre_id)
        {
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var artist_idParameter = artist_id.HasValue ?
                new ObjectParameter("artist_id", artist_id) :
                new ObjectParameter("artist_id", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            var genre_idParameter = genre_id.HasValue ?
                new ObjectParameter("genre_id", genre_id) :
                new ObjectParameter("genre_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindAlbums_Result>("FindAlbums", titleParameter, artist_idParameter, yearParameter, genre_idParameter);
        }
    
        public virtual ObjectResult<FindArtists_Result> FindArtists(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindArtists_Result>("FindArtists", nameParameter);
        }
    
        public virtual ObjectResult<FindGenres_Result> FindGenres(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindGenres_Result>("FindGenres", nameParameter);
        }
    
        public virtual ObjectResult<FindPaths_Result> FindPaths(string location)
        {
            var locationParameter = location != null ?
                new ObjectParameter("location", location) :
                new ObjectParameter("location", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindPaths_Result>("FindPaths", locationParameter);
        }
    
        public virtual ObjectResult<FindPodcastItems_Result> FindPodcastItems(Nullable<int> podcast_id)
        {
            var podcast_idParameter = podcast_id.HasValue ?
                new ObjectParameter("podcast_id", podcast_id) :
                new ObjectParameter("podcast_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindPodcastItems_Result>("FindPodcastItems", podcast_idParameter);
        }
    
        public virtual ObjectResult<FindTracks_Result> FindTracks(Nullable<int> title, Nullable<int> path_id, Nullable<int> artist_id, Nullable<int> album_id, Nullable<int> genre_id, Nullable<int> year)
        {
            var titleParameter = title.HasValue ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(int));
    
            var path_idParameter = path_id.HasValue ?
                new ObjectParameter("path_id", path_id) :
                new ObjectParameter("path_id", typeof(int));
    
            var artist_idParameter = artist_id.HasValue ?
                new ObjectParameter("artist_id", artist_id) :
                new ObjectParameter("artist_id", typeof(int));
    
            var album_idParameter = album_id.HasValue ?
                new ObjectParameter("album_id", album_id) :
                new ObjectParameter("album_id", typeof(int));
    
            var genre_idParameter = genre_id.HasValue ?
                new ObjectParameter("genre_id", genre_id) :
                new ObjectParameter("genre_id", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindTracks_Result>("FindTracks", titleParameter, path_idParameter, artist_idParameter, album_idParameter, genre_idParameter, yearParameter);
        }
    
        public virtual ObjectResult<FindTransactions_Result> FindTransactions(Nullable<int> status, string message, string error_message, string status_message, Nullable<int> type, Nullable<System.DateTime> start_date, Nullable<System.DateTime> end_date)
        {
            var statusParameter = status.HasValue ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(int));
    
            var messageParameter = message != null ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(string));
    
            var error_messageParameter = error_message != null ?
                new ObjectParameter("error_message", error_message) :
                new ObjectParameter("error_message", typeof(string));
    
            var status_messageParameter = status_message != null ?
                new ObjectParameter("status_message", status_message) :
                new ObjectParameter("status_message", typeof(string));
    
            var typeParameter = type.HasValue ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(int));
    
            var start_dateParameter = start_date.HasValue ?
                new ObjectParameter("start_date", start_date) :
                new ObjectParameter("start_date", typeof(System.DateTime));
    
            var end_dateParameter = end_date.HasValue ?
                new ObjectParameter("end_date", end_date) :
                new ObjectParameter("end_date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FindTransactions_Result>("FindTransactions", statusParameter, messageParameter, error_messageParameter, status_messageParameter, typeParameter, start_dateParameter, end_dateParameter);
        }
    
        public virtual ObjectResult<GetTrackFile_Result> GetTrackFile(Nullable<int> track_id)
        {
            var track_idParameter = track_id.HasValue ?
                new ObjectParameter("track_id", track_id) :
                new ObjectParameter("track_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTrackFile_Result>("GetTrackFile", track_idParameter);
        }
    }
}
