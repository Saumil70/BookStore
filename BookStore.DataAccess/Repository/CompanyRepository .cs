﻿using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private BookStoreEntites _db;
        public CompanyRepository(BookStoreEntites db) : base(db) 
        {
            _db = db;
        } 


        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
