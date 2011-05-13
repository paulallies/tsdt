using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace TSDTReports.Models
{
    public interface IResourceRepository
    {
        IQueryable<tbl_Resource> FindResource(int Project_ID, string User_CAI); 
        IQueryable<tbl_Resource> FindAllResources(int Project_ID);
        tbl_Resource GetResource(int Resource_ID);
        IQueryable<tbl_Resource> FindResourcesByCAI(string User_CAI);
        void Add(tbl_Resource resource);
        void Delete(tbl_Resource resource);
        void Save();
    }
}
