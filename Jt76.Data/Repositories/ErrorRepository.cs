// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
// 
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================

namespace Jt76.Data.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using DbContexts;
	using Interfaces;
	using Models.ApplicationDb;

	public class ErrorRepository : Repository<Error>, IErrorRepository
	{
        public ErrorRepository(Jt76ApplicationDbContext context) : base(context)
        { }

		private Jt76ApplicationDbContext appContext => (Jt76ApplicationDbContext)_context;

		public IEnumerable<Error> GetTopErrors(int count)
		{
			return appContext.Errors
				.Take(count)
				.OrderBy(x => x.UpdatedDate)
				.ThenBy(x => x.ErrorLevel)
				.ToList();
		}
	}
}
