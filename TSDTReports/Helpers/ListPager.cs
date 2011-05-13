using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace TSDTReports.Helpers
{
    public static class ListPager
    {
        
        public static string myPager(this HtmlHelper helper, string Message, int total, int CurrentPage, int Pagesize, string Controllerstr, string Actionstr)
        {
            string actionFirst = "/" + Controllerstr + "/" + Actionstr + "/Page/0/size/" + Pagesize.ToString();
            string actionPrev = "/" + Controllerstr + "/" + Actionstr + "/Page/" + (CurrentPage - 1).ToString() + "/size/" + Pagesize.ToString();
            string actionNext = "/" + Controllerstr + "/" + Actionstr + "/Page/" + (CurrentPage + 1).ToString() + "/size/" + Pagesize.ToString();
            string actionLast = "/" + Controllerstr + "/" + Actionstr + "/Page/" + total.ToString() + "/size/" + Pagesize.ToString() ;


            var sb = new StringBuilder();
            sb.Append("<div class='pagerBox'>");
            sb.Append("<div class='fr nw m'>");
            sb.Append("<script>function ChangePage() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/'+p+'/size/'+s; } </script>");
            sb.Append("<script>function ChangePageSize() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/0/size/'+s; } </script>");

            sb.Append("<a  title='first' href='" + actionFirst + "' ><img class='ico_first' src='/images/blank.gif' alt='' complete='complete'/></a>");
            if (CurrentPage > 0)
                sb.Append("<a title='prev' href='" + actionPrev  + "'><img class='ico_prev' src='/images/blank.gif' alt='' complete='complete'/></a>");
            else
                sb.Append("<a title='prev' href='" + actionFirst + "'  ><img class='ico_prev' src='/images/blank.gif' alt='' complete='complete'/></a>");

            sb.Append("<select style='height:22px; color:gray' id='ddlPager' onchange='ChangePage()' >");

            for (int i = 0; i <= total; i++)
            {
                if (CurrentPage == i)
                    sb.Append("<option value='" + i.ToString() + "' selected='selected'>Page " + (i + 1).ToString() + "</option>");
                else
                    sb.Append("<option value='" + i.ToString() + "'>Page " + (i + 1).ToString() + "</option>");

            }
            sb.Append("</select>");
            if (CurrentPage < total)
                sb.Append("<a title='next' href='" + actionNext + "'  ><img class='ico_next' src='/images/blank.gif' alt='' complete='complete'/></a>");
            else
                sb.Append("<a title='next' href='" + actionLast + "'  ><img class='ico_next' src='/images/blank.gif' alt='' complete='complete'/></a>");
            sb.Append("<a title='last' href='" + actionLast + "' ><img class='ico_last' src='/images/blank.gif' alt='' complete='complete'/></a>");

            sb.Append("&nbsp;&nbsp;PageSize:&nbsp;<select style='height:22px; color:gray' id='ddlPageSize' onchange='ChangePageSize()'>");
            if (Pagesize == 10) sb.Append("<option value='10' selected='selected' >10</option>"); else sb.Append("<option value='10'>10</option>");
            if (Pagesize == 15) sb.Append("<option value='15' selected='selected' >15</option>"); else sb.Append("<option value='15'>15</option>");
            if (Pagesize == 20) sb.Append("<option value='20' selected='selected' >20</option>"); else sb.Append("<option value='20'>20</option>");
            sb.Append("</select>");
            sb.Append("</div><span style='float:left'>"+Message +"</span>");

            sb.Append("</div>");
            return sb.ToString();


        }

        public static string myPager(this HtmlHelper helper, string Message, int total, int CurrentPage, int Pagesize, string Controllerstr, string Actionstr, string searchkey, string searchvalue )
        {
            string actionFirst = "/" + Controllerstr + "/" + Actionstr + "/Page/0/size/" + Pagesize.ToString();
            string actionPrev = "/" + Controllerstr + "/" + Actionstr + "/Page/" + (CurrentPage - 1).ToString() + "/size/" + Pagesize.ToString();
            string actionNext = "/" + Controllerstr + "/" + Actionstr + "/Page/" + (CurrentPage + 1).ToString() + "/size/" + Pagesize.ToString();
            string actionLast = "/" + Controllerstr + "/" + Actionstr + "/Page/" + total.ToString() + "/size/" + Pagesize.ToString() ;
            
            string jsChangePage = "<script>function ChangePage() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/'+p+'/size/'+s ; } </script>";
            string jsChangePageSize = "<script>function ChangePageSize() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/0/size/'+s; } </script>";

            if (searchvalue != null || searchvalue.Trim().Length != 0)
            {
                actionFirst += "/"+searchkey + "/" + searchvalue;
                actionPrev += "/" + searchkey + "/" + searchvalue;
                actionNext += "/" + searchkey + "/" + searchvalue;
                actionLast += "/" + searchkey + "/" + searchvalue;
                jsChangePage = "<script>function ChangePage() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/'+p+'/size/'+s+'" + "/" + searchkey + "/" + searchvalue + "' ; } </script>";
                jsChangePageSize = "<script>function ChangePageSize() { var p = $('#ddlPager').val(); var s = $('#ddlPageSize').val();  location.href='/" + Controllerstr + "/" + Actionstr + "/Page/0/size/'+s+'" + "/" + searchkey + "/" + searchvalue + "'; } </script>";

            }
            var sb = new StringBuilder();
            sb.Append("<div class='pagerBox'>");
            sb.Append("<div class='fr nw m'>");
            sb.Append(jsChangePage);
            sb.Append(jsChangePageSize);

            sb.Append("<a  title='first' href='" + actionFirst + "' ><img class='ico_first' src='/images/blank.gif' alt='' complete='complete'/></a>");
            if (CurrentPage > 0)
                sb.Append("<a title='prev' href='" + actionPrev  + "'><img class='ico_prev' src='/images/blank.gif' alt='' complete='complete'/></a>");
            else
                sb.Append("<a title='prev' href='" + actionFirst + "'  ><img class='ico_prev' src='/images/blank.gif' alt='' complete='complete'/></a>");

            sb.Append("<select style='height:22px; color:gray' id='ddlPager' onchange='ChangePage()' >");

            for (int i = 0; i <= total; i++)
            {
                if (CurrentPage == i)
                    sb.Append("<option value='" + i.ToString() + "' selected='selected'>Page " + (i + 1).ToString() + "</option>");
                else
                    sb.Append("<option value='" + i.ToString() + "'>Page " + (i + 1).ToString() + "</option>");

            }
            sb.Append("</select>");
            if (CurrentPage < total)
                sb.Append("<a title='next' href='" + actionNext + "'  ><img class='ico_next' src='/images/blank.gif' alt='' complete='complete'/></a>");
            else
                sb.Append("<a title='next' href='" + actionLast + "'  ><img class='ico_next' src='/images/blank.gif' alt='' complete='complete'/></a>");
            sb.Append("<a title='last' href='" + actionLast + "' ><img class='ico_last' src='/images/blank.gif' alt='' complete='complete'/></a>");

            sb.Append("&nbsp;&nbsp;PageSize:&nbsp;<select style='height:22px; color:gray' id='ddlPageSize' onchange='ChangePageSize()'>");
            if (Pagesize == 10) sb.Append("<option value='10' selected='selected' >10</option>"); else sb.Append("<option value='10'>10</option>");
            if (Pagesize == 15) sb.Append("<option value='15' selected='selected' >15</option>"); else sb.Append("<option value='15'>15</option>");
            if (Pagesize == 20) sb.Append("<option value='20' selected='selected' >20</option>"); else sb.Append("<option value='20'>20</option>");
            sb.Append("</select>");
            sb.Append("</div><span style='float:left'>"+Message +"</span>");

            sb.Append("</div>");
            return sb.ToString();


        }


    }


}
