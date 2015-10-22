using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Infrastructure.Data;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace LetsDoOnion.ApiControllers
{
    //[Authorize]
    public class CategoryController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: api/Category
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Category/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Add category
        /// </summary>
        /// <param name="categoryName">Catefory Name</param>
        // POST: api/Category
        public HttpResponseMessage Post(string categoryName, string userId)
        {
            try
            {
                _unitOfWork.Categories.Create(new Category() { Name = categoryName, UserId = userId });
                _unitOfWork.Save();

                var maxid = _unitOfWork.Categories.GetAll().Max(n => n.Id);
                return Request.CreateResponse(HttpStatusCode.OK, new { Id = maxid });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Remove category
        /// </summary>
        /// <param name="id">CategoryId</param>
        // DELETE: api/Category/5
        public void Delete(int id)
        {
            var issues = _unitOfWork.Issues.GetAll().Where(n => n.CategoryId == id).ToList();
            foreach (var issue in issues)
            {
                _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == issue.Id).ForEach(n => _unitOfWork.UnderIssues.Delete(n.Id));
            }
            issues.ForEach(n => _unitOfWork.Issues.Delete(n.Id));

            _unitOfWork.Categories.Delete(id);
            _unitOfWork.Save();
        }
    }
}
