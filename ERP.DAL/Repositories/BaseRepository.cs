﻿using ERP.DAL.DB.Entities;
using ERP.DAL.DB;
using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class 
    {
        private readonly ERPContext _context;
        public BaseRepository(ERPContext context)
        {
            _context = context;
        }

        public virtual void Add(T item)
        {
            _context.Add(item);
        }

        public virtual async Task Delete(int id)
        {
            var item = await _context.FindAsync<T>(id);
            if (item != null)
            {
                ((IBaseEntity)item).IsActive = false;
                _context.Update(item);
            }
        }

        public virtual IQueryable<T> Get(int id)
        {
            return _context.Set<T>().Where(x => ((IBaseEntity)x).IsActive && ((IBaseEntity)x).Id == id);
        }

        public virtual IQueryable<T> Get()
        {
            return _context.Set<T>().Where(x => ((IBaseEntity)x).IsActive);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public virtual void Update(T item)
        {
            _context.Update(item);
        }

        
    }
}
