﻿using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<Podcast> GetPodcast(object id);

        Task<IEnumerable<Podcast>> GetPodcasts();

        Task<int> InsertPodcast(Podcast podcast);

        Task<bool> DeletePodcast(int id);

        Task<bool> DeletePodcast(Podcast podcast);

        Task<bool> UpdatePodcast(Podcast podcast);
    }
}