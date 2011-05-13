using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public class ResourceRepository : IResourceRepository
    {
        tsdtDataContext db = new tsdtDataContext();

        public IQueryable<tbl_Resource> FindAllResources(int Project_ID)
        {
            return db.tbl_Resources.Where(i => i.Project_ID == Project_ID);
        }

        public IQueryable<tbl_Resource> FindResource(int Project_ID, string User_CAI)
        {
            return db.tbl_Resources.Where(i => i.Project_ID == Project_ID && i.User_CAI == User_CAI);
        }


        public tbl_Resource GetResource(int Resource_ID)
        {
            return db.tbl_Resources.SingleOrDefault(i => i.Resource_ID == Resource_ID);
        }

        public void Add(tbl_Resource resource)
        {
            db.tbl_Resources.InsertOnSubmit(resource);
        }

        public void Delete(tbl_Resource resource)
        {
            db.tbl_Resources.DeleteOnSubmit(resource);
        }

        public void Save()
        {
            db.SubmitChanges();
        }



        public IQueryable<tbl_Resource> FindResourcesByCAI(string User_CAI)
        {
            var resources = db.tbl_Resources.Where(r => r.User_CAI == User_CAI);
            return resources;
        }

    }
}
