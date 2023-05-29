using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicApp.Domain.Common.Entities;
namespace MusicApp.Infrastructure.DBContext;

public partial class MusicContext : DbContext
{
    public MusicContext()
    {
    }

    public MusicContext(DbContextOptions<MusicContext> options)
        : base(options)
    {
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial " +
                              "Catalog=Music;Integrated Security=True;TrustServerCertificate=True");
    }

    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<UserEvent> UserEvent { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Genre> Gernes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.Property(e => e.Id).HasMaxLength(100);
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.Id).HasMaxLength(100);

            entity.HasMany(d => d.Albums).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtistAlbum_Album"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtistAlbum_Artist"),
                    j =>
                    {
                        j.HasKey("ArtistId", "AlbumId");
                        j.ToTable("ArtistAlbum");
                        j.IndexerProperty<string>("ArtistId").HasMaxLength(100);
                        j.IndexerProperty<string>("AlbumId").HasMaxLength(100);
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Gerne");
            entity.Property(e => e.Id).HasMaxLength(100);
        });

        

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.ToTable("Playlist");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Owner).HasMaxLength(100);

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.Owner)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Playlist_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.ToTable("Song");

            entity.Property(e => e.Id)
                .HasMaxLength(100);

            entity.HasMany(d => d.Albums).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongAlbum_Album"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongAlbum_Song"),
                    j =>
                    {
                        j.HasKey("SongId", "AlbumId");
                        j.ToTable("SongAlbum");
                        j.IndexerProperty<string>("SongId").HasMaxLength(100);
                        j.IndexerProperty<string>("AlbumId").HasMaxLength(100);
                    });

            entity.HasMany(d => d.Artists).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongArtist_Artist"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongArtist_Song"),
                    j =>
                    {
                        j.HasKey("SongId", "ArtistId");
                        j.ToTable("SongArtist");
                        j.IndexerProperty<string>("SongId").HasMaxLength(100);
                        j.IndexerProperty<string>("ArtistId").HasMaxLength(100);
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongGenre_Gerne"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_SongGenre_Song"),
                    j =>
                    {
                        j.HasKey("SongId", "GenreId");
                        j.ToTable("SongGenre");
                        j.IndexerProperty<string>("SongId").HasMaxLength(100);
                        j.IndexerProperty<string>("GenreId").HasMaxLength(100);
                    });

            entity.HasMany(d => d.Playlists).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistSong",
                    r => r.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_PlaylistSong_Playlist"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_PlaylistSong_Song"),
                    j =>
                    {
                        j.HasKey("SongId", "PlaylistId");
                        j.ToTable("PlaylistSong");
                        j.IndexerProperty<string>("SongId").HasMaxLength(100);
                        j.IndexerProperty<string>("PlaylistId").HasMaxLength(100);
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.UserName).HasColumnName("username");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_User_Role");
                
        });

        modelBuilder.Entity<UserEvent>(entity =>
        {
            entity.ToTable("UserEvent");
         
            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(100).HasColumnName("UserId"); ;

            entity.Property(e => e.EventType).HasColumnName("EventType");

            entity.HasOne(d => d.UserNavigation).WithMany(u => u.UserEvents)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.Cascade)
              .HasConstraintName("FK_UserEvent_User");

            entity.HasDiscriminator<int>(e => e.EventType)
               .HasValue<UserSongEvent>(1)
               .HasValue<UserAlbumEvent>(2)
               .HasValue<UserPlaylistEvent>(3);
           
        });
        modelBuilder.Entity<UserSongEvent>(entity =>
        {
            entity.Property(e => e.SongId).HasMaxLength(100);
            entity.HasOne<Song>(e => e.SongNavigation).WithMany(s => s.UserSongEvents)
            .HasForeignKey(e => e.SongId)
            .HasConstraintName("FK_UserSongEvent_Song");
        });
        modelBuilder.Entity<UserAlbumEvent>(entity =>
        {
            entity.Property(e => e.AlbumId).HasMaxLength(100);
            entity.HasOne<Album>(e => e.AlbumNavigation).WithMany(s => s.UserAlbumEvents)
            .HasForeignKey(e => e.AlbumId)
            .HasConstraintName("FK_UserAlbumEvent_Album");
        });
        modelBuilder.Entity<UserPlaylistEvent>(entity =>
        {
            entity.Property(e => e.PlaylistId).HasMaxLength(100);
            entity.HasOne<Playlist>(e => e.PlaylistNavigation).WithMany(s => s.UserPlaylistEvents)
            .HasForeignKey(e => e.PlaylistId)
            .HasConstraintName("FK_UserPlaylistEvent_Playlist");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
