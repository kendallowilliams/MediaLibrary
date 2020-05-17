using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MediaLibraryDAL.DbContexts
{
    public partial class MediaLibraryEntities : DbContext
    {
        public MediaLibraryEntities()
        {
        }

        public MediaLibraryEntities(DbContextOptions<MediaLibraryEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PlaylistEpisode> PlaylistEpisodes { get; set; }
        public virtual DbSet<PlaylistPodcastItem> PlaylistPodcastItems { get; set; }
        public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public virtual DbSet<Podcast> Podcasts { get; set; }
        public virtual DbSet<PodcastItem> PodcastItems { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<TrackPath> TrackPaths { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=kserver;Initial Catalog=MediaLibrary_DEBUG;User ID=kserver_sql_debug;Password=kserver_sql_debug;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Album__3214EC0697125AA8")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Album__ArtistId__6EF57B66");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Album__GenreId__6FE99F9F");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Artist__3214EC06B29AD2A8")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JsonData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(d => d.SeriesId)
                    .HasConstraintName("FK__Episode__SeriesI__3D2915A8");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Genre__3214EC06FC4C8FFE")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tmp_ms_x__3214EC06CFD67FC4")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlaylistEpisode>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Playlist__3214EC06A012CD92")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.PlaylistEpisodes)
                    .HasForeignKey(d => d.EpisodeId)
                    .HasConstraintName("FK_playlist_episode_episode");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistEpisodes)
                    .HasForeignKey(d => d.PlaylistId)
                    .HasConstraintName("FK_playlist_episode_playlist");
            });

            modelBuilder.Entity<PlaylistPodcastItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Playlist__3214EC061EA4E1A6")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistPodcastItems)
                    .HasForeignKey(d => d.PlaylistId)
                    .HasConstraintName("FK_playlist_podcastitem_playlist");

                entity.HasOne(d => d.PodcastItem)
                    .WithMany(p => p.PlaylistPodcastItems)
                    .HasForeignKey(d => d.PodcastItemId)
                    .HasConstraintName("FK_playlist_podcastitem_podcastitem");
            });

            modelBuilder.Entity<PlaylistTrack>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Playlist__3214EC06F2D42FC0")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistTracks)
                    .HasForeignKey(d => d.PlaylistId)
                    .HasConstraintName("FK_playlist_track_playlist");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.PlaylistTracks)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK_playlist_track_track");
            });

            modelBuilder.Entity<Podcast>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Podcast__3214EC06FA7C7167")
                    .IsClustered(false);

                entity.Property(e => e.Author).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PodcastItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tmp_ms_x__3214EC06D2DF9777")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.File).IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Podcast)
                    .WithMany(p => p.PodcastItems)
                    .HasForeignKey(d => d.PodcastId)
                    .HasConstraintName("FK__PodcastIt__Podca__3E1D39E1");
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Series__3214EC067621B848")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tmp_ms_x__3214EC0631FB28BF")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Duration).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Track__AlbumId__2CF2ADDF");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Track__ArtistId__2DE6D218");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Track__GenreId__2FCF1A8A");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.PathId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Track__PathId__2EDAF651");
            });

            modelBuilder.Entity<TrackPath>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__TrackPat__3214EC0695DF2C8D")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastScanDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Transact__3214EC068D74D4EC")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
