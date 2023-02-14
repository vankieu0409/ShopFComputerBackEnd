﻿using Iot.Core.Data.Relational.Repositories.Implements;

using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Data.Repositories.Implements
{
    public class HistoryRepository : Repository<HistoryReadModel>, IHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public HistoryRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
