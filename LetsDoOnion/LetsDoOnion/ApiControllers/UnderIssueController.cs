using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Infrastructure.Data;

namespace LetsDoOnion.ApiControllers
{
    //[Authorize]
    public class UnderIssueController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public UnderIssueController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: api/UnderIssue/5
        public IEnumerable<UnderIssue> Get(int id)
        {
            var uissues = _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id).ToList();
            return uissues;
        }

        // POST: api/UnderIssue
        public UnderIssue Post(int id, [FromBody]string text)
        {
            var uissue = new UnderIssue()
            {
                Text = text,
                IsFinished = false,
                IssueId = id
            };


            _unitOfWork.UnderIssues.Create(uissue);
            _unitOfWork.Save();

            return _unitOfWork.UnderIssues.GetAll().OrderByDescending(n => n.Id).First(n => n.IssueId == id);
        }

        // PUT: api/UnderIssue/5
        public void Put(int id)
        {
            var issue = _unitOfWork.UnderIssues.Get(id);
            issue.IsFinished = true;
            _unitOfWork.UnderIssues.Update(issue);
            _unitOfWork.Save();
        }

        // DELETE: api/UnderIssue/5
        public void Delete(int id)
        {
            var uIssue = _unitOfWork.UnderIssues.Get(id);
            uIssue.IsFinished = false;

            //if parent issue was finished do it unfinished
            var issue = _unitOfWork.Issues.Get(uIssue.IssueId);
            if (issue.IsFinished)
            {
                issue.IsFinished = false;
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
