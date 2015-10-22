using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Infrastructure.Data;
using Microsoft.AspNet.Identity;

namespace LetsDoOnion.ApiControllers
{
    //[Authorize]
    public class IssueController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public IssueController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        /// Add new issue
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="text">Issue name</param>
        /// <returns></returns>
        public HttpResponseMessage Post(int? id, string text, string userId)
        {
            if (text.IsEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var issue = new Issue()
                {
                    Text = text,
                    CreatedTime = DateTime.Now,
                    IsFinished = false,
                    CategoryId = id,
                    UserId = userId
                };

                _unitOfWork.Issues.Create(issue);
                _unitOfWork.Save();



                var maxId = _unitOfWork.Issues.GetAll().Max(n => n.Id);
                var newId = 1;
                if (_unitOfWork.Issues.GetAll().Any(n => n.Id == maxId && n.CategoryId == id))
                {
                    newId = _unitOfWork.Issues.GetAll().First(n => n.Id == maxId && n.CategoryId == id).Id;
                }
                return Request.CreateResponse(HttpStatusCode.OK, newId);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Set done issue
        /// </summary>
        /// <param name="id">Issue id</param>
        [HttpPut]
        public void Put(int id)
        {
            var issue = _unitOfWork.Issues.Get(id);
            issue.IsFinished = true;

            foreach (var item in _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id))
            {
                item.IsFinished = true;
            }

            _unitOfWork.Save();
        }

        /// <summary>
        /// Unfinish(undone) issue
        /// </summary>
        /// <param name="id">Issue id</param>
        // DELETE api/values/5
        public void Delete(int id)
        {
            var issue = _unitOfWork.Issues.Get(id);
            issue.IsFinished = false;

            foreach (var item in _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id))
            {
                item.IsFinished = false;
            }

            _unitOfWork.Save();
        }

        /// <summary>
        /// Remove completed issues
        /// </summary>
        /// <param name="ids"></param>
        [HttpDelete]
        public void Delete(string ids)
        {
            if (ids.ElementAt(ids.Length - 1) == ',')
                ids = ids.Remove(ids.Length - 1);

            var finishedUIssues = ids.Split(',').Select(Int32.Parse).ToList();
            foreach (var id in finishedUIssues)
            {
                _unitOfWork.UnderIssues.Delete(id);
            }

            _unitOfWork.Save();
        }

    }
}
