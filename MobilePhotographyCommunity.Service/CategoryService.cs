﻿using MobilePhotographyCommunity.Data.DomainModel;
using MobilePhotographyCommunity.Data.Infrastructure;
using MobilePhotographyCommunity.Data.Repositories;
using MobilePhotographyCommunity.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobilePhotographyCommunity.Service
{
    public interface ICategoryService
    {
        IEnumerable<CategoryVm> GetCategories(int? pageIndex, int pageSize);
        IEnumerable<CategoryVm> Search(string str);
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        IEnumerable<Category> GetByStatus();
        CategoryVm GetCategoryVm(int id);
        void Add(CategoryVm categoryVm);
        void Update(CategoryVm categoryVm);
        void UpdateStatus(int id, bool status);
        void Delete(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(CategoryVm categoryVm)
        {
            var category = new Category();
            category.CategoryName = categoryVm.CategoryName;
            category.Description = categoryVm.Description;
            category.CreatedBy = categoryVm.CreatedBy;
            category.CreatedTime = categoryVm.CreatedTime;
            category.MetaTitle = categoryVm.MetaTitle;
            category.Status = categoryVm.Status;
            categoryRepository.Add(category);
        }

        public void Update(CategoryVm categoryVm)
        {
            var category = categoryRepository.GetById(categoryVm.CategoryId);
            category.CategoryName = categoryVm.CategoryName;
            category.Description = categoryVm.Description;
            category.ModifiedBy = categoryVm.ModifiedBy;
            category.ModifiedTime = categoryVm.ModifiedTime;
            category.MetaTitle = categoryVm.MetaTitle;
            categoryRepository.Update(category);
        }

        public IEnumerable<Category> GetByStatus()
        {
            return categoryRepository.GetByStatus();
        }

        public IEnumerable<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return categoryRepository.GetById(id);
        }

        public IEnumerable<CategoryVm> GetCategories(int? pageIndex, int pageSize)
        {
            var categories = categoryRepository.GetAllPaging(pageIndex, pageSize);
            var categoriesVm = new List<CategoryVm>();
            foreach (var item in categories)
            {
                var categoryVm = new CategoryVm();
                categoryVm.CategoryId = item.CategoryId;
                categoryVm.CategoryName = item.CategoryName;
                categoryVm.Description = item.Description;
                categoryVm.CreatedBy = item.CreatedBy;
                categoryVm.CreatedTime = item.CreatedTime;
                categoryVm.ModifiedBy = item.ModifiedBy;
                categoryVm.ModifiedTime = item.ModifiedTime;
                categoryVm.Status = item.Status;
                categoryVm.User = userRepository.GetById(Convert.ToInt32(item.CreatedBy));
                categoriesVm.Add(categoryVm);
            }

            return categoriesVm;
        }

        public CategoryVm GetCategoryVm(int id)
        {
            var category = categoryRepository.GetById(id);
            var categoryVm = new CategoryVm();
            categoryVm.CategoryId = category.CategoryId;
            categoryVm.CategoryName = category.CategoryName;
            categoryVm.Description = category.Description;
            categoryVm.CreatedBy = category.CreatedBy;
            categoryVm.CreatedTime = category.CreatedTime;
            categoryVm.Status = category.Status;
            return categoryVm;
        }

        public void Delete(int id)
        {
            categoryRepository.Delete(id);
        }

        public void UpdateStatus(int id, bool status)
        {
            var category = categoryRepository.GetById(id);
            category.Status = status;
            categoryRepository.Update(category);
        }

        public IEnumerable<CategoryVm> Search(string str)
        {
            var categories = categoryRepository.Search(str);
            return categories.Select(x => new CategoryVm
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedTime = x.CreatedTime,
                Status = x.Status,
                User = userRepository.GetById(Convert.ToInt32(x.CreatedBy))
            }
            );
        }
    }
}
