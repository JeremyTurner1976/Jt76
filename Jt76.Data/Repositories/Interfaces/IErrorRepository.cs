// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
// 
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================

namespace Jt76.Data.Repositories.Interfaces
{
	using System.Collections.Generic;
	using Data.Interfaces;
	using Models.ApplicationDb;

	public interface IErrorRepository : IRepository<Error>
    {
        IEnumerable<Error> GetTopErrors(int count);
	}
}
