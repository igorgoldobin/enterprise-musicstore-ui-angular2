﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using SSW.MusicStore.Models;

namespace SSW.MusicStore.Services.Query
{
	public class AlbumQueryService : IAlbumQueryService
	{
		private readonly IDbContextFactory<MusicStoreContext> _dbContextFactory;

		public AlbumQueryService(IDbContextFactory<MusicStoreContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<IEnumerable<Album>> GetByGenre(string genre)
		{
			using (var dbContext = _dbContextFactory.Create())
			{
				var albums =
					await dbContext.Albums
								 .Where(a => a.Genre.Name.ToUpper().Trim().Equals(genre.ToUpper().Trim()))
								 .ToListAsync();
				return albums;
			}
		}

		public async Task<IEnumerable<Album>> GetTopSellingAlbums(int count)
		{
			using (var dbContext = _dbContextFactory.Create())
			{
				var albums =
					await dbContext.Albums
						.OrderByDescending(a => a.OrderDetails.Count)
						.Take(count)
						.ToListAsync();
				return albums;
			}
		}

		public async Task<Album> GetAlbumDetails(int id)
		{
			using (var dbContext = _dbContextFactory.Create())
			{
				var albums =
					await dbContext.Albums
						.SingleOrDefaultAsync(a => a.AlbumId == id);
				return albums;
			}
		}
	}

}
